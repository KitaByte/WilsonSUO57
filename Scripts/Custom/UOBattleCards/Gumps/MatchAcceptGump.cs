using System.Collections.Generic;

using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Services.UOBattleCards.Core;
using Server.Services.UOBattleCards.Items;

namespace Server.Services.UOBattleCards.Gumps
{
	public class MatchAcceptGump : BaseGump
    {
        private MatchMakeInfo MatchMake { get; set; }

        private BattleDeck Deck { get; set; }


        public static List<BaseGump> AcceptGumpPool = new List<BaseGump>();

        public override bool CloseOnMapChange => true;

        public MatchAcceptGump(PlayerMobile user, MatchMakeInfo info, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)
        {
            MatchMake = info;

            Deck = info.ValidPlayers.Find(p => p.Opponent == user).Deck;

            AcceptGumpPool.Add(this);

            if (counter == -1)
            {
                counter = info.RoundTime + 2;
            }
        }

        public override void AddGumpLayout()
        {
            if (MatchMake != null)
            {
                Disposable = false;
                Closable = false;

                AddBackground(X, Y, 400, 170, 39925);

                AddImage(X + 50, Y + 50, 9012);
                AddImage(X + 290, Y + 50, 5577);

                var message = "UO Battle Cards - Deck Challenged";

                AddLabel(X + GumpUtility.Center(195, message), Y + 15, GumpUtility.HeadHue, message);

                AddButton(X + 160, Y + 50, Settings.EmptySlotUp, Settings.EmptySlotDown, 1, GumpButtonType.Reply, 0);

                AddLabel(X + 200, Y + 50, GumpUtility.SubHue, "Accept");

                AddButton(X + 160, Y + 75, Settings.EmptySlotUp, Settings.EmptySlotDown, 2, GumpButtonType.Reply, 0);

                AddLabel(X + 200, Y + 75, GumpUtility.SubHue, "Decline");

                var wager = $"Wager : {MatchMake.Wager}";

                AddLabel(X + GumpUtility.Center(195, wager), Y + 107, GumpUtility.HeadHue, wager);

                AddLabel(X + GumpUtility.Center(185, GumpUtility.Footer), Y + 135, GumpUtility.SubHue, GumpUtility.Footer);

                Deck.Hue = GumpUtility.HeadHue + 1;
            }
        }

        public override void OnResponse(RelayInfo info)
        {
            Deck.Hue = 2500;

            switch (info.ButtonID)
            {
                case 1: // Accept
                    {
                        if (!MatchMake.Accepted)
                        {
                            MatchMake.Accepted = true;

                            var game = new MatchInfo()
                            {
                                PlayerOne = MatchMake.Player,
                                PlayerOneDeck = MatchMake.Deck,
                                PlayerTwo = User,
                                PlayerTwoDeck = Deck,
                                RoundTimeSet = MatchMake.RoundTime,
                                MatchTick = MatchMake.RoundTime,
                                Wager = MatchMake.Wager
                            };

                            if (MatchMake.Wager > 0)
                            {
                                var gold = User.Backpack.FindItemByType(typeof(Gold));

                                if (gold != null)
                                {
                                    if (gold.Amount < MatchMake.Wager + 1)
                                    {
                                        gold.Amount -= MatchMake.Wager;
                                    }
                                }
                            }

                            GameUtility.StartGame(game);
                        }
                        else
                        {
                            User.SendMessage(52, "Offer no longer exists!");
                        }

                        Close();

                        break;
                    }
                case 2: // Decline
                    {
                        MatchMake.ValidPlayers.Remove((User, Deck));

                        Close();

                        break;
                    }
                default:
                    {
                        Close();
                    }
                    break;
            }
        }

        private int counter = -1;

		public void UpdateGumpTime()
        {
            if (!MatchMake.Accepted && counter > 0)
            {
                counter--;
            }
            else
            {
                MatchMake.ValidPlayers.Remove((User, Deck));

                Deck.Hue = 2500;

                Close();
            }
        }

        public override void Close()
        {
            AcceptGumpPool.Remove(this);

            if (!MatchMake.Accepted && MatchMake.ValidPlayers.Count == 0)
            {
                MatchUtility.LoadAIPlayer(MatchMake);
            }

            base.Close();  
        }
    }
}

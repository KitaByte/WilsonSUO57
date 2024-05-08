using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Services.UOBattleCards.Core;
using Server.Services.UOBattleCards.Items;

namespace Server.Services.UOBattleCards.Gumps
{
    public class MatchMakeGump : BaseGump
    {
        private BattleDeck Deck { get; set; }

        private bool FoilsAllowed { get; set; }

        private ushort MaxRarity { get; set; }

        private ushort MaxLevel { get; set; }

        private int ValueCap { get; set; }

        private ushort RoundTime { get; set; }

        private ushort Wager { get; set; }

        private ushort GoldMax { get; set; }

        public override bool CloseOnMapChange => true;

        public MatchMakeGump(PlayerMobile user, BattleDeck deck, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)
        {
            Deck = deck;

            GoldMax = 0;

            FoilsAllowed = false;
            MaxRarity = 1;
            MaxLevel = 1;
            ValueCap = 20000;
            RoundTime = 3;
            Wager = 0;

            var gold = User.Backpack.FindItemByType(typeof(Gold));

            if (gold != null && gold is Gold g)
            {
                if (g.Amount > 0)
                {
                    GoldMax = (ushort)g.Amount;
                }
            }

            if (GoldMax > 60000)
            {
                GoldMax = 60000;
            }
        }

        public override void AddGumpLayout()
        {
            AddBackground(X, Y, 400, 280, 39925);

            // Title

            AddLabel(X + GumpUtility.Center(190, "UO Battle Cards : Match Setup"), Y + 25, GumpUtility.HeadHue, "UO Battle Cards : Match Setup");

            AddImage(X + 56, Y + 65, 9000, GumpUtility.SubHue);

            // Game Rules

            // Foils

            AddButton(X + 200, Y + 65, Settings.EmptySlotUp, Settings.EmptySlotDown, 1, GumpButtonType.Reply, 0);

            AddLabel(X + 235, Y + 65, FoilsAllowed ? GumpUtility.HeadHue : GumpUtility.SubHue, $"Foils - {FoilsAllowed}");

            // Rarity Max

            AddButton(X + 200, Y + 90, Settings.EmptySlotUp, Settings.EmptySlotDown, 2, GumpButtonType.Reply, 0);

            AddLabel(X + 235, Y + 90, MaxRarity == 10 ? GumpUtility.HeadHue : GumpUtility.SubHue, $"Max Rarity - {MaxRarity}");

            // Level Max

            AddButton(X + 200, Y + 115, Settings.EmptySlotUp, Settings.EmptySlotDown, 3, GumpButtonType.Reply, 0);

            AddLabel(X + 235, Y + 115, MaxLevel == 10 ? GumpUtility.HeadHue : GumpUtility.SubHue, $"Max Level - {MaxLevel}");

            // Deck Value Cap

            AddButton(X + 200, Y + 140, Settings.EmptySlotUp, Settings.EmptySlotDown, 4, GumpButtonType.Reply, 0);

            AddLabel(X + 235, Y + 140, ValueCap == 0 ? GumpUtility.HeadHue : GumpUtility.SubHue, $"Value Cap - {ValueCap}");

            // Round Time

            AddButton(X + 200, Y + 165, Settings.EmptySlotUp, Settings.EmptySlotDown, 5, GumpButtonType.Reply, 0);

            AddLabel(X + 235, Y + 165, RoundTime == 0 ? GumpUtility.HeadHue : GumpUtility.SubHue, $"Round Time - {RoundTime}");

            // Match Wager

            if (GoldMax > 0)
            {
                AddButton(X + 200, Y + 190, Settings.EmptySlotUp, Settings.EmptySlotDown, 6, GumpButtonType.Reply, 0);

                AddLabel(X + 235, Y + 190, Wager > 0 ? GumpUtility.HeadHue : GumpUtility.SubHue, $"Wager - {Wager}");
            }

            // Game Info

            AddButton(X + 80, Y + 190, Settings.EmptySlotUp, Settings.EmptySlotDown, 7, GumpButtonType.Reply, 0);

            AddLabel(X + 115, Y + 190, GumpUtility.HeadHue, $"Instructions");

            // Game Start / Find Match

            AddButton(X + 80, Y + 215, Settings.EmptySlotUp, Settings.EmptySlotDown, 8, GumpButtonType.Reply, 0);

            AddLabel(X + 115, Y + 215, GumpUtility.HeadHue, $"Start Match");

            // Footer

            AddLabel(X + GumpUtility.Center(180, GumpUtility.Footer), Y + 245, GumpUtility.SubHue, GumpUtility.Footer);
        }

        public override void OnResponse(RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 1: // Foils
                    {
                        if (FoilsAllowed)
                        {
                            FoilsAllowed = false;
                        }
                        else
                        {
                            FoilsAllowed = true;
                        }

                        Refresh(true, false);

                        break;
                    }
                case 2: // Rarity
                    {
                        if (MaxRarity < 10)
                        {
                            MaxRarity++;
                        }
                        else
                        {
                            MaxRarity = 1;
                        }

                        Refresh(true, false);

                        break;
                    }
                case 3: // Level
                    {
                        if (MaxLevel < 10)
                        {
                            MaxLevel++;
                        }
                        else
                        {
                            MaxLevel = 1;
                        }

                        Refresh(true, false);

                        break;
                    }
                case 4: // Value
                    {
                        if (ValueCap < 10000000)
                        {
                            if (ValueCap < 100000)
                            {
                                ValueCap += 10000;
                            }
                            else
                            {
                                if (ValueCap < 1000000)
                                {
                                    ValueCap += 100000;
                                }
                                else
                                {
                                    ValueCap += 1000000;
                                }
                            }
                        }
                        else
                        {
                            ValueCap = 0;
                        }

                        Refresh(true, false);

                        break;
                    }
                case 5: // Time
                    {
                        if (RoundTime < 9)
                        {
                            RoundTime++;
                        }
                        else
                        {
                            RoundTime = 0;
                        }

                        Refresh(true, false);

                        break;
                    }
                case 6: // Wager
                    {
                        if (Wager > 0 && Wager < GoldMax)
                        {
                            Wager *= 2;

                            if (Wager > GoldMax)
                            {
                                Wager = GoldMax;
                            }
                        }
                        else
                        {
                            if (Wager == 0)
                            {
                                Wager = 5 < GoldMax ? (ushort)5 : GoldMax;
                            }
                            else
                            {
                                Wager = 0;
                            }
                        }

                        Refresh(true, false);

                        break;
                    }
                case 7: // Instructions
                    {
						User.LaunchBrowser(@"https://sites.google.com/view/uo-battle-cards/instructions");

                        Refresh(true, false);

                        break;
                    }
                case 8: // Start
                    {
                        var match = new MatchMakeInfo()
                        {
                            Player = User,
                            Deck = Deck,
                            FoilAllowed = FoilsAllowed,
                            MaxRarity = MaxRarity,
                            MaxLevel = MaxLevel,
                            ValueCap = ValueCap,
                            RoundTime = RoundTime,
                            Wager = Wager
                        };

                        if (Wager > 0)
                        {
                            var gold = User.Backpack.FindItemByType(typeof(Gold));

                            if (gold != null)
                            {
                                if (gold.Amount < Wager + 1)
                                {
                                    gold.Amount -= Wager;
                                }
                            }
                        }

                        MatchUtility.MatchMaking.Add(match);

                        User.SendMessage(52, "Match Making Started, looking for opponents...");

                        break;
                    }
                default:
                    break;
            }
        }
    }
}

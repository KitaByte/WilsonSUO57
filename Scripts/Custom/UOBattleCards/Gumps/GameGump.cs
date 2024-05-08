using Server.Gumps;
using Server.Mobiles;
using Server.Services.UOBattleCards.Cards.Types;
using Server.Services.UOBattleCards.Core;
using System;

namespace Server.Services.UOBattleCards.Gumps
{
    public class GameGump : BaseGump
    {
        private MatchInfo Game { get; set; }

        public override bool CloseOnMapChange => true;

        private readonly bool IsPlayerOne = false;

        public GameGump(PlayerMobile user, MatchInfo info, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)
        {
            Game = info;

            if (Game.PlayerOne == user)
            {
                Game.P1GameGumpHandle = this;

                IsPlayerOne = true;
            }
            else
            {
                Game.P2GameGumpHandle = this;

                IsPlayerOne = false;
            }
        }

        public override void AddGumpLayout()
        {
            Disposable = false;
            Closable = false;

            // Size 574 x 485
            AddImage(X, Y, Settings.MatchArt, 0); // Outer Background

            var offSet = Game.Round > 27 ? 30 : Game.Round + 1;

            AddBackground(X + 45, Y + 20, 484, 413 - offSet, 0x238C); // Inner Background

            AddItem(X + 38 - offSet / 10, Y + 421 - offSet, GetSkelly(true));
            AddItem(X + 506 + offSet / 10, Y + 421 - offSet, GetSkelly(false));

            var firstDigit = Game.Round / 10;
            var secondDigit = Game.Round % 10;

            var roundColor = Game.Round > 29 ? 2750 : 2497;

            AddImage(X + 269, Y + 9, 1423 + firstDigit, roundColor); // Digit tens
            AddImage(X + 285, Y + 9, 1423 + secondDigit, roundColor); // Digit ones

            AddBackground(X + 60, Y + 48, 455, 24, 0x2560); // Vs Background // Gold Trim = 0x251C

            AddImage(X + 252, Y - 44, 9000, Game.IsRoundStart ? GumpUtility.SubHue : GumpUtility.HeadHue); // UO Logo

            var p1Name = Game.PlayerOne.Name;
            var p2Name = Game.IsAI ? "AI" : Game.PlayerTwo.Name;

            AddLabel(X + GumpUtility.Center(173, p1Name), Y + 53, p1Name == User.Name ? GumpUtility.HeadHue : GumpUtility.SubHue, p1Name);

            AddLabel(X + 278, Y + 53, Game.IsRoundStart ? GumpUtility.HeadHue : GumpUtility.SubHue, "VS");

            AddLabel(X + GumpUtility.Center(418, p2Name), Y + 53, p2Name == User.Name ? GumpUtility.HeadHue : GumpUtility.SubHue, p2Name);

            var deckOne = Game.PlayerOneDeck;

            var deckTwo = Game.PlayerTwoDeck;

            // Attack Button Art
            var atkBtnUp = 5585;
            var atkBtnDown = 5586;

            // Block Button Art
            var blkBtnUp = 5587;
            var blkBtnDown = 5588;

            // Pass Button Art
            var passBtnUp = 5581;
            var passBtnDown = 5582;

            // Next Round Button Art
            var rndBtnUp = 5577;
            var rndBtnDown = 5578;

            // End Game Button Art
            var endBtnUp = 5545;
            var endBtnDown = 5545;

            // Winning Ribbon Art
            var ribbonArt = 9004;

            if (Game.IsRoundStart)
            {
                User.SendMessage(62, $"Starting Round {Game.Round}");

                if (IsPlayerOne)
                {
                    var cardOne = deckOne.DrawCard(Game.Round, false);

                    GumpUtility.LoadCardGump(Game, User, cardOne.Info, this, X + 51, Y + 75);

                    AddImage(X + 300, Y + 75, Settings.CardBack);

                    if (cardOne is CreatureCard)
                    {
                        AddButton(X + 133, Y + 407, atkBtnUp, atkBtnDown, 1, GumpButtonType.Reply, 0);

                        AddButton(X + 383, Y + 407, blkBtnUp, blkBtnDown, 2, GumpButtonType.Reply, 0);
                    }
                    else
                    {
                        AddButton(X + 257, Y + 407, passBtnUp, passBtnDown, 3, GumpButtonType.Reply, 0);
                    }
                }
                else
                {
                    var cardTwo = deckTwo.DrawCard(Game.Round, Game.IsAI);

                    AddImage(X + 51, Y + 75, Settings.CardBack);

                    GumpUtility.LoadCardGump(Game, User, cardTwo.Info, this, X + 300, Y + 75);

                    if (cardTwo is CreatureCard)
                    {
                        AddButton(X + 133, Y + 407, atkBtnUp, atkBtnDown, 1, GumpButtonType.Reply, 0);

                        AddButton(X + 383, Y + 407, blkBtnUp, blkBtnDown, 2, GumpButtonType.Reply, 0);
                    }
                    else
                    {
                        AddButton(X + 257, Y + 407, passBtnUp, passBtnDown, 3, GumpButtonType.Reply, 0);
                    }
                }

                AddImage(X + 1, Y - 20, 0x28C8, 2499); // Dragon Left
                AddImage(X + 491, Y - 20, 0x28C9, 2499); // Dragon Right
            }
            else
            {
                User.SendMessage(52, $"Resolved Round {Game.Round}");

                if (User == Game.PlayerOne)
                {
                    GumpUtility.LoadCardGump(Game, User, deckOne.DrawCard(Game.Round, false).Info, this, X + 51, Y + 75);

                    GumpUtility.LoadCardGump(Game, User, deckTwo.DrawCard(Game.Round, Game.IsAI).Info, this, X + 300, Y + 75);
                }
                else
                {
                    GumpUtility.LoadCardGump(Game, User, deckOne.DrawCard(Game.Round, false).Info, this, X + 51, Y + 75);

                    GumpUtility.LoadCardGump(Game, User, deckTwo.DrawCard(Game.Round, Game.IsAI).Info, this, X + 300, Y + 75);
                }

                // Show Round Winner

                var p1Stats = Game.PlayerOneStats.LastMatchPoints.ToString();
                var p2Stats = Game.PlayerTwoStats.LastMatchPoints.ToString();
                var outcome = Game.RoundWinner;

                AddImage(X + 1, Y - 20, 0x28C8, outcome == PlayerTypes.PlayerOne ? GumpUtility.HeadHue : GumpUtility.SubHue); // Dragon Left
                AddImage(X + 491, Y - 20, 0x28C9, outcome == PlayerTypes.PlayerTwo ? GumpUtility.HeadHue : GumpUtility.SubHue); // Dragon Right

                AddImage(X + 145, Y + 419, ribbonArt, outcome == PlayerTypes.PlayerOne ? GumpUtility.HeadHue : GumpUtility.SubHue); // Ribbon
                AddLabel(X + GumpUtility.Center(163, p1Stats), Y + 430, outcome == PlayerTypes.PlayerOne ? GumpUtility.SubHue : GumpUtility.HeadHue, p1Stats);

                AddImage(X + 395, Y + 419, ribbonArt, outcome == PlayerTypes.PlayerTwo ? GumpUtility.HeadHue : GumpUtility.SubHue); // Ribbon
                AddLabel(X + GumpUtility.Center(413, p2Stats), Y + 430, outcome == PlayerTypes.PlayerTwo ? GumpUtility.SubHue : GumpUtility.HeadHue, p2Stats);

                if (Game.Round < 32)
                {
                    AddButton(X + 257, Y + 407, rndBtnUp, rndBtnDown, 3, GumpButtonType.Reply, 0);
                }
                else
                {
                    AddButton(X + 257, Y + 407, endBtnUp, endBtnDown, 4, GumpButtonType.Reply, 0);
                }
            }

            if (Game.RoundTimeSet > 0)
            {
                Game.MatchTick = Game.RoundTimeSet;

                if (IsPlayerOne)
                {
                    SendGump(Game.PlayerOneTimeG = new RoundTimerGump(User, Game));
                }
                else
                {
                    SendGump(Game.PlayerTwoTimeG = new RoundTimerGump(User, Game));
                }
            }
        }

        public override void OnResponse(RelayInfo info)
		{
			if (IsPlayerOne)
			{
				Game.PlayerOneReady = true;
			}
			else
			{
				Game.PlayerTwoReady = true;
			}

			if (Game.IsAI && !Game.PlayerTwoReady)
			{
				Game.PlayerTwoReady = true;
			}

			switch (info.ButtonID)
            {
                case 0:
                    {
                        //Refresh(true, false);

                        break;
                    }
                case 1: // Attack
					{
						AttackRound();

						break;
					}
				case 2: // Block
					{
						BlockRound();

						break;
					}
				case 3: // Pass
                    {
                        PassRound();

                        break;
                    }
                case 4: // End Game
                    {
                        EndMatch();

                        break;
                    }
                default:
                    break;
            }
        }

		public void AttackRound()
		{
			if (User.HasGump(typeof(RoundTimerGump)))
			{
				User.CloseGump(typeof(RoundTimerGump));
			}

			if (IsPlayerOne)
			{
				Game.PlayerOneChoice = (true, false, false);

				if (Game.PlayerOneDeck.CardDeck[0] is CreatureCard card)
				{
					User.PlaySound(CreatureUtility.GetInfo(card.Info.Creature).C_AtkSound);
				}
			}
			else
			{
				Game.PlayerOneChoice = (true, false, false);

				if (Game.IsAI)
				{
					if (Game.PlayerTwoDeck.AIDeck[0] is CreatureCard card)
					{
						User.PlaySound(CreatureUtility.GetInfo(card.Info.Creature).C_AtkSound);
					}
				}
				else
				{
					if (Game.PlayerTwoDeck.CardDeck[0] is CreatureCard card)
					{
						User.PlaySound(CreatureUtility.GetInfo(card.Info.Creature).C_AtkSound);
					}
				}
			}

			User.SendMessage(82, "You Attack!");

			if (Game.PlayerOneReady && Game.PlayerTwoReady)
			{
				GameUtility.UpdateMatch(Game);
			}
		}

		public void BlockRound()
		{
			if (User.HasGump(typeof(RoundTimerGump)))
			{
				User.CloseGump(typeof(RoundTimerGump));
			}

			if (IsPlayerOne)
			{
				Game.PlayerOneChoice = (false, true, false);

				if (Game.PlayerOneDeck.CardDeck[0] is CreatureCard card)
				{
					User.PlaySound(CreatureUtility.GetInfo(card.Info.Creature).C_BlockSound);
				}
			}
			else
			{
				Game.PlayerOneChoice = (false, true, false);

				if (Game.IsAI)
				{
					if (Game.PlayerTwoDeck.AIDeck[0] is CreatureCard card)
					{
						User.PlaySound(CreatureUtility.GetInfo(card.Info.Creature).C_BlockSound);
					}
				}
				else
				{
					if (Game.PlayerTwoDeck.CardDeck[0] is CreatureCard card)
					{
						User.PlaySound(CreatureUtility.GetInfo(card.Info.Creature).C_BlockSound);
					}
				}
			}

			User.SendMessage(82, "You Block!");

			if (Game.PlayerOneReady && Game.PlayerTwoReady)
			{
				GameUtility.UpdateMatch(Game);
			}
		}

		public void PassRound()
		{
			if (User.HasGump(typeof(RoundTimerGump)))
			{
				User.CloseGump(typeof(RoundTimerGump));
			}

			if (IsPlayerOne)
            {
                Game.PlayerOneChoice = (false, false, true);
            }
            else
            {
                Game.PlayerTwoChoice = (false, false, true);
            }

            if (Game.IsRoundStart)
            {
                User.SendMessage(82, "You Pass!");
            }
            else
            {
                User.SendMessage(52, "---------------");
            }

			if (Game.PlayerOneReady && Game.PlayerTwoReady)
			{
				GameUtility.UpdateMatch(Game);
			}
        }

		public void EndMatch()
		{
			if (User.HasGump(typeof(RoundTimerGump)))
			{
				User.CloseGump(typeof(RoundTimerGump));
			}

			User.Say(52, $"GG");

			if (Game.PlayerOneReady && Game.PlayerTwoReady)
            {
                GameUtility.EndGame(Game);
            }
        }

        private int GetSkelly(bool isLeft)
        {
            if (isLeft) // 0x1B1D // 0X1A01 // 0x1A04 // 0x1A05 // 0x1A09
            {
                if (Game.Round < 7)
                {
                    return 0x1B1D;
                }

                if (Game.Round < 14)
                {
                    return 0X1A01;
                }

                if (Game.Round < 21)
                {
                    return 0x1A04;
                }

                if (Game.Round < 28)
                {
                    return 0x1A05;
                }

                if (Game.Round < 33)
                {
                    return 0x1A09;
                }

                return 0x1B1D;
            }
            else // 0x1B1E // 0X1A02 // 0x1A03 // 0x1A06 // 0x1A0A
            {
                if (Game.Round < 7)
                {
                    return 0x1B1E;
                }

                if (Game.Round < 14)
                {
                    return 0X1A02;
                }

                if (Game.Round < 21)
                {
                    return 0x1A03;
                }

                if (Game.Round < 28)
                {
                    return 0x1A06;
                }

                if (Game.Round < 33)
                {
                    return 0x1A0A;
                }

                return 0x1B1E;
            }
        }

        public override void Close()
        {
            base.Close();
        }
    }
}

using Server.Services.UOBattleCards.Gumps;
using Server.Services.UOBattleCards.Items;

namespace Server.Services.UOBattleCards.Core
{
    public enum PlayerTypes
    {
        None = 0,
        PlayerOne = 1,
        PlayerTwo = 2
    }

    public class MatchInfo
    {
        // Player One
        public Mobile PlayerOne { get; set; }

        public BattleDeck PlayerOneDeck { get; set; }

        public PlayerInfo PlayerOneStats { get; set; }

        public bool PlayerOneReady { get; set; }

        public (bool Attack, bool Block, bool Pass) PlayerOneChoice { get; set; }

        public GameGump P1GameGumpHandle { get; set; }

        public RoundTimerGump PlayerOneTimeG { get; set; }

        // Player Two
        public Mobile PlayerTwo { get; set; }

        public BattleDeck PlayerTwoDeck { get; set; }

        public PlayerInfo PlayerTwoStats { get; set; }

        public bool PlayerTwoReady { get; set; }

        public (bool Attack, bool Block, bool Pass) PlayerTwoChoice { get; set; }

        public GameGump P2GameGumpHandle { get; set; }

        public RoundTimerGump PlayerTwoTimeG { get; set; }

        // Round Info
        public bool IsAI { get; set; } = false;

        public ushort Round { get; private set; } = 1;

        public PlayerTypes RoundWinner { get; set; }

        public PlayerTypes RoundEndedBy { get; set; }

        public ushort Wager { get; set; }

		private ushort NextRound()
		{
			if (Round < 32)
			{
				Round++;
			}
			else
			{
				Round = 0; // End Game
			}

			return Round;
		}

		public bool IsRoundStart { get; private set; } = true;

		public (ushort Round, bool RoundStart) UpdateMatchPosition()
		{
			ResetTurn();

			if (Round == 0)
			{
				return (0, true); // End Match
			}
			else if (IsRoundStart)
			{
				IsRoundStart = false;

				return (Round, false);
			}
			else
			{
				var lastRnd = Round;

				NextRound();

				IsRoundStart = true;

				return (lastRnd, true);
			}
		}

		public ushort RoundTimeSet { get; set; }

        public ushort MatchTick { get; set; }

        public void RoundTick()
        {
            RefreshTimerGump();

            if (MatchTick > 0)
            {
                MatchTick--;
            }
            else
			{
				var isEndRound = Round == 32 && !IsRoundStart;

				if (Round > 0 && Round < 33 && !isEndRound)
				{
					if (!PlayerOneReady)
					{
						PlayerOneReady = true;

						P1GameGumpHandle?.PassRound();
					}

					if (!PlayerTwoReady)
					{
						PlayerTwoReady = true;

						P2GameGumpHandle?.PassRound();
					}

					MatchTick = RoundTimeSet;
				}
                else
				{
					RoundTimeSet = 0;

					if (!PlayerOneReady)
					{
						PlayerOneReady = true;

						P1GameGumpHandle?.EndMatch();
					}

					if (!PlayerTwoReady)
					{
						PlayerTwoReady = true;

						P2GameGumpHandle?.EndMatch();
					}
				}
            }
        }

        private void RefreshTimerGump()
        {
            if (!PlayerOneReady)
                PlayerOneTimeG?.Refresh(true, false);

            if (!PlayerTwoReady)
                PlayerTwoTimeG?.Refresh(true, false);
        }

        private void ResetTurn()
        {
            PlayerOneReady = false;

            PlayerOneChoice = (false, false, false);

            PlayerTwoReady = false;

            PlayerTwoChoice = (false, false, false);
        }
    }
}

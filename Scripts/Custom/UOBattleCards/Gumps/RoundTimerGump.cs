using Server.Gumps;
using Server.Mobiles;
using Server.Services.UOBattleCards.Core;

namespace Server.Services.UOBattleCards.Gumps
{
	public class RoundTimerGump : BaseGump
	{
		private MatchInfo Game { get; set; }

		public RoundTimerGump(PlayerMobile user, MatchInfo info, int x = 25, int y = 25) : base(user, x, y)
		{
			Game = info;
		}

		public override void AddGumpLayout()
		{
			Disposable = false;
			Closable = false;
			Dragable = false;

			if (Game.MatchTick % 2 == 0)
			{
				if (Game == null || Game.Round < 30)
				{
					AddImage(X, Y, 0x918, 2499);
				}
				else
				{
					AddImage(X, Y, 0x918, 2750);
				}
			}
			else
			{
				if (Game == null || Game.Round < 30)
				{
					AddImage(X, Y, 0x919, 2499);
				}
				else
				{
					AddImage(X, Y, 0x919, 2750);
				}
			}

			AddAlphaRegion(X, Y, 80, 60);

			AddLabel(X + 37, Y + 23, 52, Game.MatchTick.ToString());

			AddImage(X - 3, Y + 51, 0x151);

			AddButton(X + 23, Y + 63, 0x158B, 0x158A, 1, GumpButtonType.Reply, 0);
		}

        public override void OnResponse(RelayInfo info)
		{
			switch (info.ButtonID)
			{
				case 0: { break; }
				case 1:
					{
                        Game.RoundEndedBy = Game.PlayerOne == User ? PlayerTypes.PlayerOne : PlayerTypes.PlayerTwo;

						GameUtility.EndGame(Game);

						break; 
					}
				default: break;
			}
		}
	}
}

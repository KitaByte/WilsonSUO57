using System;

using Server.Services.UOBattleCards.Gumps;

namespace Server.Services.UOBattleCards.Core
{
    public class RoundTimer : Timer
    {
        public RoundTimer() : base(TimeSpan.FromSeconds(2))
        {
            CoreUtility.LogMessage(ConsoleColor.DarkMagenta, "UO Battle Cards : [Game Timer - Created]");
        }

        protected override void OnTick()
        {
            var match = MatchUtility.CurrentMatches();

            if (match.Count > 0)
            {
                for (int i = 0; i < match.Count; i++)
                {
                    if (match[i] is MatchInfo info && info.RoundTimeSet > 0)
                        match[i].RoundTick();
                }
            }

            if (MatchAcceptGump.AcceptGumpPool.Count > 0)
            {
                for (int i = 0; i < MatchAcceptGump.AcceptGumpPool.Count; i++)
                {
                    if (MatchAcceptGump.AcceptGumpPool[i] != null)
                    {
                        if (MatchAcceptGump.AcceptGumpPool[i] is MatchAcceptGump gump)
                            gump.UpdateGumpTime();
                    }
                }
            }

            if (MatchUtility.MatchMaking.Count > 0)
            {
                for (int i = 0; i < MatchUtility.MatchMaking.Count; i++)
                {
                    MatchUtility.FindMatch(i);
                }
            }

            Start();
        }
    }
}

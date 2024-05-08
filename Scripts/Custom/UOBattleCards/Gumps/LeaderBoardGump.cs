using Server.Gumps;
using Server.Mobiles;
using Server.Services.UOBattleCards.Core;
using Server.Services.UOBattleCards.Items;
using System.Collections.Generic;
using System.Text;

namespace Server.Services.UOBattleCards.Gumps
{
	public class LeaderBoardGump : BaseGump
    {
        private BattleDeck Deck { get; set; }

        public LeaderBoardGump(PlayerMobile user, BattleDeck deck, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)
        {
            Deck = deck;
        }

        public override void AddGumpLayout()
        {
            AddBackground(X, Y, 400, 475, 39925);

            AddLabel(X + GumpUtility.Center(195, "UO Battle Cards : Leaderboard"), Y + 10, GumpUtility.HeadHue, "UO Battle Cards : Leaderboard");

            var leaders = new Dictionary<int, PlayerInfo>();

            if (PlayerUtility.PlayerStats?.Count > 0)
            {
                foreach (var info in PlayerUtility.PlayerStats)
                {
					var rank = info.GetRank();

					if (!leaders.ContainsKey(rank) && rank != 0)
					{
						leaders.Add(rank, info);
					}
                }
            }

            var leaderList = new StringBuilder();

            leaderList.AppendLine($"<basefont color=#{Settings.GetFontColor(true)}><Center>Rank-Name:Matches:Wins:Ties:Loses:Score</Center>");

            leaderList.AppendLine($"<basefont color=#{Settings.GetFontColor(true)}><Center>-----------------------------------------------------</Center>");

            if (leaders.Count > 0 )
            {
                for (var i = 1; i <= leaders.Count; i++)
                {
                    leaders.TryGetValue(i, out PlayerInfo stats);

                    if (stats != null)
                    {
                        var comboStats = $"{i} - {stats.Name} : {stats.MatchesPlayed} : {stats.Wins} : {stats.Ties} : {stats.Loses} : {stats.TotalPoints}";

                        leaderList.AppendLine($"<basefont color=#{Settings.GetFontColor(false)}>{comboStats}");
                    }
                }
            }

            leaderList.AppendLine($"<basefont color=#{Settings.GetFontColor(true)}><Center>-----------------------------------------------------</Center>");

            AddHtml(X + 25, 50, 350, 350, leaderList.ToString(), false, true);

            AddLabel(X + 35, Y + 415, GumpUtility.HeadHue, "Deck Name : ");

            AddTextEntry(X + 125, Y + 415, 225, 25, GumpUtility.SubHue, 1, Deck.Name);

            AddLabel(X + GumpUtility.Center(185, GumpUtility.Footer), Y + 450, GumpUtility.HeadHue, GumpUtility.Footer);
        }

        public override void OnResponse(RelayInfo info)
        {
            base.OnResponse(info);

            if (info.TextEntries.Length > 0 && info.GetTextEntry(1).Text.Length > 3)
            {
                Deck.Name = info.GetTextEntry(1).Text;
            }
            else
            {
                Deck.Name = $"{User.Name}'s Battle Deck";
            }
        }

        public override void Close()
        {
            base.Close();
        }
    }
}

using System.Collections.Generic;
using System.IO;

namespace Server.Services.UOBattleCards.Core
{
    public class PlayerInfo
    {
		public string Name { get; set; }

		public int BestMatchPoints { get; set; }

		public int LastMatchPoints { get; set; }

		public int MatchesPlayed { get; set; }

		public int TotalPoints { get; set; }

		public int Wins { get; set; }

		public int Ties { get; set; }

		public int Loses { get; set; }

		public PlayerInfo(string name)
        {
            Name = name;
        }

		public void UpdateInfo(int win, int tie, int lose)
        {
            MatchesPlayed++;

            Wins += win;
            Ties += tie;
            Loses += lose;

            TotalPoints += LastMatchPoints;

            if (BestMatchPoints < LastMatchPoints)
            {
                BestMatchPoints = LastMatchPoints;
            }
        }

		public int TotalScore()
        {
            if (TotalPoints > 0 && MatchesPlayed > 0)
            {
                var perMatch = TotalPoints / MatchesPlayed;

                var winMod = perMatch + (Wins - Loses);
                var tieMod = perMatch + (Ties - Loses);
                var loseMod = perMatch - (Loses * 2);
				var mod = tieMod - loseMod > 0 ? tieMod - loseMod : 1;


				var total = (winMod * mod) / MatchesPlayed;

                return total;
            }

            return 0;
        }

		public int GetRank()
        {
            if (PlayerUtility.PlayerStats.Count > 0)
            {
				var count = 1;

				foreach (var info in PlayerUtility.PlayerStats)
				{
					if (info != this)
					{
						var score1 = TotalScore();

						var score2 = info.TotalScore();

						if (score1 < score2)
						{
							count++;
						}

						if (score1 == score2)
						{
							if (Name.Length < info.Name.Length)
							{
								count++;
							}

							if (Name.Length == info.Name.Length)
							{
								if (MatchesPlayed < info.MatchesPlayed)
								{
									count++;
								}
							}
						}
					}
				}

				return count;
            }
            else
            {
                return 0;
            }
        }
    }

    public static class PlayerUtility
    {
        private static readonly string FilePath = Path.Combine(@"Saves\UOBattleCard", $"PlayerStats.bin");

		public static List<PlayerInfo> PlayerStats = new List<PlayerInfo>();

        public static void LoadGameInfo()
        {
            Persistence.Deserialize(FilePath, OnDeserialize);
        }

        private static void OnDeserialize(GenericReader reader)
        {
            var count = reader.ReadInt();

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var info = new PlayerInfo(reader.ReadString())
                    {
                        BestMatchPoints = reader.ReadInt(),
                        MatchesPlayed = reader.ReadInt(),
                        TotalPoints = reader.ReadInt(),
                        Wins = reader.ReadInt(),
                        Ties = reader.ReadInt(),
                        Loses = reader.ReadInt()
                    };

                    if (PlayerStats != null)
                    {
                        PlayerStats.Add(info);
                    }
                    else
                    {
                        PlayerStats = new List<PlayerInfo>
                        {
                            info
                        };
                    }
                }
            }
        }

        public static void SaveGameInfo()
        {
            Persistence.Serialize(FilePath, OnSerialize);
        }

        private static void OnSerialize(GenericWriter writer)
        {
            writer.Write(PlayerStats.Count);

            if (PlayerStats.Count > 0)
            {
                foreach (var info in PlayerStats)
                {
                    writer.Write(info.Name);
                    writer.Write(info.BestMatchPoints);
                    writer.Write(info.MatchesPlayed);
                    writer.Write(info.TotalPoints);
                    writer.Write(info.Wins);
                    writer.Write(info.Ties);
                    writer.Write(info.Loses);
                }
            }
        }
    }
}

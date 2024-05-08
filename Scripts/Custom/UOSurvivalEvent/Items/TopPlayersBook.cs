using System.Linq;
using Server.Items;
using System.Collections.Generic;

namespace Server.Custom.UOSurvivalEvent.Items
{
    [Flipable(0xA92D, 0xA92E)]
    internal class TopPlayersBook : Static, IFlipable
    {
        private static List<SurvivalLeaderEntity> TopPlayers = new List<SurvivalLeaderEntity>();

        private static TopPlayersBook b_Instance;

        internal static void ValidateTopPlayer(SurvivalPlayer player)
        {
            if (TopPlayers == null)
            {
                TopPlayers = new List<SurvivalLeaderEntity>();
            }

            if (TopPlayers.Count == 0)
            {
                TopPlayers.Add(new SurvivalLeaderEntity(player.S_Player.Name, player.Kills, player.Deaths, player.Points));
            }
            else
            {
                for (int i = 0; i < TopPlayers.Count; i++)
                {
                    if (TopPlayers[i].Points < player.Points)
                    {
                        TopPlayers.Insert(i, new SurvivalLeaderEntity(player.S_Player.Name, player.Kills, player.Deaths, player.Points));

                        break;
                    }
                }
            }

            if (TopPlayers.Count > 25)
            {
                TopPlayers.Remove(TopPlayers.Last());
            }
        }

        [Constructable]
        public TopPlayersBook() : base(0xA92D)
        {
            Name = "Survival Island - Top 25";

            Hue = 2500;

            Movable = false;

            if (b_Instance == null)
            {
                b_Instance = this;
            }
            else
            {
                World.Broadcast(43, false, "Already have book placed in world!");

                Delete();
            }
        }

        public void OnFlip(Mobile m)
        {
            if (ItemID == 0xA92D)
            {
                base.ItemID = 0xA92E;
            }
            else
            {
                base.ItemID = 0xA92D;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (TopPlayers.Count > 0)
            {
                from.SendMessage(33, "Top 25 Players");

                for (int i = 0; i < TopPlayers.Count; i++)
                {
                    from.SendMessage(53, "--------------------------------");
                    from.SendMessage(63, $"{i + 1}. {TopPlayers[i].Name}");
                    from.SendMessage(63, $"{i + 1}. {TopPlayers[i].Kills} - Kills");
                    from.SendMessage(63, $"{i + 1}. {TopPlayers[i].Deaths} - Deaths");
                    from.SendMessage(63, $"{i + 1}. {TopPlayers[i].Points} - Points");
                    from.SendMessage(53, "--------------------------------");
                }
            }
            else
            {
                from.SendMessage(43, "Leaderboard - Empty!");
                from.SendMessage(53, "--------------------------");
                from.SendMessage(63, "Start Event : [StartSurvival ");
            }

            base.OnDoubleClick(from);
        }

        public TopPlayersBook(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(TopPlayers.Count);

            if (TopPlayers.Count > 0)
            {
                foreach (var entity in TopPlayers)
                {
                    writer.Write(entity.Name);
                    writer.Write(entity.Kills);
                    writer.Write(entity.Deaths);
                    writer.Write(entity.Points);
                }
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int count = reader.ReadInt();

            if (count > 0)
            {
                List<SurvivalLeaderEntity> temp = new List<SurvivalLeaderEntity>();

                for (int i = 0; i < count; i++)
                {
                    var entity = new SurvivalLeaderEntity(reader.ReadString(), reader.ReadInt(), reader.ReadInt(), reader.ReadInt());

                    temp.Add(entity);
                }

                if (TopPlayers == null)
                {
                    TopPlayers = new List<SurvivalLeaderEntity>();
                }

                if (TopPlayers.Count == 0)
                {
                    TopPlayers.AddRange(temp);
                }
            }
        }
    }

    internal class SurvivalLeaderEntity
    {
        public string Name { get; private set; }

        public int Kills { get; private set; }

        public int Deaths { get; private set; }

        public int Points { get; private set; }

        public SurvivalLeaderEntity(string name, int kills, int deaths, int points)
        {
            Name = name;

            Kills = kills;

            Deaths = deaths;

            Points = points;
        }
    }
}

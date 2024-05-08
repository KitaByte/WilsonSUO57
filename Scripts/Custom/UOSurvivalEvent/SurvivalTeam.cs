using Server.Custom.UOSurvivalEvent.Islands;
using Server.Custom.UOSurvivalEvent.Items;
using System.Collections.Generic;
using Server.Multis;
using Server.Items;
using System;

namespace Server.Custom.UOSurvivalEvent
{
    public class SurvivalTeam
    {
        public List<SurvivalPlayer> TeamPlayers { get; private set; }

        public BaseIsland TeamIsland { get; private set; }

        public List<Static> IslandStatics { get; private set; }

        public Static TeamTotem { get; private set; }

        internal int TotemTime { get; set; } = 0;

        public Point3D HomeLocation { get; private set; }

        public bool IsWinner { get; set; } = false;

        public SurvivalTeam()
        {
            TeamPlayers = new List<SurvivalPlayer>();

            IslandStatics = new List<Static>();
        }

        public void AddPlayer(SurvivalPlayer player)
        {
            if (!TeamPlayers.Contains(player))
            {
                TeamPlayers.Add(player);

                player.S_Player.AddToBackpack(new SmallBoatDeed());

                player.S_Player.SendMessage(53, "A small boat deed has been added to your pack!");
            }
        }

        internal SurvivalPlayer GetPlayerInfo(Mobile player)
        {
            return TeamPlayers.Find(sp => sp.S_Player == player);
        }

        internal bool HasPlayer(Mobile player)
        {
            return GetPlayerInfo(player) != null;
        }

        internal void RemovePlayer(Mobile player)
        {
            var info = GetPlayerInfo(player);

            if (info != null)
            {
                TeamPlayers.Remove(info);
            }
        }

        public void AddIslandLocation(Rectangle2D bounds)
        {
            if (TeamIsland == null)
            {
                TeamIsland = new BaseIsland(bounds);
            }
        }

        public void SpawnIsland(Map map, IslandType biome)
        {
            TeamIsland?.SpawnIsland(this, map, biome);
        }

        public void DespawnIsland()
        {
            if (IslandStatics.Count > 0)
            {
                for (int i = 0; i < IslandStatics.Count; i++)
                {
                    IslandStatics[i].Delete();
                }

                IslandStatics.Clear();
            }

            if (BaseIsland.HasWeather())
            {
                BaseIsland.EndWeather();
            }
        }

        public void AddTotem(Static totem)
        {
            TeamTotem = totem;
        }

        public bool IslandHasPlayer(Mobile player)
        {
            if (TeamIsland.IslandBounds.Contains(player))
            {
                return true;
            }

            return false;
        }

        internal void AddHomeLocation(Point3D location)
        {
            HomeLocation = location;
        }

        internal void MoveToIsland()
        {
            if (TeamPlayers.Count > 0)
            {
                var count = TeamPlayers.Count;

                var dropOuts = new List<SurvivalPlayer>();

                for (int i = 0; i < count; i++)
                {
                    if (TeamPlayers[i] is SurvivalPlayer sp)
                    {
                        if (sp.S_Player.HasGump(typeof(SurvivalGump)))
                        {
                            sp.S_Player.CloseGump(typeof(SurvivalGump));
                        }

                        if (TeamPlayers.Count > i && sp.S_Player.Map != Map.Internal && sp.S_Player.Alive)
                        {
                            sp.MarkHomeLocation();

                            sp.S_Player.MoveToWorld(HomeLocation, TeamTotem.Map);
                        }

                        if (!TeamIsland.IslandBounds.Contains(sp.S_Player))
                        {
                            dropOuts.Add(TeamPlayers[i]);
                        }
                    }
                }

                foreach (var oldPlayer in dropOuts)
                {
                    if (TeamPlayers.Contains(oldPlayer))
                    {
                        TeamPlayers.Remove(oldPlayer);
                    }
                }
            }
        }

        internal void LateMove(SurvivalPlayer sp)
        {
            sp.MarkHomeLocation();

            sp.S_Player.MoveToWorld(HomeLocation, TeamTotem.Map);
        }

        internal void MoveToHome()
        {
            if (TeamPlayers.Count > 0)
            {
                for (int i = 0; i < TeamPlayers.Count; i++)
                {
                    var info = TeamPlayers[i];

                    SurvivalCore.RemoveBoat(info.S_Player);

                    info.S_Player.MoveToWorld(info.HomeLocation.oldLocation, info.HomeLocation.map);

                    if (!info.S_Player.Alive)
                    {
                        info.S_Player.Resurrect();
                    }

                    info.S_Player.Heal(100);

                    info.S_Player.Frozen = false;

                    RewardPlayer(info);
                }
            }
        }

        private void RewardPlayer(SurvivalPlayer info)
        {
            var total = info.Points;

            total += info.Kills * 2;

            total -= info.Deaths;

            if (IsWinner)
            {
                AddWinReward(info, total);
            }
            else
            {
                AddLoseReward(info, total);
            }

            TopPlayersBook.ValidateTopPlayer(info);
        }

        private static void AddWinReward(SurvivalPlayer info, int total)
        {
            if (SurvivalSettings.IsGoldPrize)
            {
                int amount = total * SurvivalSettings.GoldAmount;

                info.S_Player.AddToBackpack(new Gold(amount));

                info.S_Player.SendMessage(53, $"Thanks for playing, You recieved {amount} Gold!");
            }

            if (SurvivalSettings.IsItemPrize)
            {
                var prize = Activator.CreateInstance(SurvivalSettings.ItemPrize);

                if (prize is Item p)
                {
                    info.S_Player.AddToBackpack(p);

                    info.S_Player.SendMessage(53, $"Thanks for playing, Your reward [{p.Name}]");
                }
            }
        }

        private static void AddLoseReward(SurvivalPlayer info, int total)
        {
            AddWinReward(info, total);
        }

        internal void FreezeAllPlayers()
        {
            for (int i = 0; i < TeamPlayers.Count; i++)
            {
                TeamPlayers[i].S_Player.Frozen = true;
            }
        }

        internal void UnFreezeAllPlayers()
        {
            for (int i = 0; i < TeamPlayers.Count; i++)
            {
                TeamPlayers[i].S_Player.Frozen = false;
            }
        }

        internal void PlayEndGameSound()
        {
            for (int i = 0; i < TeamPlayers.Count; i++)
            {
                TeamPlayers[i].S_Player.PlaySound(0x543);
            }
        }
    }
}

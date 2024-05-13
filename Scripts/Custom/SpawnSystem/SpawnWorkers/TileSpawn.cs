using System.Collections.Generic;
using System.Linq;

namespace Server.Custom.SpawnSystem
{
    internal static class TileSpawn
    {
        internal static string TryTileSpawn(Map map, Point3D location)
        {
            string tileName = SpawnSysUtility.TryGetWetName(map, location);

            if (string.IsNullOrEmpty(tileName) || tileName == "NoName")
            {
                tileName = SpawnSysTileInfo.GetTileName(map.Tiles.GetLandTile(location.X, location.Y).ID);
            }

            if (SpawnSysDataBase.WorldSpawns.Count > 0)
            {
                try
                {
                    var entity = SpawnSysDataBase.WorldSpawns.Find(e => e.MapHandle == map);

                    if (entity == null)
                    {
                        return string.Empty;
                    }

                    if (tileName == "rock")
                    {
                        tileName = Utility.RandomList("cave", "cave floor");
                    }

                    var spawnList = entity.GetSpawnList(tileName, SpawnSysDataBase.GetFreq(Utility.RandomDouble()));

                    if (spawnList.Count > 0)
                    {
                        List<TileEntity> refinedList = new List<TileEntity>();

                        if (Utility.RandomDouble() < (SpawnSysDataBase.CreatureChance * SpawnSysFactory.NightMod))
                        {
                            refinedList.AddRange(spawnList.Where(s => s.IsMob).ToList());
                        }
                        else
                        {
                            refinedList.AddRange(spawnList.Where(s => !s.IsMob).ToList());
                        }

                        if (refinedList.Count > 0)
                        {
                            return refinedList[Utility.Random(refinedList.Count - 1)].Name;
                        }
                    }
                }
                catch
                {
                    SpawnSysUtility.SendConsoleMsg(System.ConsoleColor.DarkRed, "Factory => Tile Error!");
                }
            }

            return string.Empty;
        }

        internal static string TryTileSpawn(Map map, string tileName, Frequency freq)
        {
            if (SpawnSysDataBase.WorldSpawns.Count > 0)
            {
                try
                {
                    var entity = SpawnSysDataBase.WorldSpawns.Find(e => e.MapHandle == map);

                    if (entity == null)
                    {
                        return string.Empty;
                    }

                    var spawnList = entity.GetSpawnList(tileName, freq);

                    if (spawnList.Count > 0)
                    {
                        List<TileEntity> refinedList = new List<TileEntity>();

                        if (Utility.RandomDouble() < (SpawnSysDataBase.CreatureChance * SpawnSysFactory.NightMod))
                        {
                            refinedList.AddRange(spawnList.Where(s => s.IsMob).ToList());
                        }
                        else
                        {
                            refinedList.AddRange(spawnList.Where(s => !s.IsMob).ToList());
                        }

                        if (refinedList.Count > 0)
                        {
                            return refinedList[Utility.Random(refinedList.Count - 1)].Name;
                        }
                    }
                }
                catch
                {
                    SpawnSysUtility.SendConsoleMsg(System.ConsoleColor.DarkRed, "UORspawn: Factory => Tile 2 Error!");
                }
            }

            return string.Empty;
        }
    }
}

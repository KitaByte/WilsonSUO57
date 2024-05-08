using Server.Targeting;
using System.Linq;

namespace Server.Custom.SpawnSystem
{
    internal static class StaticSpawn
    {
        internal static string TryStaticSpawn(Map map, Point3D location)
        {
            var objectPool = map.GetObjectsInRange(location, SpawnSysSettings.MIN_RANGE);

            try
            {
                var objects = objectPool.ToList();

                if (objects.Count > 0 && Utility.RandomDouble() < (SpawnSysDataBase.StaticChance * SpawnSysFactory.NightMod))
                {
                    foreach (var obj in objects)
                    {
                        if (obj is StaticTarget st && SpawnSysDataBase.StaticSpawns.Count > 0)
                        {
                            foreach (var entity in SpawnSysDataBase.StaticSpawns)
                            {
                                if (entity.Name == st.Name)
                                {
                                    objectPool.Free();

                                    return entity.GetRandomSpawn(SpawnSysDataBase.GetFreq(Utility.RandomDouble()));
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                SpawnSysUtility.SendConsoleMsg(System.ConsoleColor.DarkRed, "UORspawn: Factory => Statics Error!");
            }

            objectPool.Free();

            return string.Empty;
        }
    }
}

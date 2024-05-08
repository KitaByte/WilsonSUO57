using Server.Items;

namespace Server.Custom.StaffRing
{
    internal static class StaffRingCore
    {

        internal static void SpawnGameBoundry(StaffRingEntity entity)
        {
            var height = entity.RingMap.GetAverageZ(entity.RingRect2D.Start.X, entity.RingRect2D.Start.Y);

            for (int x = entity.RingRect2D.Start.X; x <= entity.RingRect2D.End.X; x++)
            {
                AddBoundry(new Point3D(x, entity.RingRect2D.Start.Y, height), entity);

                AddBoundry(new Point3D(x, entity.RingRect2D.End.Y, height), entity);
            }

            for (int y = entity.RingRect2D.Start.Y; y <= entity.RingRect2D.End.Y; y++)
            {
                AddBoundry(new Point3D(entity.RingRect2D.Start.X, y, height), entity);

                AddBoundry(new Point3D(entity.RingRect2D.End.X, y, height), entity);
            }
        }

        private static void AddBoundry(Point3D location, StaffRingEntity entity)
        {
            Static boundry = new Static(Utility.RandomList(0x45B4, 0x45B5));

            boundry.MoveToWorld(location, entity.RingMap);

            entity.AddBlocker(boundry);
        }
    }
}

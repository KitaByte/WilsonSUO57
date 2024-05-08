using System.Collections.Generic;

namespace Server.Custom.StaffRing
{
    public class StaffRingEntity
    {
        public Rectangle2D RingRect2D { get; private set; }

        public Map RingMap { get; private set; }

        public List<Item> Blockers { get; private set; }

        public StaffRingEntity()
        {
            Blockers = new List<Item>();
        }

        public void AddBlocker(Item item)
        {
            Blockers.Add(item);
        }
    }
}

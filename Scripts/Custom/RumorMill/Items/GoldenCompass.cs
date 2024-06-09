using System;

namespace Server.Custom.RumorMill.Items
{
    public class GoldenCompass : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public IRumor CurrentRumor { get; private set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int lastCubit { get; private set; }

        [Constructable]
        public GoldenCompass() : base(0x1058)
        {
            Name = "Golden Compass";

            Hue = 2734;

            Weight = 0.1;
        }

        public void SetRumor(IRumor r, Mobile to)
        {
            CurrentRumor = r;

            lastCubit = (int)to.GetDistanceToSqrt(CurrentRumor.RumorLocation);

            Timer.DelayCall(TimeSpan.FromMinutes(30), () =>
            {
                CurrentRumor = null;

                lastCubit = 0;

                to.SendMessage(53, "The compass has lost connection with target!");
            });
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (CurrentRumor != null)
            {
                if (from.Map != CurrentRumor.RumorMap)
                {
                    from.SendMessage(53, $"Target is in the lands of {CurrentRumor.RumorMap.Name}!");

                    return;
                }

                int currentCubit = (int)from.GetDistanceToSqrt(CurrentRumor.RumorLocation);

                if (currentCubit > lastCubit)
                {
                    from.SendMessage(5, "Getting Colder!");
                }
                else
                {
                    from.SendMessage(40, "Getting Warmer!");
                }

                lastCubit = currentCubit;
            }
            else
            {
                from.SendMessage(53, "No target set, find a vendor with a rumor!");
            }
        }

        public GoldenCompass(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

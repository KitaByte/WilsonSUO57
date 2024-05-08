using Server.Targeting;

namespace Server.Engines.Sickness.Items
{
    public class MedicCloth : Item, IArtifact
	{
		public override bool IsArtifact => true;

		public int ArtifactRarity => 10;

		public override double DefaultWeight => 1.0;

        [Constructable]
        public MedicCloth() : base(0x175D)
        {
            Name = "a medical cloth";

            Hue = 1153;

			LootType = LootType.Blessed;
		}

        public MedicCloth(Serial serial) : base(serial)
        {
		}

		public override bool ForceShowProperties => true;

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1061078, ArtifactRarity.ToString()); // artifact rarity ~1_val~
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (IsChildOf(from.Backpack))
			{
				from.BeginTarget(-1, false, TargetFlags.None, OnTarget);

				from.SendMessage("Target a thermometer to clean!");
			}
			else
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
		}

		public void OnTarget(Mobile from, object obj)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else if (obj is Item && ((Item)obj).RootParent != from)
			{
				from.SendLocalizedMessage(1005425); // You may only wipe down items you are holding or carrying.
			}
			else if (obj is Thermometer thermo)
			{
				if (thermo.HasGerms)
				{
					thermo.HasGerms = false;

					from.SendMessage("The thermometer, has been cleaned!");
				}
				else
				{
					from.SendMessage("The thermometer, is already clean!");
				}
			}
			else
			{
				from.SendLocalizedMessage(1005426); // The cloth will not work on that.
			}
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

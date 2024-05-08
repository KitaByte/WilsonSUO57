using Server.Engines.Sickness.Commands;
using Server.Engines.Sickness.IllnessTypes;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Sickness.Items
{

	[Flipable(0x0F31, 0x0F32)]
	public class Thermometer : Item, IArtifact
	{
		public override bool IsArtifact => true;

		public int ArtifactRarity => 8;

		[CommandProperty(AccessLevel.GameMaster)]
		public SicknessType GermType { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool HasGerms { get; set; } = false;


		[Constructable]
		public Thermometer() : base(0x0F31 + Utility.Random(2))
		{
			Name = "Medical Thermometer";

			Hue = 1171;

			LootType = LootType.Cursed;
		}

		public Thermometer(Serial serial) : base(serial)
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
				from.Target = new GetHealthTarget(); //Located in => Commands/GetIllnessInfo.cs

				if (HasGerms && from is PlayerMobile player)
				{
					var germ = SicknessUtility.GetSickness((int)GermType);

					IllnessHandler.GetPlayerIllness(player.Name).TryInfection(player, germ);
				}
			}
			else
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}

			base.OnDoubleClick(from);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}

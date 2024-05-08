using Server.Engines.Sickness.IllnessTypes;
using Server.Mobiles;

namespace Server.Engines.Sickness.Items
{
	public class BaseCureItem : Item
	{
		// Remedy Type
		private BaseRemedy RemType { get; set; }

		// Constructor takes ItemID and Remedy Type
		public BaseCureItem(int itemID, BaseRemedy remType) : base(itemID)
		{
			Name = remType.RemName;
			RemType = remType;
		}

		public BaseCureItem(Serial serial) : base(serial)
		{
		}

		// Handles on double click, tries to add the cure,
		// will provide feedback based on success
		public override void OnDoubleClick(Mobile from)
		{
			if (from is PlayerMobile player)
			{
				if (RootParent == player)
				{
					var illness = IllnessHandler.GetPlayerIllness(player.Name);

					var IsWorking = illness.TryCure(player, RemType, out var wasCured);

					if (wasCured)
					{
						player.SendMessage("Success, you were completely cured!");

						IllnessEmote.RunAnimation(ref player, false);
					}
					else if (IsWorking)
					{
						player.SendMessage("Some Success, you were somewhat cured!");

						IllnessEmote.RunAnimation(ref player, false);
					}
					else
					{
						player.SendMessage("No Success, you were not sick!");

						IllnessEmote.RunAnimation(ref player, true);
					}

					RunSideEffect(ref player);

					Delete();
				}
				else
				{
					player.SendMessage("This must be in your backpack!");
				}
			}

			base.OnDoubleClick(from);
		}

		// Will damage/heal player, damage/heal depends on remedy
		// RemModMinMax, Potency and the players immmunity level.
		private void RunSideEffect(ref PlayerMobile player)
		{
			if (RemType.RemSideEffect)
			{
				var immunityMod = IllnessImmunity.PlayerImmunityLevel(player);

				if (RemType.IsPositiveSE)
				{
					player.Heal(RemType.RemModMinMax * RemType.RemPotency / immunityMod);

					EffectAnimInfo(ref player, true, "healed");
				}
				else
				{
					player.Damage(RemType.RemModMinMax * RemType.RemPotency / immunityMod);

					EffectAnimInfo(ref player, false, "damaged");
				}
			}
		}

		// Runs animation and sends feedback to player.
		private void EffectAnimInfo(ref PlayerMobile player, bool isPositive, string msg)
		{
			IllnessEmote.RunAnimation(ref player, isPositive);

			player.SendMessage($"The cure had a bite to it, you were {msg}!");
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

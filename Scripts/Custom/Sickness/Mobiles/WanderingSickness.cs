using Server.Mobiles;
using Server.Engines.Sickness.IllnessTypes;

namespace Server.Engines.Sickness.Mobiles
{
	//A class that represents an in-game sickness that can be contracted by players.
	//This sickness can be contacted through speech or by walking over it.

	[CorpseName("dead infection")]
	public class WanderingSickness : BaseCreature
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public BaseSickness Sickness { get; set; }

		[Constructable]
		public WanderingSickness() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Infection";

			Sickness = SicknessUtility.GetRandomSickness();

			Body = 6;

			Hidden = true;

			SetStr(10);
			SetDex(10);
			SetInt(10);

			SetDamage(0);

			SetDamageType(ResistanceType.Physical, 100);

			Fame = 0;
			Karma = 0;
		}

		public WanderingSickness(Serial serial) : base(serial)
		{
		}

		public override void OnAfterMove(Point3D oldLocation)
		{
			if (!Hidden)
			{
				Hidden = true;
			}

			base.OnAfterMove(oldLocation);
		}

		private int CheckMobsCounter = 0;

		//A method that runs every server tick.
		//This method checks if there are any players within a certain range of
		//the WanderingSickness instance. If there are no players within range,
		//the instance is deleted.
		public override void OnThink()
		{
			if (CheckMobsCounter > 5000)
			{
				var mobs = GetMobilesInRange(40);

				var hasPlayer = false;

				foreach (var mob in mobs)
				{
					if (mob is PlayerMobile pm)
					{
						if (IllnessHandler.InDebug || pm.AccessLevel < AccessLevel.GameMaster)
							hasPlayer = true;
					}
				}

				if (!hasPlayer)
					Delete();

				CheckMobsCounter = 0;
			}
			else
			{
				CheckMobsCounter++;
			}

			base.OnThink();	
		}

		//A method that is called when a player speaks near the WanderingSickness instance.
		//If the sickness is not a contact sickness, the InfectPlayer() method is called and
		//the WanderingSickness instance is deleted.
		public override void OnSpeech(SpeechEventArgs e)
		{
			if (Sickness != null && !Sickness.IsContact)
			{
				if (e.Mobile is PlayerMobile player)
				{
					InfectPlayer(player);

					Delete();
				}
			}
			else
			{
				base.OnSpeech(e);
			}
		}

		//A method that is called when a player walks over the WanderingSickness instance.
		//If the sickness is a contact sickness, the InfectPlayer() method is called and
		//the WanderingSickness instance is deleted.
		public override bool OnMoveOver(Mobile m)
		{
			if (Sickness != null && Sickness.IsContact)
			{
				if (m is PlayerMobile player)
				{
					InfectPlayer(player);

					Delete();
				}
			}

			return base.OnMoveOver(m);
		}

		//A method that is called when a player is infected with a sickness.
		//If the player is not immune to the sickness, the player's illness data is updated
		//with the new sickness. The player is then played an animation and sound effect.
		private void InfectPlayer(PlayerMobile player)
		{
			IllnessHandler.UpdatePlayerIllness(player, Sickness);

			IllnessEmote.RunAnimation(ref player, true);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write((int)Sickness.Sickness);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();

			Sickness = SicknessUtility.GetSickness(reader.ReadInt());
		}
	}
}

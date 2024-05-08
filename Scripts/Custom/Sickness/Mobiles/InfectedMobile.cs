using Server.Engines.Sickness.IllnessTypes;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Sickness.Mobiles
{
	public class InfectedMobile : BaseCreature
	{
		public InfectedMobile(Serial serial) : base(serial)
		{
		}

		public InfectedMobile(AIType ai, FightMode mode, int iRangePerception, int iRangeFight) : base(ai, mode, iRangePerception, iRangeFight)
		{
		}

		public InfectedMobile(AIType ai, FightMode mode, int iRangePerception, int iRangeFight, double dActiveSpeed, double dPassiveSpeed) : base(ai, mode, iRangePerception, iRangeFight, dActiveSpeed, dPassiveSpeed)
		{
		}

		// Release a wandering sickness

		public virtual bool IsActiveOnHits { get; set; } = false;

		public virtual int OnHitsChance { get; set; } = 5;

		public override void OnHitsChange(int oldValue)
		{
			if (IsActiveOnHits)
			{
				if (oldValue > Hits && Utility.Random(100) < OnHitsChance)
				{
					SicknessUtility.AddSicknessToWorld(this, SicknessUtility.GetRandomSickness());
				}
			}

			base.OnHitsChange(oldValue);
		}

		public virtual bool IsActiveOnDamage { get; set; } = false;

		public virtual int OnDamageChance { get; set; } = 5;

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if (IsActiveOnDamage)
			{
				if (amount < Hits && Utility.Random(100) < OnDamageChance)
				{
					SicknessUtility.AddSicknessToWorld(this, SicknessUtility.GetRandomSickness());
				}
			}

			base.OnDamage(amount, from, willKill);
		}

		public virtual bool IsActiveOnDeath { get; set; } = false;

		public virtual int OnDeathChance { get; set; } = 5;

		public override void OnDeath(Container c)
		{
			if (IsActiveOnDeath)
			{
				if (Utility.Random(100) < OnDeathChance)
				{
					SicknessUtility.AddSicknessToWorld(this, SicknessUtility.GetRandomSickness());
				}
			}

			base.OnDeath(c);
		}

		// Try to infect player directly

		public virtual bool IsActiveOnGaveMelee { get; set; } = false;

		public virtual int OnGaveMeleeChance { get; set; } = 5;

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			if (IsActiveOnGaveMelee)
			{
				if (defender is PlayerMobile player && Utility.Random(100) < OnGaveMeleeChance)
				{
					IllnessHandler.GetPlayerIllness(player.Name).TryInfection(player, SicknessUtility.GetRandomSickness());
				}
			}

			base.OnGaveMeleeAttack(defender);
		}

		public virtual bool IsActiveOnGotMelee { get; set; } = false;

		public virtual int OnGotMeleeChance { get; set; } = 5;

		public override void OnGotMeleeAttack(Mobile defender)
		{
			if (IsActiveOnGotMelee)
			{
				if (defender is PlayerMobile player && Utility.Random(100) < OnGotMeleeChance)
				{
					IllnessHandler.GetPlayerIllness(player.Name).TryInfection(player, SicknessUtility.GetRandomSickness());
				}
			}

			base.OnGaveMeleeAttack(defender);
		}

		public virtual bool IsActiveOnCarve { get; set; } = false;

		public virtual int OnCarveChance { get; set; } = 5;

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (IsActiveOnCarve)
			{
				if (from is PlayerMobile player && Utility.Random(100) < OnCarveChance)
				{
					IllnessHandler.GetPlayerIllness(player.Name).TryInfection(player, SicknessUtility.GetRandomSickness());
				}
			}

			base.OnCarve(from, corpse, with);
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

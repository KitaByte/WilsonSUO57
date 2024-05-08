using System.Collections.Generic;

using Server.Items;

namespace Server.Mobiles
{
	public sealed class WhiteRabbit : Rabbit
	{
		public const int RangeOfHearing = 5; // vendor range for parsing speech
		public const int RangeOfAttraction = 25; // golden carrot range of attraction

		public static WhiteRabbit Rabbit { get; private set; }

		public static void Initialize()
		{
			EventSink.ServerStarted += HandleServerStarted;
			EventSink.Speech += HandleSpeech;
			EventSink.AggressiveAction += HandleAggressiveAction;
			EventSink.CreatureDeath += HandleCreatureDeath;
		}

		private static void HandleServerStarted()
		{
			if (Rabbit?.Deleted != false)
			{
				SpawnRabbit();
			}
		}

		private static void HandleSpeech(SpeechEventArgs e)
		{
			if (e.Mobile is PlayerMobile player && Insensitive.Contains(e.Speech, "white rabbit"))
			{
				var mobs = player.GetMobilesInRange(RangeOfHearing);

				foreach (var mob in mobs)
				{
					if (mob is BaseVendor vendor && !vendor.Hidden)
					{
						var info = GetRabbitInfo(vendor);

						if (!string.IsNullOrWhiteSpace(info))
						{
							vendor.SayTo(player, info);
						}
					}
				}

				mobs.Free();
			}
		}

		private static void HandleAggressiveAction(AggressiveActionEventArgs e)
		{
			var player = e.Aggressor as PlayerMobile ?? e.Aggressed as PlayerMobile;
			var creature = e.Aggressed as BaseCreature ?? e.Aggressor as BaseCreature;

			if (player == null || creature == null)
			{
				return;
			}

			if (HasGoldenCarrot(player))
			{
				var attracted = false;

				var mobs = player.GetMobilesInRange(RangeOfAttraction);

				foreach (var mob in mobs)
				{
					if (!creature.Deleted && mob is Rabbit rabbit && rabbit != Rabbit && rabbit.Combatant == null && !rabbit.Hidden)
					{
						rabbit.Attack(creature);

						if (rabbit.Combatant == creature)
						{
							attracted = true;
						}
					}
				}

				mobs.Free();

				if (attracted)
				{
					player.SendMessage(52, "The golden carrot is attracting local rabbits to aid you in battle!");
				}
			}
		}

		private static void HandleCreatureDeath(CreatureDeathEventArgs e)
		{
			if (e.Creature == Rabbit)
			{
				if (e.Killer is PlayerMobile player)
				{
					AddRabbitLoot(e.ForcedLoot, player);
				}

				Rabbit = null;

				SpawnRabbit();
			}
		}

		private static void SpawnRabbit()
		{
			if (GetSpawnLoc(out var loc, out var map))
			{
				Rabbit?.Delete();

				Rabbit = new WhiteRabbit();

				Rabbit.MoveToWorld(loc, map);
			}
		}

		private static bool GetSpawnLoc(out Point3D loc, out Map map)
		{
			loc = Point3D.Zero;
			map = Utility.RandomList(Map.Felucca, Map.Trammel, Map.Ilshenar, Map.Malas, Map.Tokuno, Map.TerMur);

			if (map == null || map == Map.Internal)
			{
				return false;
			}

			Rectangle2D bounds;

			if (map.MapID == 0 || map.MapID == 1)
			{
				bounds = new Rectangle2D(16, 16, 5120 - 32, 4096 - 32);
			}
			else if (map.MapID == 2)
			{
				bounds = new Rectangle2D(16, 16, 2304 - 32, 1600 - 32);
			}
			else if (map.MapID == 4)
			{
				bounds = new Rectangle2D(16, 16, 1448 - 32, 1448 - 32);
			}
			else
			{
				bounds = new Rectangle2D(0, 0, map.Width, map.Height);
			}

			do
			{
				loc = map.GetRandomSpawnPoint(bounds);
			}
			while (Mobiles.Spawner.IsValidWater(map, loc.X, loc.Y, loc.Z) || !map.CanSpawnMobile(loc));

			return true;
		}

		private static string GetDirection(Direction direction)
		{
			switch (direction & Direction.Mask)
			{
				default:
				case Direction.North: return "North";
				case Direction.Right: return "North-East";
				case Direction.East: return "East";
				case Direction.Down: return "South-East";
				case Direction.South: return "South";
				case Direction.Left: return "South-West";
				case Direction.West: return "West";
				case Direction.Up: return "North-West";
			}
		}

		private static string GetRabbitInfo(BaseVendor vendor)
		{
			if (Rabbit?.Deleted != false)
			{
				SpawnRabbit();
			}

			if (Rabbit?.Map == vendor.Map)
			{
				if (vendor.GetDistanceToSqrt(Rabbit) < 1000)
				{
					vendor.Emote("*points*");

					vendor.Direction = vendor.GetDirectionTo(Rabbit.Location);

					vendor.Animate(AnimationType.Eat, 0);

					switch (Utility.Random(5))
					{
						case 0: return $"I have seen a white rabbit '{GetDirection(vendor.Direction)}' from here!";
						case 1: return "I will get that pesky rabbit!";
						case 2: return $"I saw a white rabbit today in my travels '{GetDirection(vendor.Direction)}' from here!";
						case 3: return "I saw one but it ran away!";
						case 4: return $"I have heard of a white rabbit '{GetDirection(vendor.Direction)}' from here!";
						default: return $"I saw a white rabbit '{GetDirection(vendor.Direction)}' from here!";
					}
				}
				else
				{
					switch (Utility.Random(5))
					{
						case 0: return "I have heard of a white rabbit!";
						case 1: return "What color of white rabbit?";
						case 2: return "I see white rabbits everywhere!";
						case 3: return "Sorry, I can't help you!";
						case 4: return "I have heard of a white rabbit in my travels!";
						default: return "I have seen a white rabbit, in another region!";
					}
				}
			}
			else
			{
				switch (Utility.Random(5))
				{
					case 0: return "I have never heard of a white rabbit!";
					case 1: return "What is a white rabbit?";
					case 2: return "I see rabbits everywhere!";
					case 3: return "Sorry, I'm colored blind!";
					case 4: return "I haven't heard of a white rabbit in my travels!";
					default: return "I have never seen a white rabbit!";
				}
			}
		}

		private static void AddRabbitLoot(List<Item> loot, PlayerMobile killer)
		{
			var hasCarrot = HasGoldenCarrot(killer);

			var goldMin = 100;
			var goldMax = 1000;

			if (hasCarrot)
			{
				goldMin *= 10;
				goldMax *= 10;
			}

			loot.Add(new Gold(goldMin, goldMax));

			if (!hasCarrot && Utility.RandomDouble() < 0.02)
			{
				loot.Add(new GoldenCarrot());
			}
		}

		private static bool HasGoldenCarrot(PlayerMobile player)
		{
			return player?.Backpack?.FindItemByType<GoldenCarrot>() != null;
		}

		[Constructable]
		public WhiteRabbit()
		{
			AI = AIType.AI_Mage;
			FightMode = FightMode.Closest;

			Name = "White Rabbit";
			Hue = 2499;

			Tamable = false;

			VirtualArmor = 15;

			ForceActiveSpeed = 0.2;
			ForcePassiveSpeed = 0.4;

			Fame = 15000;
			Karma = -15000;

			SetStr(125);
			SetInt(125);
			SetDex(125);

			SetHits(5000);
			SetStam(500);
			SetMana(500);

			SetDamage(5, 15);
			SetDamageType(ResistanceType.Physical, 80);

			SetSkill(SkillName.Magery, 60.0);
			SetSkill(SkillName.EvalInt, 60.0);
			SetSkill(SkillName.Meditation, 60.0);
			SetSkill(SkillName.MagicResist, 80.0);

			SetSkill(SkillName.Tactics, 50.0);
			SetSkill(SkillName.Wrestling, 50.0);

			SetResistance(ResistanceType.Physical, 60);
			SetResistance(ResistanceType.Fire, 60);
			SetResistance(ResistanceType.Cold, 60);
			SetResistance(ResistanceType.Poison, 60);
			SetResistance(ResistanceType.Energy, 60);

			SetSpecialAbility(SpecialAbility.Rage);

			PackItem(new BagOfReagents());
		}

		public WhiteRabbit(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0);

			writer.Write(Rabbit == this);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			_ = reader.ReadEncodedInt();

			if (reader.ReadBool())
			{
				Rabbit?.Delete();
				Rabbit = this;
			}
		}
	}

	public sealed class GoldenCarrot : Carrot
	{
		[Constructable]
		public GoldenCarrot()
		{
			Name = "Golden Carrot";
			Hue = 2734;
		}

		public GoldenCarrot(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			_ = reader.ReadEncodedInt();
		}
	}
}

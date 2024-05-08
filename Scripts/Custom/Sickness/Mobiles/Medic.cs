using Server.Mobiles;
using Server.Items;
using Server.Engines.Sickness.Items;

namespace Server.Engines.Sickness.Mobiles
{
	/*
	Medic.cs
	Includes various properties such as a random skill level, random gender, random name, and random title.
	Has a method called GetSkillName which returns a string based on the skill level.
	It also has a method called OnSpeech which responds to player prompts such as "help" or "reset" and
	performs various actions based on the input.
	The OnDragDrop handles payment and medic services
	*/

	public class Medic : BaseCreature
	{
		// Maximum doctor fee accepted
		private const int MaxDoctorFee = IllnessSettings.MaxDoctorFee;

		// Random skill level
		[CommandProperty(AccessLevel.GameMaster)]
		public int SkillLevel {get; set; } = Utility.Random(100);

		[Constructable]
		public Medic() : base(AIType.AI_Healer, FightMode.None, 10, 1, 0.2, 0.4)
		{
			InitStats(31, 41, 51);

			SpeechHue = Utility.RandomDyedHue();

			Hue = Utility.RandomSkinHue();

			var uniBrightHue = Utility.RandomBrightHue();

			if (Female = Utility.RandomBool())
			{
				Body = 0x191;

				Name = NameList.RandomName("female");

				SetWearable(new Skirt(), uniBrightHue, 1);
			}
			else
			{
				Body = 0x190;

				Name = NameList.RandomName("male");

				SetWearable(new LongPants(), uniBrightHue, 1);
			}

			var addSkillName = GetSkillName(SkillLevel);

			var addMedicType = Utility.RandomList("Chirurgeon", "Apothecary", "Miraculator", "Seraph");

			Title = $"the {addSkillName} {addMedicType}";

			SetWearable(new WizardsHat(), 1175, 1);

			SetWearable(new Shirt(), 1153, 1);

			SetWearable(new FullApron(), uniBrightHue, 1);

			SetWearable(new Sandals(), 1175, 1);

			Utility.AssignRandomHair(this);

			HairHue = uniBrightHue;

			Blessed = true;

			Container pack = new Backpack();

			pack.DropItem(new Gold(250, 300));

			SetWearable(pack);
		}

		// Returns a string based on skill level
		private string GetSkillName(int level)
		{
			if (level < 50)
			{
				return "Adept";
			}

			if (level < 70)
			{
				return "Skilled";
			}

			if (level < 90)
			{
				return "Master";
			}

			return "GrandMaster";
		}

		public Medic(Serial serial) : base(serial)
		{
		}
		// Handles player speech
		// Displays available services and handles special commands for staff members
		public override void OnSpeech(SpeechEventArgs e)
		{
			if (e.Mobile is PlayerMobile player)
			{
				var prompt = e.Speech.ToLower(); 
					
				var illness = IllnessHandler.GetPlayerIllness(player.Name);

				switch (prompt)
				{
					case "help":
						{
							Say($"{player.Name}, what is ailing you?");
							Say("For a small amount of gold,");
							Say("I could study your symptoms!");

							break;
						}

					case "sick":
						{
							if (illness.SicknessList.Count > 0)
							{
								var isSick = false;

								foreach (var infection in illness.SicknessList)
								{
									if (infection.IsDiscovered == false)
									{
										IllnessEmote.RunAnimation(ref player, true);

										isSick = true;
									}
								}
								if (isSick)
								{
									Say("You look sick!");
								}
								else
								{
									Say("You do not look sick!");
								}
							}
							else
							{
								Say("You are not sick!");
							}
							break;
						}
					case "i am interested":
						{
							if (illness.TaskInfo.HasTask)
							{
								var remedy = IllnessUtility.SplitCamelCase(illness.TaskInfo.Remedy.ToString());

								var target = illness.TaskInfo.TargetToKill;

								if (!illness.TaskInfo.TaskStarted)
								{
									Say($"{player.Name}, If you kill a {target}, ");
									Say($"You'll find a {remedy} on your first slain!");
									Say("Good Luck, may you be well soon!");

									illness.TaskInfo.StartTask();
								}
								else
								{
									Say($"You already have a task to find a {remedy} on a {target}!");
								}
							}
							break;
						}
					default:
						{
							Say("Has thou been sick, do you require assitence?");
							Say("You only need to ask for help!");

							break;
						}
				}
			}
			base.OnSpeech(e);
		}

		// Handles player dropping gold to pay for services
		// Attempts to diagnose player's illness and inform them of the results
		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (from is PlayerMobile player)
			{
				if (dropped != null)
				{
					switch (dropped.GetType().Name)
					{
						case "Gold":
							{
								if (dropped.Amount <= MaxDoctorFee)
								{
									var illness = IllnessHandler.GetPlayerIllness(player.Name);

									var caculateChance = ((dropped.Amount / MaxDoctorFee) * 100) + (SkillLevel / 100);

									var success = illness.TryDiscovery(ref player, caculateChance, out var sickness);

									if (success)
									{
										// Chance for medic to level up
										if (SkillLevel < 100 && Utility.Random(0, 1000) < 10)
										{
											Say($"{player.Name}, I've gained some knowledge and skill!");

											SkillLevel++;

											var oldTitle = Title.Split(' ');

											if (oldTitle.Length > 1)
												Title = $"the {GetSkillName(SkillLevel)} {oldTitle[2]}";
										}

										Say($"{player.Name}, I found something, you have {sickness.IllName}!");
										Say($"{sickness.IllName} is a {sickness.Severity}/10 in Severity!");

										var remedy = IllnessUtility.SplitCamelCase(sickness.Remedy.ToString());

										Say($"{remedy} will cure {sickness.IllName}!");
										Say($"I do have a task to find a {remedy}, if thy interested?");

										player.SendMessage("Reply \"I am interested\" to accept task!");

										illness.TaskInfo = new IllnessTask();

										var target = IllnessUtility.SplitCamelCase(IllnessTask.GetTargetToKill());

										illness.TaskInfo.AddTask(target, sickness.Remedy);
									}
									else
									{
										Say($"{player.Name}, I found nothing!");
									}

									dropped.Delete();

									return true;
								}
								else
								{
									Say($"{player.Name}, I can't accept more than {MaxDoctorFee}!");
								}
								break;
							}
						case "Thermometer":
							{
								if (SkillLevel > 89)
								{
									if (dropped is Thermometer thermo)
									{
										if (thermo.HasGerms)
										{
											Say("I'll clean this up, it is filthy!");
										}
										else
										{
											Say("That seems to be clean!");
										}

										thermo.HasGerms = false;
									}
								}
								else
								{
									Say("Only a Grandmaster can clean that!");
								}

								player.PlaceInBackpack(dropped);

								return true;
							}
						default:
							{
								Say("I don't know what to do with that!");

								player.PlaceInBackpack(dropped);

								break;
							}
					}
				}
			}
			return base.OnDragDrop(from, dropped);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(SkillLevel);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();

			SkillLevel = reader.ReadInt();
		}
	}
}

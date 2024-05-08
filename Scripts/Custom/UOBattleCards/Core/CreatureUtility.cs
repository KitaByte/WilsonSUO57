using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using Server.Mobiles;
using Server.Engines.Quests;
using Ultima;
using System.Linq;

namespace Server.Services.UOBattleCards.Core
{
    public class CreatureInfo
    {
        // Creature Props
        public string C_Name { get; set; }
        public int C_Body { get; set; }
        public int C_IdleSound { get; set; }
        public int C_AtkSound { get; set; }
        public int C_BlockSound { get; set; }
        public int C_Hue { get; set; }
        public int C_Damage { get; set; }
        public int C_Hits { get; set; }
        public int C_Mana { get; set; }
        public int C_Stam { get; set; }
        public int C_ItemID { get; set; }
        public int C_Width { get; set; }
        public int C_Height { get; set; }
        public bool C_IsWater { get; set; }
        public int C_Fame { get; set; }
		public int C_Karma { get; set; }
    }

    public static class CreatureUtility
    {
        private static readonly string FilePath = Path.Combine(@"Saves\UOBattleCard", $"Creatures.bin");

        private static int CreatureCount = 0;

		public static bool RebuildCreatureList = Settings.RebuildCreatureCards;

		public static bool IsGoodAllowed = Settings.IsGoodCardsAllowed;

        public static Dictionary<string, CreatureInfo> CreatureDict { get; set; } = new Dictionary<string, CreatureInfo>();

        public static CreatureInfo GetInfo(string name)
        {
            HasCreature(name);

            if (CreatureDict.TryGetValue(name, out var creature))
            {
                return creature;
            }
            else
            {
                return null;
            }
        }

        public static bool HasCreature(string name)
        {
            if (CreatureDict.Count == 0)
            {
                LoadCreatureInfo();
            }

            if (CreatureDict.ContainsKey(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetRandomCreature()
        {
            if (CreatureDict.Count == 0)
            {
                LoadCreatureInfo();
            }

            var rndNumber = Utility.Random(CreatureDict.Count - 1);

            var creatureList = new List<string>();

            foreach (var creature in CreatureDict.Keys)
            {
                creatureList.Add(creature);
            }

            return creatureList[rndNumber];
        }

        private static int SetItemID(int body, string name)
        {
            var id = ShrinkTable.Lookup(body, 8430); // Default Bird

            if (id == 8430 && name.ToLower() != "bird")
            {
                id = GetCreatureArt(body, out var hasFailed); // Convert Anim > Item

                if (hasFailed)
                {
                    CoreUtility.LogMessage(ConsoleColor.DarkYellow, $"UO Battle Cards : [Missing/Add] case {(int)body}: return ###;      // {name}");

                    id = 0;
                }
            }

            return id;
        }

        private static int GetCreatureArt(int body, out bool hasFailed)
        {
            var art = 0;

            switch (body)
            {
                //case <BodyID>: return <ItemID>;

                case 149: art = 16942; break;           // Succubus:
                case 174: art = 16941; break;           // Semidar
                case 256: art = 11650; break;           // Chief Paroxysmus
                case 257: art = 11651; break;           // Dread Horn
                case 258: art = 11652; break;           // Lady Melisande
                case 259: art = 11653; break;           // Grizzle
                case 260: art = 11668; break;           // Corporeal Brume
                case 261: art = 11655; break;           // Effusion
                case 262: art = 11656; break;           // Tormented Minotaur
                case 263: art = 11657; break;           // Minotaur
                case 264: art = 11658; break;           // Travesty
                case 265: art = 11659; break;           // Hydra
                case 266: art = 11660; break;           // Insane Dryad
                case 267: art = 11651; break;           // Troglodyte
                case 270: art = 17054; break;           // Grubber
                case 271: art = 11664; break;           // Satyr
                case 273: art = 11666; break;           // Essence
                case 278: art = 11671; break;           // Squirrel
                case 279: art = 11672; break;           // Ferret
                case 280: art = 11673; break;           // Minotaur Captain / General
                case 281: art = 11674; break;           // Minotaur Scout
                case 287: art = 17053; break;           // Blood Worm
                case 574: art = 14315; break;           // Birling Blades
                case 637: art = 41797; break;           // Spectral Armour
                case 713: art = 17031; break;           // Abyssal Infernal
                case 1026: art = 41152; break;          // Giant Turkey
                case 1071: art = 19462; break;          // Virtuebane
                case 1244: art = 19463; break;          // Charydbis
                case 1246: art = 18067; break;          // Pumpkin Head
                case 1247: art = 18067; break;          // Pumpkin Head
                case 1248: art = 19464; break;          // Clockwork Exodus
                case 1428: art = 40807; break;          // Jack in the Box
                case 1479: art = 41414; break;          // Khal Ankur
                case 1484: art = 41584; break;          // Krampus
                case 1485: art = 41585; break;          // Krampus Minion
                case 1510: art = 41783; break;          // Coconut Crab

                default: art = -1; break;
            }

            if (art > 0)
                hasFailed = false;
            else
                hasFailed = true;

            return art;
        }

        private static bool CheckBase(BaseCreature creature)
		{
			if (creature is BaseVendor || creature is BaseFamiliar)
			{
				return true;
			}

			if (creature is BaseEscort || creature.GetType().Name.StartsWith("Summoned"))
			{
				return true;
			}

			if (creature is BaseHire || creature is BaseHealer)
			{
				return true;
			}

			return false;
		}

        private static bool CheckKarma(int karma)
        {
            if (!IsGoodAllowed)
            {
                if (karma >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static int FameChance(int fame)
        {
            if (fame == 0)
            {
                fame = 1;
            }

            if (fame > 25000)
            {
                fame = 25000;
            }

            var percentage = (double)fame / 25000 * 100;

            var result = (int)Math.Round(percentage / 10);

            if (result == 0)
            {
                result = 1;
            }

            return result;
        }

        private static bool VerifyCreature(BaseCreature creature)
        {
            if (CheckBase(creature))
            {
                return false;
            }

            if (creature.Fame == 0 || CheckKarma(creature.Karma))
            {
                return false;
            }

            if (creature.HitsMax == 0 || creature.DamageMax == 0)
            {
                return false;
            }

            if (creature.ManaMax == 0 || creature.StamMax == 0)
            {
                return false;
            }

            if (!creature.CanBeParagon)
            {
                return false;
            }

            return true;
        }

        private static readonly List<string> RejectedNames = new List<string>()
        {
                "Ranger",
                "Necromancer",
                "NewHavenMerchant",
                "GargishNoble",
                "ClanCA",
                "ClanCT",
                "ClanRC",
                "ClanRS",
                "ClanSH",
                "ClanSS",
                "ClanSSW"
        };

        private static bool ValidName(string name)
        {
            if (RejectedNames.Find(s => s == name) != null)
            {
                return false;
            }

            return true;
        }

        public static string CleanName(string rawInput)
        {
            var input = rawInput.ToLower();

            if (input.StartsWith("a ") || input.StartsWith("an "))
            {
                input = input.Substring(input.IndexOf(' ') + 1);
            }

            var words = input.Split(' ');

            var sb = new StringBuilder();

            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];

                if (word.Length > 0 && !Char.IsUpper(word[0]))
                {
                    sb.Append(Char.ToUpper(word[0]) + word.Substring(1) + " ");
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static int loadCounter = 0;

        public static void LoadCreatureInfo()
        {
            Persistence.Deserialize(FilePath, OnDeserialize);

            if (CreatureCount == 0 || RebuildCreatureList)
            {
                if (CreatureCount > 0)
                {
                    CreatureDict = new Dictionary<string, CreatureInfo>();
                }

                var entireList = Assembly.GetExecutingAssembly().GetExportedTypes();

                var bestiary = entireList.Where(t => t.IsClass && !t.IsAbstract && t.BaseType == typeof(BaseCreature));

                foreach (var type in bestiary)
                {
                    if (type.GetConstructors().Any(c => c.GetParameters().Length == 0))
                    {
                        if (ValidName(type.Name))
                        {
                            BaseCreature creature = null;

                            try
                            {
                                creature = (BaseCreature)Activator.CreateInstance(type);

                                if (VerifyCreature(creature))
                                {
                                    var newName = CleanName(creature.Name);

                                    if (newName.Length < Settings.NameLengthMax && !CreatureDict.ContainsKey(newName))
                                    {
                                        var creatureInfo = new CreatureInfo()
                                        {
                                            C_Name = newName,
                                            C_Body = creature.BodyValue,
                                            C_IdleSound = creature.GetIdleSound() == 0 ? creature.BaseSoundID : creature.GetIdleSound(),
                                            C_AtkSound = creature.GetAttackSound() == 0 ? creature.BaseSoundID : creature.GetAttackSound(),
                                            C_BlockSound = creature.GetHurtSound() == 0 ? creature.BaseSoundID : creature.GetHurtSound(),
                                            C_Hue = creature.Hue,
                                            C_Damage = creature.DamageMax,
                                            C_Hits = creature.HitsMax,
                                            C_Mana = creature.ManaMax,
                                            C_Stam = creature.StamMax,
                                            C_ItemID = SetItemID(creature.BodyValue, newName),
                                            C_IsWater = creature.CanSwim,
                                            C_Fame = creature.Fame,
                                            C_Karma = creature.Karma
                                        };

                                        if (creatureInfo.C_ItemID > 0)
                                        {
                                            var art = Art.GetStatic(creatureInfo.C_ItemID);

                                            creatureInfo.C_Width = art.Width;

                                            creatureInfo.C_Height = art.Height;
                                            CreatureDict.Add(newName, creatureInfo);

                                            CoreUtility.LogMessage(ConsoleColor.Green, $"UO Battle Cards : [Creature Added] => {newName}");
                                        }
                                    }
                                    else
                                    {
                                        if (CreatureDict.ContainsKey(newName))
                                        {
                                            CoreUtility.LogMessage(ConsoleColor.DarkYellow, $"UO Battle Cards : [Creature Duplicate] => {newName}");
                                        }
                                        else
                                        {
                                            CoreUtility.LogMessage(ConsoleColor.DarkYellow, $"UO Battle Cards : [Creature Name Issue] => {newName}");
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                //do nothing
                            }
                            finally
                            {
                                creature?.Delete();
                            }
                        }
                    }
                }

                CreatureCount = CreatureDict.Count;

                if (CreatureCount > 0)
                {
                    CoreUtility.LogMessage(ConsoleColor.DarkCyan, $"UO Battle Cards : [Cards Created] => {CreatureCount}");

                    CoreUtility.LogMessage(ConsoleColor.Yellow, $"UO Battle Cards : [CleanUp ... Save & Restart]");
                }
                else
                {
                    CoreUtility.LogMessage(ConsoleColor.DarkRed, $"UO Battle Cards : [Cards Creation Failed] Restart Server as Administrator");
                }
            }
            else
            {
                if (loadCounter == 0)
                {
                    loadCounter++;

                    CoreUtility.LogMessage(ConsoleColor.DarkCyan, $"UO Battle Cards : [Cards Loaded] => {CreatureCount}");
                }
            }
        }

        private static void OnDeserialize(GenericReader reader)
        {
            CreatureCount = reader.ReadInt();

            if (CreatureDict == null)
            {
                CreatureDict = new Dictionary<string, CreatureInfo>();
            }

            if (CreatureCount > 0 && CreatureDict.Count == 0)
            {
                for (int i = 0; i < CreatureCount; i++)
                {
                    var creature = new CreatureInfo()
                    {
                        C_Name = reader.ReadString(),
                        C_Body = reader.ReadInt(),
                        C_IdleSound = reader.ReadInt(),
                        C_AtkSound = reader.ReadInt(),
                        C_BlockSound = reader.ReadInt(),
                        C_Hue = reader.ReadInt(),
                        C_Damage = reader.ReadInt(),
                        C_Hits = reader.ReadInt(),
                        C_Mana = reader.ReadInt(),
                        C_Stam = reader.ReadInt(),
                        C_ItemID = reader.ReadInt(),
                        C_Width = reader.ReadInt(),
                        C_Height = reader.ReadInt(),
                        C_IsWater = reader.ReadBool(),
                        C_Fame = reader.ReadInt(),
                        C_Karma = reader.ReadInt()
                    };

                    CreatureDict.Add(creature.C_Name, creature);
                }
            }
        }

        public static void SaveCreatureInfo()
        {
            Persistence.Serialize(FilePath, OnSerialize);
        }

        private static void OnSerialize(GenericWriter writer)
        {
            writer.Write(CreatureCount);

            if (CreatureCount > 0 && CreatureDict != null)
            {
                foreach (var creature in CreatureDict.Values)
                {
                    writer.Write(creature.C_Name);
                    writer.Write(creature.C_Body);
                    writer.Write(creature.C_IdleSound);
                    writer.Write(creature.C_AtkSound);
                    writer.Write(creature.C_BlockSound);
                    writer.Write(creature.C_Hue);
                    writer.Write(creature.C_Damage);
                    writer.Write(creature.C_Hits);
                    writer.Write(creature.C_Mana);
                    writer.Write(creature.C_Stam);
                    writer.Write(creature.C_ItemID);
                    writer.Write(creature.C_Width);
                    writer.Write(creature.C_Height);
                    writer.Write(creature.C_IsWater);
                    writer.Write(creature.C_Fame);
                    writer.Write(creature.C_Karma);
                }
            }
        }
    }
}

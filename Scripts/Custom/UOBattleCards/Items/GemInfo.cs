using System;
using Server.Items;

namespace Server.Services.UOBattleCards.Items
{
    public class GemInfo
    {
		public StatCode Stat { get; set; }

		public GemType Gem { get; set; }

		public int Mod { get; set; }

		public string Name => GetName();

        private string GetName()
        {
            return $"{Gem} of {GetGemName(Mod)} {Stat}";
        }

		public int Hue => GetGemHue();

		private string GetGemName(int amount)
		{
			if (amount < 50) { return "Weak"; }

			if (amount < 71) { return "Average"; }

			if (amount < 91) { return "Strong"; }

			if (amount <= 99) { return "Master"; }

			if (amount >= 100) { return "Grandmaster"; }

			return String.Empty;
		}

		private int GetGemHue()
        {
            switch (Gem)
            {
                case GemType.None: return 0;
                case GemType.StarSapphire: return 2617; 
                case GemType.Emerald: return 2541;
                case GemType.Sapphire: return 2580;
                case GemType.Ruby: return 1922;
                case GemType.Citrine: return 2592; 
                case GemType.Amethyst: return 2643;
                case GemType.Tourmaline: return 2527;
                case GemType.Amber: return 2515;
                case GemType.Diamond: return 2500;
            }

            return 0;
        }

        public void GemSerialize(GenericWriter writer)
        {
            writer.Write((int)Stat);

            writer.Write((int)Gem);

            writer.Write(Mod);
        }

        public void GemDeserialize(GenericReader reader)
        {
            Stat = (StatCode)reader.ReadInt();

            Gem = (GemType)reader.ReadInt();

            Mod = reader.ReadInt();
        }
    }
}

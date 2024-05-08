namespace Server.Custom.InfusionSystem
{
    internal static class InfusionInfo
    {
        private const int ColdHue =     2729; // Blue
        private const int FireHue =     2736; // Orange
        private const int WindHue =     2720; // Whitish
        private const int EarthHue =    2724; // Brown
        private const int MagicHue =    2732; // Purple
        private const int PoisonHue =   2758; // Green
        private const int RotHue =      2731; // Redish
        private const int DeathHue =    2734; // Black & Gold

        internal static InfusionType GetInfusion(int hue)
        {
            switch (hue)
            {
                case ColdHue:   return InfusionType.Cold;
                case FireHue:   return InfusionType.Fire;
                case WindHue:   return InfusionType.Wind;
                case EarthHue:  return InfusionType.Earth;
                case MagicHue:  return InfusionType.Magic;
                case PoisonHue: return InfusionType.Poison;
                case RotHue:    return InfusionType.Rot;
                case DeathHue:  return InfusionType.Death;
            }

            return InfusionType.Base;
        }

        internal static (string name, int hue) GetInfo(InfusionType infusion)
        {
            switch (infusion)
            {
                case InfusionType.Cold:     return ("Frost", ColdHue); 
                case InfusionType.Fire:     return ("Flame", FireHue); 
                case InfusionType.Wind:     return ("Storm", WindHue);
                case InfusionType.Earth:    return ("Stone", EarthHue);
                case InfusionType.Magic:    return ("Force", MagicHue);
                case InfusionType.Poison:   return ("Venom", PoisonHue);
                case InfusionType.Rot:      return ("Decay", RotHue);
                case InfusionType.Death:    return ("Death", DeathHue);

                default: return (string.Empty, 0); 
            }
        }

        internal static double GetChance(InfusionType infusion)
        {
            switch (infusion)
            {
                case InfusionType.Cold:     return 0.3;
                case InfusionType.Fire:     return 0.3;
                case InfusionType.Wind:     return 0.3;
                case InfusionType.Earth:    return 0.3;
                case InfusionType.Magic:    return 0.2;
                case InfusionType.Poison:   return 0.1;
                case InfusionType.Rot:      return 0.1;
                case InfusionType.Death:    return 0.1;

                default: return 0.0;
            }
        }

        internal static int GetID()
        {
            return Utility.RandomList(
                0x9CB6, 0x223A, 0x223B, 0x223C, 0x223D, 0x223E,
                0x223F, 0x2240, 0x2241, 0x2242, 0x2243, 0x2244,
                0x2245, 0x2246, 0x2247, 0x2248, 0x2249, 0x5738);
        }
    }
}

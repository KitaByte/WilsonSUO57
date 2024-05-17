using System;
using Server.Mobiles;

namespace Server.Custom.UOStudio
{
    public class StudioActor : BaseCreature
    {
        public override bool IsInvulnerable => true;

        public override bool PlayerRangeSensitive => false;

        public StudioActor(BodyDouble info) : base(AIType.AI_Use_Default, FightMode.None, 0, 0, 0.0, 0.0)
        {
            AccessLevel = AccessLevel.Counselor;

            IgnoreMobiles = true;

            InitStats(31, 41, 51);

            SpeechHue = Utility.RandomDyedHue();

            Female = info.IsFemale;

            Name = info.ActorName;

            Body = info.BodyID;

            Hue = info.SkinHue;

            HairItemID = info.HairStyle;

            HairHue = info.HairColor;

            if (!Female)
            {
                FacialHairItemID = info.FaceHairStyle;

                FacialHairHue = info.FaceHairColor;
            }

            int count = info.Clothing.Count;

            for (int i = 0; i < count; i++)
            {
                string name = info.Clothing[i].ToString();

                Type itemType = Type.GetType(name);

                if (itemType != null)
                {
                    object itemInstance = Activator.CreateInstance(itemType);

                    if (itemInstance is Item item)
                    {
                        item.Hue = (int)info.ClothingHue[i];

                        AddItem(item);
                    }
                }
            }
        }

        public StudioActor(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}

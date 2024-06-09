using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.RumorMill.Mobiles
{
    public class MissingNPC : BaseCreature
    {
        private Gold gold;

        public override bool ClickTitle => false;

        public override bool IsInvulnerable => true;

        public MissingNPC() : base(AIType.AI_Animal, FightMode.None, 0, 0)
        {
            Title = "the lost";

            Female = Utility.RandomBool();

            Hue = Utility.RandomSkinHue();

            if (Female)
            {
                Body = 0x191;

                Name = NameList.RandomName("female");

                AddItem(new FancyDress(Utility.RandomDyedHue()));
            }
            else
            {
                Body = 0x190;

                Name = NameList.RandomName("male");

                AddItem(new LongPants(Utility.RandomNeutralHue()));

                AddItem(new FancyShirt(Utility.RandomDyedHue()));
            }

            AddItem(new Boots(Utility.RandomNeutralHue()));

            Utility.AssignRandomHair(this);

            Container pack = new Backpack();

            if (Utility.RandomDouble() < 0.05)
            {
                gold = new Gold(5000, 50000);
            }
            else
            {
                gold = new Gold(500, 5000);
            }

            gold.Visible = false;

            pack.DropItem(gold);

            pack.Movable = false;

            AddItem(pack);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.Player && InRange(m, 6) && !InRange(oldLocation, 6))
            {
                SayTo(m, true, $"{m.Name}, You found me, Thank you!");

                SayTo(m, true, $"Here is {gold.Amount} gold for your trouble!");

                gold.Visible = true;

                m.AddToBackpack(gold);

                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    Effects.SendBoltEffect(this);

                    Delete();
                });
            }
            else
            {
                base.OnMovement(m, oldLocation);
            }
        }

        public MissingNPC(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

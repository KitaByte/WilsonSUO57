using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.InfusionSystem.Items
{
    public class InfusionSponge : Item
    {
        public int Uses { get; private set; } = 10;

        [Constructable]
        public InfusionSponge() : base(0x1422)
        {
            Name = "Infusion Sponge";

            Hue = 2498;
        }

        public void UseSponge(Mobile from, InfusionType infusion)
        {
            if (Uses > 0)
            {
                Uses--;

                if (Uses == 0)
                {
                    from.SendMessage(53, "The sponge is used up and crumbles to dust in your hands!");

                    Delete();
                }
                else
                {
                    from.SendMessage(53, $"The sponge soaked up the {InfusionInfo.GetInfo(infusion).name} infusion!");

                    Hue = InfusionInfo.GetInfo(infusion).hue;
                }
            }
            else
            {
                from.SendMessage(53, "The sponge is used up and crumbles to dust in your hands!");

                Delete();
            }
        }

        public InfusionSponge(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable || !from.Alive)
                return;

            if (IsChildOf(from.Backpack))
            {
                from.Target = new InternalTarget(this);
            }

            base.OnDoubleClick(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(Uses);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Uses = reader.ReadInt();
        }

        private class InternalTarget : Target
        {
            private readonly InfusionSponge i_Sponge;

            public InternalTarget(InfusionSponge sponge) : base(3, false, TargetFlags.None)
            {
                i_Sponge = sponge;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (from is PlayerMobile && targeted is Item)
                {
                    PlayerMobile pm = from as PlayerMobile;

                    Item item = targeted as Item;

                    if (item.Name != null && item.Name.EndsWith("Infused]"))
                    {
                        if (Utility.RandomDouble() < InfusionInfo.GetChance(InfusionInfo.GetInfusion(item.Hue)))
                        {
                            i_Sponge.UseSponge(pm, InfusionInfo.GetInfusion(item.Hue));

                            BaseInfusion.RemoveInfusion(pm, item);
                        }
                        else
                        {
                            pm.SendMessage(53, "Failed to soak up infusion!");

                            pm.PlaySound(0x5D8);
                        }

                        pm.Animate(AnimationType.Spell, 0);
                    }
                    else
                    {
                        pm.SendMessage(53, "That is not infused!");
                    }
                }
                else
                {
                    from.SendMessage(53, "Are we having trouble with using the sponge?");
                }
            }
        }
    }
}

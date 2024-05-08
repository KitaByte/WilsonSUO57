using System;
using Server.Mobiles;

namespace Server.Custom.Misc
{
    internal class PowerHourCard : Item
    {
        private DateTime LastUsed;

        [Constructable]
        public PowerHourCard() : base(Utility.RandomList(0x9C14, 0x9C15))
        {
            SetStats();
        }

        private void SetStats()
        {
            Name = "Power Hour Card";

            Hue = 2734;

            LastUsed = DateTime.Now;

            LootType = LootType.Blessed;
        }

        public PowerHourCard(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("Must be in your backpack, to use!");

                return;
            }

            if (CheckLastUsed(from)) return;

            if (from.Alive)
            {
                PlayerMobile pm = from as PlayerMobile;

                LastUsed = DateTime.Now + TimeSpan.FromHours(24);

                pm.AcceleratedStart = DateTime.Now + TimeSpan.FromHours(1);

                pm.SendMessage("You have activated Power Hour! Enjoy your hour of advantage");

                PlaySoundEffect(pm);

                Timer.DelayCall(TimeSpan.FromHours(1), () =>
                {
                    if (pm != null && !pm.Deleted)
                    {
                        pm.AcceleratedStart = DateTime.MinValue;

                        pm.SendMessage("Your Power Hour has ended. We hope you enjoyed it!");

                        PlaySoundEffect(pm);
                    }
                });
            }

            base.OnDoubleClick(from);
        }

        private void PlaySoundEffect(PlayerMobile pm)
        {
            pm.PlaySound(0x5C3);

            pm.BoltEffect(1153);
        }

        private bool CheckLastUsed(Mobile from)
        {
            if (LastUsed > DateTime.Now)
            {
                from.SendMessage(53, $"Card currently cooling down, please try again after : " + LastUsed);

                return true;
            }

            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteDeltaTime(LastUsed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            LastUsed = reader.ReadDeltaTime();
        }
    }
}

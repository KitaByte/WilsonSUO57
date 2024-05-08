using Server.Mobiles;
using System.Linq;
using System;

namespace Server.Custom.Misc
{
    public class GroundSpellItem : Item
    {
        private PlayerMobile m_Caster;

        public GroundSpellItem(PlayerMobile caster) : base (0x1f13)
        {
            m_Caster = caster;

            Name = "Ground Item";

            Movable = false;
        }

        public GroundSpellItem(Serial serial) : base(serial)
        {
        }

        public override bool HandlesOnMovement => true;

        private bool m_Started = false;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            var mobs = GetMobilesInRange(5);

            if (HandlesOnMovement && mobs != null && mobs.Count() > 0 && mobs.Contains(m))
            {
                if (!m.Criminal && !m.Murderer && m is PlayerMobile pm)
                {
                    m_Caster.DoHarmful(pm);

                    int damage = 2;

                    damage = (int)((((m_Caster.Skills[SkillName.Magery].Value) * 0.0015) + 1) * damage);

                    if (pm != null)
                        AOS.Damage(pm, m_Caster, damage, 0, 100, 0, 0, 0);
                    else
                        AOS.Damage(pm, m_Caster, damage, 0, 100, 0, 0, 0);

                    pm.MovingParticles(pm, 0x36D4, 7, 0, false, true, 9502, 4019, 0x160);

                    pm.PlaySound(Core.AOS ? 0x15E : 0x44B);
                }
            }

            mobs.Free();

            if (!m_Started)
            {
                m_Started = true;

                Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                {
                    Delete();
                });
            }

            base.OnMovement(m, oldLocation);
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

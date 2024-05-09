using Server.Items;
using Server.Spells;
using Server.Spells.Necromancy;

namespace Server.Custom.UOHotBar
{
    public class HotBarIcon
    {
        private Spell _Spell;

        public HotBarIcon(Spell spell)
        {
            _Spell = spell;
        }

        public void SetSpell(Spell spell)
        {
            _Spell = spell;
        }

        public Spell GetSpell()
        {
            return _Spell;
        }

        public SpellInfo GetInfo()
        {
            return _Spell?.Info;
        }

        public void CastSpell()
        {
            _Spell?.Cast();
        }

        public int GetGumpIcon()
        {
            if (_Spell != null)
            {
                return HotBarArt.GetSpellArt(_Spell);
            }
            else
            {
                return 0;
            }
        }
    }

    public class test
    {
        public void something(Mobile from)
        {
            var t = new HotBarIcon(new CorpseSkinSpell(from, new CorpseSkinScroll()));

            int art = t.GetGumpIcon();
        }
    }
}

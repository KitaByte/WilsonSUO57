using Server.Gumps;
using Server.Items;
using Server.Spells;
using Server.Spells.Necromancy;

namespace Server.Custom.UOHotBar
{
    public class HotBarIcon
    {
        private Spell _Spell;

        public void SetSpell(Spell spell)
        {
            _Spell = spell;
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
            return 0; // Get Spell Gump Art
        }
    }

    public class test
    {
        public void something(Mobile from)
        {
            var t = new HotBarIcon();

            t.SetSpell(new CorpseSkinSpell(from, new CorpseSkinScroll()));
        }
    }
}

using Server.Spells;

namespace Server.Custom.UOHotBar
{
    public class HotBarIcon
    {
        private Spell _Spell;

        private SpecialMove _Move;

        public HotBarIcon(Spell spell)
        {
            _Spell = spell;
        }

        public HotBarIcon(SpecialMove move)
        {
            _Move = move;
        }

        public void SetSpell(Spell spell)
        {
            _Spell = spell;

            _Move = null;
        }

        public void SetMove(SpecialMove move)
        {
            _Move = move;

            _Spell = null;
        }

        public Spell GetSpell()
        {
            return _Spell;
        }

        public SpecialMove GetMove()
        {
            return _Move;
        }

        public void CastSpell(Mobile from)
        {
            if (_Spell != null)
            {
                _Spell.Cast();
            }
        }

        public void CastMove(Mobile from)
        {
            if (_Move != null)
            {
                SpecialMove.SetCurrentMove(from, _Move);
            }
        }

        public int GetSpellIcon()
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

        public int GetMoveIcon()
        {
            if (_Move != null)
            {
                return HotBarArt.GetMoveArt(_Move);
            }
            else
            {
                return 0;
            }
        }
    }
}

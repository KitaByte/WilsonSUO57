using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Items
{
    public class SpellFocusingSash : BodySash
    { 
        public static Dictionary<Mobile, SpellFocusingSash> SpellFocusList = new Dictionary<Mobile, SpellFocusingSash>();

        public static void Initialize()
        {
            EventSink.CastSpellRequest += EventSink_CastSpellRequest;
        }

        private static void EventSink_CastSpellRequest(CastSpellRequestEventArgs e)
        {
            if (SpellFocusList.Count > 0 && SpellFocusList.ContainsKey(e.Mobile))
            {
                if (SpellFocusList[e.Mobile].CurrentSpell > 0)
                {
                    if (SpellFocusList[e.Mobile].CurrentSpell == e.SpellID)
                    {
                        SpellFocusList[e.Mobile].CurrentCount++;
                    }
                    else
                    {
                        SpellFocusList[e.Mobile].CurrentSpell = e.SpellID;
                    }
                }
                else
                {
                    SpellFocusList[e.Mobile].CurrentSpell = e.SpellID;
                }

                if (SpellFocusList[e.Mobile].CurrentCount > 20)
                {
                    SpellFocusList[e.Mobile].CurrentCount = 0;
                }
            }
        }

        public int CurrentSpell { get; set; }

        public int CurrentCount { get; set; }

        [Constructable]
        public SpellFocusingSash()
        {
            Name = "Spell Focusing Sash";
        }

        public override bool CanEquip(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
                return true;

            if (from.Str < 10)
            {
                from.SendMessage("You are not strong enough to equip this sash.");

                return false;
            }

            return base.CanEquip(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.

                return;
            }

            if (from is PlayerMobile player)
            {
                // Implement the spell damage adjustment logic here
                // For simplicity, let's just print a message indicating activation
                player.SendMessage("You activate the Spell Focusing Sash.");
            }
        }

        public SpellFocusingSash(Serial serial) : base(serial)
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

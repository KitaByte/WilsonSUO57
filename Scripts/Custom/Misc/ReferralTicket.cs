using System.Collections.Generic;
using Server.ContextMenus;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;
using Server.Misc;

namespace Server
{
    public class ReferralTicket : Item
    {
        private static readonly string _ShardName = ServerList.ServerName;

        public static void Initialize()
        {
            EventSink.Login += EventSink_Login;
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm)
            {
                var ticket = pm.Backpack.FindItemByType(typeof(ReferralTicket));

                if (pm.Young)
                {
                    if (ticket == null)
                    {
                        pm.AddToBackpack(new ReferralTicket(pm));
                    }
                }
                else
                {
                    if (ticket != null)
                    {
                        ticket.Visible = true;

                        pm.SendMessage(53, "You received a referral ticket!");
                    }
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile TicketOwner
        {
            get
            {
                return BlessedFor;
            }

            set
            {
                if (RootParent != null && RootParent == value)
                {
                    BlessedFor = value;

                    UpdateOwner();

                    InvalidateProperties();
                }
                else
                {
                    value.SendMessage(53, "Must be in your pack in order to set blessed");
                }
            }
        }

        public ReferralTicket(PlayerMobile pm) : this()
        {
            BlessedFor = pm;
        }

        [Constructable] // Staff Override
        public ReferralTicket() : base(0x14EF)
        {
            SetInit();
        }

        private void SetInit()
        {
            Name = $"{_ShardName} Referral Ticket";

            Hue = 1266;

            Weight = 1.0;

            LootType = LootType.Blessed;

            UpdateOwner();
        }

        private void UpdateOwner()
        {
            if (BlessedFor != null)
            {
                Movable = false;

                if (BlessedFor is PlayerMobile pm)
                {
                    Visible = !pm.Young;
                }
            }
        }

        public ReferralTicket(Serial serial) : base(serial)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (RootParent == from && from.AccessLevel >= AccessLevel.Player)
            {
                list.Add(new ReferralEntry(this));
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile pm && BlessedFor == pm)
            {
                if (!pm.Young)
                {
                    from.SendMessage(53, "Target the friend who sent you to receive 500 gold!");

                    from.Target = new ReferralTarget(this);
                }
                else
                {
                    pm.SendMessage(53, "You must wait until your not 'young' in order to use this!");
                }

                base.OnDoubleClick(from);
            }
            else
            {
                if (from.AccessLevel == AccessLevel.Player)
                {
                    Delete();
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }


        // Context Menu
        private class ReferralEntry : ContextMenuEntry
        {
            private readonly ReferralTicket _Ticket;

            public ReferralEntry(ReferralTicket ticket) : base(1072364) // Delete Me
            {
                _Ticket = ticket;
            }

            public override void OnClick()
            {
                _Ticket.Delete();
            }
        }

        // Target
        private class ReferralTarget : Target
        {
            private readonly ReferralTicket _Ticket;

            public ReferralTarget(ReferralTicket ticket) : base(3, false, TargetFlags.None)
            {
                _Ticket = ticket;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile pm && !pm.Young)
                {
                    if (pm != from)
                    {
                        pm.AddToBackpack(new ReferrerReward());

                        pm.SendMessage(53, "You received a referral apron reward!");

                        from.AddToBackpack(new Gold(500));

                        from.SendMessage(53, "You received a referral gold reward!");

                        _Ticket.Delete();
                    }
                    else
                    {
                        from.SendMessage(53, "You can't target yourself!");
                    }
                }
                else
                {
                    from.SendMessage(53, "You can only target players that are not Young!");
                }
            }
        }

        // Reward
        public class ReferrerReward : HalfApron
        {
            [Constructable]
            public ReferrerReward() : base()
            {
                Name = $"{_ShardName} Referrer Apron";

                Hue = 1266;

                LootType = LootType.Blessed;

                Attributes.DefendChance = 10;

                Resistances.Poison = 5;

                Attributes.Luck = 25;
            }

            public ReferrerReward(Serial serial) : base(serial)
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
}

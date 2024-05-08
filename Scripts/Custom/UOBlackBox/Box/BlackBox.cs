using Server.Items;
using Server.Mobiles;
using Server.Services.UOBlackBox.Items;
using System;

namespace Server.Services.UOBlackBox
{
    public class BlackBox : MetalBox
    {
        public Mobile Staff;

        public BoxSession Session { get; set; }

        public BlackBox(Mobile staff) : base()
        {
            Staff = staff;

            Name = $"UO Black Box : {Staff.Name}";

            Hue = 1175;

            LootType = LootType.Blessed;

            ItemID = 0x9F4E;

            Weight = 1.0;

            Locked = false;

            Crafter = staff;

            ValidateTools();
        }

        private void ValidateTools()
        {
            if (Items.Find(t => t is BBSearchTool) == null)
            {
                AddItem(new BBSearchTool(this));
            }

            if (Items.Find(t => t is BBHueTool) == null)
            {
                AddItem(new BBHueTool(this));
            }

            if (Items.Find(t => t is BBMoveTool) == null)
            {
                AddItem(new BBMoveTool(this));
            }

            if (Items.Find(t => t is BBRandomTool) == null)
            {
                AddItem(new BBRandomTool(this));
            }

            if (Items.Find(t => t is BBRemoveTool) == null)
            {
                AddItem(new BBRemoveTool(this));
            }

            if (Items.Find(t => t is BBTravelTool) == null)
            {
                AddItem(new BBTravelTool(this));
            }

            if (Items.Find(t => t is BBHeatMapTool) == null)
            {
                AddItem(new BBHeatMapTool(this));
            }

            if (Items.Find(t => t is BBGumpEditor) == null)
            {
                AddItem(new BBGumpEditor(this));
            }

            if (Items.Find(t => t is BBGumpArtTool) == null)
            {
                AddItem(new BBGumpArtTool(this));
            }

            if (Items.Find(t => t is BBSupport) == null)
            {
                AddItem(new BBSupport(this));
            }
        }

        public BlackBox(Serial serial) : base(serial)
        {
        }

        public override void Open(Mobile from)
        {
            if (from is PlayerMobile pm)
            {
                if (pm.AccessLevel > BoxCore.StaffAccess && pm == Staff)
                {
                    if (BoxCore.NeedsUpdate)
                    {
                        pm.SendMessage(32, "UO Black Box => Requires Update!");

                        pm.LaunchBrowser(BoxCore.Url);

                        return;
                    }

                    if (ArtCore.GetTotalCount() == 0)
                    {
                        pm.SendMessage(32, "Art missing, Restart server as administrator!");

                        return;
                    }

                    if (RootParent != pm)
                    {
                        pm.Backpack.AddItem(this);
                    }

                    if (!isSetuoLoop)
                    {
                        UpdateBox();
                    }
                    else
                    {
                        isSetuoLoop = false;
                    }

                    base.Open(from);
                }
                else
                {
                    pm.SendMessage(32, $"Filth, I belong to {Staff}, not you!");

                    pm.PlaySound(0x144);

                    pm.BoltEffect(2720);

                    pm.Damage((int)(pm.Hits * 0.5));

                    pm.Say("I will never touch a Black Box again, my Bad!");

                    Delete();
                }
            }
        }

        internal bool IsOpen { get; set; } = false;

        private bool isSetuoLoop = false;

        public void UpdateBox()
        {
            if (!BoxCore.BoxTime.Running)
            {
                BoxCore.BoxTime.Start();
            }

            if (!IsOpen)
            {
                Session = new BoxSession(Staff, this);

                ValidateTools();

                Session.StartBox();

                UpdateHue(2500);
            }
            else
            {
                Session?.EndBox();
            }
        }

        internal void UpdateHue(int hue)
        {
            Hue = hue;

            Timer.DelayCall(TimeSpan.FromMilliseconds(100), () =>
            {
                isSetuoLoop = true;

                Open(Staff);
            });
        }

        public override void OnDelete()
        {
            Session?.EndBox();

            base.OnDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(Staff);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Staff = reader.ReadMobile();

            if (Hue != 1175 || !Movable || Session != null)
            {
                Hue = 1175;

                Movable = true;

                Session = null;
            }
        }
    }
}

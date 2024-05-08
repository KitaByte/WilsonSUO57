using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Services.UOBlackBox.Tools
{
    internal class TravelTool : BaseGump, IToolInfo
    {
        BoxSession Session { get; set; }

        private Map GMap { get; set; }

        private readonly List<Point3D> TravelLoc;

		private bool IsPrivate { get; set; } // todo add private POI

        public bool IsLive { get; private set; }

        private bool IsTele { get; set; }

        private bool IsTSpecial { get; set; }

        private bool IsGate { get; set; }

        private int Position { get; set; }

        public TravelTool(BoxSession session) : base(session.User, 0, 0, null)
        {
            User.SendMessage(52, $"Opening Travel Tool");

            Session = session;

            GMap = session.User.Map;

            TravelLoc = new List<Point3D>();

            IsTele = true;

            if (!BoxCore.BoxTime.Running)
            {
                BoxCore.BoxTime.Start();
            }
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Map Tool : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Use to get around with ease!");
            information.AppendLine("");
            information.AppendLine("Left Arrow - Cycle Prev Map");
            information.AppendLine("Right Arrow - Cycle Next Map");
            information.AppendLine("Green Button - Add  Current Location");
            information.AppendLine("Red Button - Remove Current Location");
            information.AppendLine("");
            information.AppendLine("Bright Buttons - Default Go Location");
            information.AppendLine("Dull Buttons - Custom Go Location");
            information.AppendLine("Stick Man - You/Live On/Off");
            information.AppendLine("Blue Man - Staff Go Location");
            information.AppendLine("Red Man - Player Go Location");
            information.AppendLine("");
            information.AppendLine("Gold Bottom L - Teleport");
            information.AppendLine("Gold Bottom M - Special");
            information.AppendLine("Gold Bottom R - Gate");
            information.AppendLine("");
            information.AppendLine("Gold Bottom R Map - Staff/Private Map");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = $"UO Black Box : {GMap.Name}";

            var header = 60;
            var margin = 70;

            var width = margin + (int)MapCore.MapWidth(GMap);
            var height = 95 + (int)MapCore.MapHeight(GMap);

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // 16x16 Prev
            AddButton(X + GumpCore.GetCentered(width, title, true) - 25, Y + 21, GumpCore.PrevBtnUP, GumpCore.PrevBtnDown, 1, GumpButtonType.Reply, 0);

            // 16x16 Next
            AddButton(X + GumpCore.GetCentered(width, title, false) + 9, Y + 21, GumpCore.NextBtnUP, GumpCore.NextBtnDown, 2, GumpButtonType.Reply, 0);

            // Map 600*Max (w/h)
            AddBackground(X + (margin / 2) - 5, Y + header - 7, (int)MapCore.MapWidth(GMap) + 10, (int)MapCore.MapHeight(GMap) + 14, IsPrivate ? GumpCore.SBlackBG : GumpCore.BlackBG);

            AddImage(X + (margin / 2), Y + header, 65000 + GMap.MapID);

            // Add Locations
            TravelLoc.Clear();

            int up;
            int down;

            var xOffset = GetWidthOffset();
            var yOffset = GetHeightOffset();

            // Xml Locations 
            var tree = new LocationTree($"{GMap.Name.ToLower()}.xml", GMap);

            if (tree != null)
            {
                tree.LastBranch.TryGetValue(User, out ParentNode branch);

                if (branch == null)
                    branch = tree.Root;

                if (branch != null)
                {
                    TraverseTree(branch, xOffset, yOffset, margin, header);
                }
            }

            // Box Locations
            AddBoxLoc(xOffset, yOffset, margin, header);

            // Add Me
            AddMe(xOffset, yOffset, margin, header);

            // Add Them
            AddPlayers(xOffset, yOffset, margin, header);

            // Add New Location
            AddButton(X - 1, Y + height - 10, GumpCore.GreenUp, GumpCore.GreenDown, 3, GumpButtonType.Reply, 0);

            // Remove Old Location
            AddButton(X + width - 10, Y + height - 10, GumpCore.RedUp, GumpCore.RedDown, 4, GumpButtonType.Reply, 0);

            // Teleport
            up = IsTele ? GumpCore.RndBtnDown : GumpCore.RndBtnUp;
            down = IsTele ? GumpCore.RndBtnUp : GumpCore.RndBtnDown;

            AddButton(X + width / 3 - 5, Y + height - 25, up, down, 6, GumpButtonType.Reply, 0);

            // Special
            up = IsTSpecial ? GumpCore.RndBtnDown : GumpCore.RndBtnUp;
            down = IsTSpecial ? GumpCore.RndBtnUp : GumpCore.RndBtnDown;

            AddButton(X + width / 2 - 5, Y + height - 25, up, down, 7, GumpButtonType.Reply, 0);

            // Gate
            up = IsGate ? GumpCore.RndBtnDown : GumpCore.RndBtnUp;
            down = IsGate ? GumpCore.RndBtnUp : GumpCore.RndBtnDown;

            AddButton(X + ((width / 3) * 2) - 5, Y + height - 25, up, down, 8, GumpButtonType.Reply, 0);

            // Set Private
            up = IsPrivate ? GumpCore.RndBtnDown : GumpCore.RndBtnUp;
            down = IsPrivate ? GumpCore.RndBtnUp : GumpCore.RndBtnDown;

            AddButton(X + width - 50, Y + height - 50, up, down, 9, GumpButtonType.Reply, 0);

            Position = 10;

            // Close
            GumpCore.SetGumpClose(this, width);
        }


        // Last ButtonID - Locations
        private int GetBtnID()
        {
            return Position + TravelLoc.Count;
        }

        // Xml locations
		private void TraverseTree(ParentNode node, float xOffset, float yOffset, int margin, int header)
		{
			foreach (var child in node.Children)
			{
				if (child is ChildNode c)
				{
					AddChild(xOffset, yOffset, margin, header, c);
				}
				else if (child is ParentNode p)
				{
					TraverseTree(p, xOffset, yOffset, margin, header);
				}
			}
		}

        private float GetWidthOffset()
        {
            return GMap.Width / MapCore.MapWidth(GMap);
        }

        private float GetHeightOffset()
        {
            return GMap.Height / MapCore.MapHeight(GMap);
        }

        private void AddChild(double xOffset, double yOffset, int margin, int header, ChildNode cn)
        {
            double x = Math.Floor(cn.Location.X / xOffset) + (margin / 2);
            double y = Math.Floor(cn.Location.Y / yOffset) + header;

            // Add a button
            AddButton(X + Convert.ToInt32(x), Y + Convert.ToInt32(y), GumpCore.SmBtnUp, GumpCore.SmBtnDown, GetBtnID(), GumpButtonType.Reply, 0);

            TravelLoc.Add(cn.Location);
        }

        private void AddMe(double xOffset, double yOffset, int margin, int header)
        {
            if (User.Map.MapID == GMap.MapID)
            {
                double x = Math.Floor(User.Location.X / xOffset) + (margin / 2);
                double y = Math.Floor(User.Location.Y / yOffset) + header;

                // Add a button
                AddButton(X + Convert.ToInt32(x), Y + Convert.ToInt32(y), GumpCore.MeIcon, GumpCore.SmBtnDown, 5, GumpButtonType.Reply, 0);
            }
        }

        private void AddPlayers(double xOffset, double yOffset, int margin, int header)
        {
            foreach (var player in World.Mobiles.Values)
            {
                if (player is PlayerMobile pm)
                {
                    if (pm != User && pm.Map.MapID == GMap.MapID)
                    {
                        double x = Math.Floor(pm.Location.X / xOffset) + (margin / 2);
                        double y = Math.Floor(pm.Location.Y / yOffset) + header;

                        // Add a button 
                        if (pm.AccessLevel > AccessLevel.VIP)
                        {
                            AddButton(X + Convert.ToInt32(x), Y + Convert.ToInt32(y), GumpCore.StaffIcon, GumpCore.StaffIcon, GetBtnID(), GumpButtonType.Reply, 0);
                        }
                        else
                        {
                            AddButton(X + Convert.ToInt32(x), Y + Convert.ToInt32(y), GumpCore.PlayerIcon, GumpCore.PlayerIcon, GetBtnID(), GumpButtonType.Reply, 0);
                        }

                        TravelLoc.Add(pm.Location);
                    }
                }
            }
        }

        private void AddBoxLoc(double xOffset, double yOffset, int margin, int header)
        {
            if (IsPrivate)
            {
                if (GumpCore.StaffLocations.Count > 0)
                {
                    var (Staff, SLoc) = GumpCore.StaffLocations.FirstOrDefault(s => s.Staff == User);

                    foreach (var (BMap, BLoc) in SLoc)
                    {
                        if (BMap == GMap)
                        {
                            double x = Math.Floor(BLoc.X / xOffset) + (margin / 2);
                            double y = Math.Floor(BLoc.Y / yOffset) + header;

                            // Add a button
                            AddButton(X + Convert.ToInt32(x), Y + Convert.ToInt32(y), GumpCore.SmBtnDown, GumpCore.SmBtnUp, GetBtnID(), GumpButtonType.Reply, 0);

                            TravelLoc.Add(BLoc);
                        }
                    }
                }
            }
            else
            {
                if (GumpCore.BoxLocations.Count > 0)
                {
                    foreach (var (BMap, BLoc) in GumpCore.BoxLocations)
                    {
                        if (BMap == GMap)
                        {
                            double x = Math.Floor(BLoc.X / xOffset) + (margin / 2);
                            double y = Math.Floor(BLoc.Y / yOffset) + header;

                            // Add a button
                            AddButton(X + Convert.ToInt32(x), Y + Convert.ToInt32(y), GumpCore.SmBtnDown, GumpCore.SmBtnUp, GetBtnID(), GumpButtonType.Reply, 0);

                            TravelLoc.Add(BLoc);
                        }
                    }
                }
            }
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                switch (info.ButtonID)
                {
                    case 1:
                        {
                            if (GMap.MapID <= 0)
                            {
                                GMap = Map.TerMur;
                            }
                            else
                            {
                                GMap = Map.Maps[GMap.MapID - 1];
                            }

                            break;
                        }
                    case 2:
                        {
                            if (GMap.MapID >= 5)
                            {
                                GMap = Map.Felucca;
                            }
                            else
                            {
                                GMap = Map.Maps[GMap.MapID + 1];
                            }

                            break;
                        }
                    case 3:
                        {
                            if (IsPrivate)
                            {
                                if (GumpCore.AddStaffLocation(User, GMap, User.Location))
                                {
                                    User.SendMessage(52, $"{User.Location} - Added!");
                                }
                                else
                                {
                                    User.SendMessage(32, $"{User.Location} - Already Exist!");
                                }
                            }
                            else
                            {
                                if (GumpCore.AddBoxLocation(GMap, User.Location))
                                {
                                    User.SendMessage(52, $"{User.Location} - Added!");
                                }
                                else
                                {
                                    User.SendMessage(32, $"{User.Location} - Already Exist!");
                                }
                            }

                            break;
                        }
                    case 4:
                        {
                            if (IsPrivate)
                            {
                                if (GumpCore.RemoveStaffLocation(User, GMap, User.Location))
                                {
                                    User.SendMessage(52, $"{User.Location} - Removed!");
                                }
                                else
                                {
                                    User.SendMessage(32, $"{User.Location} - Doesn't Exist!");
                                }
                            }
                            else
                            {
                                if (GumpCore.RemoveBoxLocation(GMap, User.Location))
                                {
                                    User.SendMessage(52, $"{User.Location} - Removed!");
                                }
                                else
                                {
                                    User.SendMessage(32, $"{User.Location} - Doesn't Exist!");
                                }
                            }

                            break;
                        }
                    case 5:
                        {
                            if (IsLive)
                            {
                                IsLive = false;

                                User.SendMessage(32, "Live Tracking - Stopped!");

								BoxCore.RunBBCommand(User, "m Tele");
                            }
                            else
                            {
                                IsLive = true;

                                User.SendMessage(52, "Live Tracking - Started!");
                            }

                            break;
                        }
                    case 6:
                        {
                            if (!IsTele)
                            {
                                IsTele = true;
                                IsTSpecial = false;
                                IsGate = false;

                                User.SendMessage(52, "Teleport Travel On!");
                            }

                            break;
                        }
                    case 7:
                        {
                            if (!IsTSpecial)
                            {
                                IsTele = false;
                                IsTSpecial = true;
                                IsGate = false;

                                User.SendMessage(52, "Special Travel On!");
                            }

                            break;
                        }
                    case 8:
                        {
                            if (!IsGate)
                            {
                                IsTele = false;
                                IsTSpecial = false;
                                IsGate = true;

                                User.SendMessage(52, "Gate Travel On!");
                            }

                            break;
                        }
                    case 9:
                        {
                            IsPrivate = !IsPrivate;

                            var onOff = IsPrivate ? "On" : "Off";

                            User.SendMessage(IsPrivate ? 52 : 32, $"Private Map {onOff}!");

                            break;
                        }
                    default:
                        {
                            if (info.ButtonID < TravelLoc.Count + Position + 1)
                            {
                                var location = TravelLoc[info.ButtonID - Position];

                                if (IsTele)
                                {
                                    User.Hidden = true;

                                    User.MoveToWorld(location, GMap);

                                    User.SendMessage(52, $"You teleported to {User.Map.Name} {User.Location}");
                                }

                                if (IsTSpecial)
                                {

                                    GMHidingStone.SendStoneEffects((StoneEffect)Utility.Random(19), 0, User);

                                    User.MoveToWorld(location, GMap);

                                    GMHidingStone.SendStoneEffects((StoneEffect)Utility.Random(19), 0, User);

                                    User.SendMessage(52, $"You teleported to {User.Map.Name} {User.Location}");

                                    User.Hidden = false;
                                }

                                if (IsGate)
                                {
                                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                                    {
                                        User.SendMessage(52, $"Gate has opened to {User.Map.Name} {User.Location}");

                                        Effects.PlaySound(User.Location, User.Map, 0x20E);

                                        InternalItem firstGate = new InternalItem(location, GMap);
                                        firstGate.MoveToWorld(User.Location, User.Map);

                                        Effects.PlaySound(location, GMap, 0x20E);

                                        InternalItem secondGate = new InternalItem(User.Location, User.Map);
                                        secondGate.MoveToWorld(location, GMap);

                                        firstGate.LinkedGate = secondGate;
                                        secondGate.LinkedGate = firstGate;
                                    });
                                }

                            }
                            else
                            {
                                GumpCore.SendInfoGump(Session, this);
                            }

                            break;
                        }
                }

                Session.UpdateBox("Travel");

                Refresh(true, false);
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }

        private class InternalItem : Moongate
        {
            [CommandProperty(AccessLevel.GameMaster)]
            public Moongate LinkedGate { get; set; }

            public InternalItem(Point3D target, Map map) : base(target, map)
            {
                Map = map;

                Hue = Utility.RandomBrightHue();

                Dispellable = false;

                InternalTimer t = new InternalTimer(this);

                t.Start();
            }

            public override void UseGate(Mobile m)
            {
                if (LinkedGate == null || !(LinkedGate is InternalItem) || !LinkedGate.Deleted)
                {
                    if (LinkedGate != null && m.AccessLevel < AccessLevel.Counselor)
                    {
                        m.SendMessage(32, "Staff Only!");

                        return;
                    }

                    base.UseGate(m);
                }
                else
                    m.SendMessage("The other gate no longer exists.");
            }

            public override void OnLocationChange(Point3D old)
            {
                base.OnLocationChange(old);
            }

            public override void OnMapChange()
            {
                base.OnMapChange();
            }

            public InternalItem(Serial serial) : base(serial)
            {
            }

            public override bool ShowFeluccaWarning
            {
                get
                {
                    return false;
                }
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                Delete();
            }

            private class InternalTimer : Timer
            {
                private readonly Item m_Item;

                public InternalTimer(Item item) : base(TimeSpan.FromSeconds(10.0))
                {
                    Priority = TimerPriority.OneSecond;

                    m_Item = item;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }
        }
    }
}

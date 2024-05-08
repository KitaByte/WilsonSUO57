using System.Collections.Generic;
using System.Linq;
using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class ArtViewer : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        private List<ArtEntity> ArtList { get; set; }

        public string ListName { get; set; }

        private readonly int ArtWidth = 0;

        private readonly int ArtHeight = 0;

        private int Position = 0;

        private int NextPosition = 0;

        public bool IsNoDraw = false;

        public bool IsHex = false;

        public bool IsTransOn = false;

        private int LastSelected = 0;

        public ArtViewer(BoxSession session, List<ArtEntity> artList, string name) : base(session.User, 0, 0, null)
        {
            Session = session;

            ArtList = artList;

            ListName = name;

            ArtWidth = artList.OrderByDescending(i => i.Width).FirstOrDefault().Width + 10;

            ArtHeight = artList.OrderByDescending(i => i.Height).FirstOrDefault().Height + 20;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Art Viewer : Instructions");
            information.AppendLine("-------------------------");
            information.AppendLine("");
            information.AppendLine("Left/Right Arrow - Navigate Page");
            information.AppendLine("First Gold Button - Toggle NoDraw Tile");
            information.AppendLine("Second Gold Button - Toggle Art Hex/Int");
            information.AppendLine("Third Gold Button - Toggle Transparency");
            information.AppendLine("Fourth Minus Button - Minimize");
            information.AppendLine("");
            information.AppendLine("Art Display Slot Info");
            information.AppendLine("---------------------");
            information.AppendLine("");
            information.AppendLine("Blue Button - Select Art");
            information.AppendLine("Gold Button - Pop Out Selected");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var margin = 50;
            var space = 25;

            var numColumns = 600 / ArtWidth;
            var numRows = 600 / ArtHeight < 1 ? 1 : 600 / ArtHeight;

            var perPage = numRows * numColumns;

            var width = margin * 2 + (ArtWidth + space) * numColumns - space;
            var height = margin * 2 + (ArtHeight + space) * numRows - space;

            NextPosition = Position + perPage;

            var text = $"{ListName} Static Art - Page {NextPosition / perPage} of {ArtList.Count / perPage}".TrimStart();

            AddBackground(X, Y, width, height + 20, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, text, true), Y + 25, GumpCore.GoldText, text);

            // 16x16 Prev
            if (NextPosition / perPage > 1)
            {
                AddButton(X + GumpCore.GetCentered(width, text, true) - 40, Y + 25, GumpCore.PrevBtnUP, GumpCore.PrevBtnDown, NextPosition + 1, GumpButtonType.Reply, 0);
            }

            // 16x16 Next
            if (NextPosition / perPage < ArtList.Count / perPage)
            {
                AddButton(X + GumpCore.GetCentered(width, text, false) + 24, Y + 25, GumpCore.NextBtnUP, GumpCore.NextBtnDown, NextPosition + 2, GumpButtonType.Reply, 0);
            }

            // NoDraw
            AddButton(X + width - 85, Y + 25, GumpCore.RndBtnUp, GumpCore.RndBtnDown, NextPosition + 3, GumpButtonType.Reply, 0);

            // Hex
            AddButton(X + width - 70, Y + 25, GumpCore.RndBtnUp, GumpCore.RndBtnDown, NextPosition + 4, GumpButtonType.Reply, 0);

            // Trans
            AddButton(X + width - 55, Y + 25, GumpCore.RndBtnUp, GumpCore.RndBtnDown, NextPosition + 5, GumpButtonType.Reply, 0);

            // Min
            AddButton(X + width - 40, Y + 23, GumpCore.MinBtn, GumpCore.MinBtn, NextPosition + 6, GumpButtonType.Reply, 0);

            // Close
            GumpCore.SetGumpClose(this, width);

            // Art Slots
            for (int i = Position; i < NextPosition && i < ArtList.Count; i++)
            {
                var row = (i - Position) / numColumns;
                var col = (i - Position) % numColumns;

                var artEntity = ArtList[i];
                var artId = artEntity.ID;

                var x = X + margin + (col * (ArtWidth + space)) + (ArtWidth - artEntity.Width) / 2;
                var y = Y + margin + 20 + (row * (ArtHeight + space)) + (ArtHeight - artEntity.Height);

                var xx = X + margin + (col * (ArtWidth + space));
                var yy = Y + margin + 20 + (row * (ArtHeight + space));

                var xs = X + margin + (col * (ArtWidth + space)) + (ArtWidth - 44) / 2;
                var ys = Y + margin + 20 + (row * (ArtHeight + space)) + (ArtHeight - 44);

                AddBackground(xx - 10, yy - 10, ArtWidth + 20, ArtHeight + 20, GumpCore.SubBG);

                if (IsTransOn)
                    AddAlphaRegion(xx, yy, ArtWidth, 20);

                if (IsNoDraw)
                    AddItem(xs, ys, 1, 0x4001);

                if (LastSelected == i)
                {
                    AddItem(x, y, artId, GumpCore.GoldText + 1);

                    AddButton(xx + 2, yy + 5, GumpCore.RndBtnUp, GumpCore.RndBtnDown, i + 1, GumpButtonType.Reply, 0);

                    AddLabel(xx + 16, yy + 1, GumpCore.WhtText, IsHex ? artEntity.Hex.Split('x').Last() : artEntity.ID.ToString());
                }
                else
                {
                    AddItem(x, y, artId, ArtList[i].Hue);

                    AddButton(xx + 2, yy + 5, GumpCore.RndBtnDown, GumpCore.RndBtnUp, i + 1, GumpButtonType.Reply, 0);

                    AddLabel(xx + 16, yy + 1, GumpCore.GoldText, IsHex ? artEntity.Hex.Split('x').Last() : artEntity.ID.ToString());
                }
            }

            // Art Info
            if (LastSelected >= 0)
            {
                var entity = ArtList[LastSelected];

                var textSel = $"[{entity.Name}] {entity.ID} : {entity.Hex} ({entity.Width}x{entity.Height})";

                AddLabel(X + GumpCore.GetCentered(width, textSel, true), Y + height - 15, GumpCore.WhtText, textSel);
            }
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                var btnID = info.ButtonID > NextPosition ? info.ButtonID : info.ButtonID - 1;

                if (btnID > NextPosition)
                {
                    if (btnID == NextPosition + 1) // Prev
                    {
                        Position -= (NextPosition - Position);

                        if (Position < 0)
                        {
                            Position = 0;
                        }
                    }

                    if (btnID == NextPosition + 2) // Next
                    {
                        if (NextPosition < ArtList.Count)
                        {
                            Position = NextPosition;
                        }
                    }

                    if (btnID == NextPosition + 3) // NoDraw
                        IsNoDraw = !IsNoDraw;

                    if (btnID == NextPosition + 4) // IsHex
                        IsHex = !IsHex;

                    if (btnID == NextPosition + 5) // IsTrans
                        IsTransOn = !IsTransOn;

                    if (btnID == NextPosition + 6) // Min
                    {
                        SendGump(new ArtViewerMin(Session, this));

                        Session.UpdateBox("Min");
                    }
                    else
                    {
                        if (btnID == 123456)
                        {
                            GumpCore.SendInfoGump(Session, this);
                        }
                        else
                        {
                            Session.UpdateBox("Menu Toggled");
                        }

                        Refresh(true, false);
                    }

                    if (Children.Count > 0)
                    {
                        for (int i = 0; i < Children.Count; i++)
                        {
                            if (Children[i].Open)
                            {
                                Children[i].Refresh(true, false);
                            }
                            else
                            {
                                Children.RemoveAt(i);
                            }
                        }
                    }
                }
                else
                {
                    if (LastSelected != btnID)
                    {
                        LastSelected = btnID;
                    }
                    else
                    {
                        var art = new ArtEntity(ArtList[btnID].Name, ArtList[btnID].ID, ArtList[btnID].Width, ArtList[btnID].Height, null);

                        art.SendGump(Session, this);
                    }

                    Session.ArtSelected = ArtList[btnID].ID;

                    Session.HueSelected = ArtList[btnID].Hue;

                    Session.UpdateBox("Art Selected");

                    Refresh(true, false);
                }
            }
            else
            {
                if (Children.Count > 0)
                {
                    Children.Clear();
                }

                Session.UpdateBox("Close");
            }
        }
    }
}

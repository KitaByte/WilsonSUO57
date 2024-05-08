using System;
using System.Collections.Generic;
using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class HeatMapTool : BaseGump, IToolInfo
    {
        BoxSession Session { get; set; }

        private Map GMap { get; set; }

        private List<(Map, Point3D)> CurrentData { get; set; }

        private DataTypes DataType { get; set; }

        private enum DataTypes
        {
            Craft,
            CreatureDeath,
            PlayerDeath,
            Quest,
            Harvest,
            Tame,
            Buy,
            Sell
        }

        public HeatMapTool(BoxSession session) : base(session.User, 0, 0, null)
        {
            User.SendMessage(52, $"Opening Heat Map Tool");

            Session = session;

            GMap = session.User.Map;

            CurrentData = GameInfo.CraftData;

            DataType = DataTypes.Craft;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Heat Map Tool : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Use to track activity!");
            information.AppendLine("");
            information.AppendLine("Cycle Maps via arrow keys");
            information.AppendLine("Cycle Events via bottom right button");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = $"{GMap.Name} : {DataType}";

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
            AddButton(X + GumpCore.GetCentered(width, title, false) + 25, Y + 21, GumpCore.NextBtnUP, GumpCore.NextBtnDown, 2, GumpButtonType.Reply, 0);

            // Map 600*Max (w/h)
            AddBackground(X + (margin / 2) - 5, Y + header - 7, (int)MapCore.MapWidth(GMap) + 10, (int)MapCore.MapHeight(GMap) + 14, GumpCore.BlackBG);

            AddImage(X + (margin / 2), Y + header, 65000 + GMap.MapID);

            // Add Locations

            var xOffset = GetWidthOffset();
            var yOffset = GetHeightOffset();

            if (CurrentData != null && CurrentData.Count > 0)
            {
                foreach (var (map, loc) in CurrentData)
                {
                    if (map == GMap)
                    {
                        double x = Math.Floor(loc.X / xOffset) + (margin / 2);
                        double y = Math.Floor(loc.Y / yOffset) + header;

                        // Add Marker
                        AddImage(X + Convert.ToInt32(x), Y + Convert.ToInt32(y), GumpCore.SmPinArt, 1171);
                    }
                }
            }

            // Cycle Data
            AddButton(X + width - 50, Y + height - 50, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 3, GumpButtonType.Reply, 0);

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        private float GetWidthOffset()
        {
            return GMap.Width / MapCore.MapWidth(GMap);
        }

        private float GetHeightOffset()
        {
            return GMap.Height / MapCore.MapHeight(GMap);
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
                            switch (DataType)
                            {
                                case DataTypes.Craft:           { CurrentData = GameInfo.CDeathData;    DataType = DataTypes.CreatureDeath; break; }
                                case DataTypes.CreatureDeath:   { CurrentData = GameInfo.PDeathData;    DataType = DataTypes.PlayerDeath; break; }
                                case DataTypes.PlayerDeath:     { CurrentData = GameInfo.QuestData;     DataType = DataTypes.Quest; break; }
                                case DataTypes.Quest:           { CurrentData = GameInfo.HarvestData;   DataType = DataTypes.Harvest; break; }
                                case DataTypes.Harvest:         { CurrentData = GameInfo.TameData;      DataType = DataTypes.Tame; break; }
                                case DataTypes.Tame:            { CurrentData = GameInfo.BuyData;       DataType = DataTypes.Buy; break; }
                                case DataTypes.Buy:             { CurrentData = GameInfo.SellData;      DataType = DataTypes.Sell; break; }
                                case DataTypes.Sell:            { CurrentData = GameInfo.CraftData;     DataType = DataTypes.Craft; break; }
                            }

                            break;
                        }
                    default:
                        {
                            GumpCore.SendInfoGump(Session, this);

                            break;
                        }
                }

                Session.UpdateBox("Heat Map");

                Refresh(true, false);
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }
    }
}

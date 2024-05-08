using System.IO;
using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class GumpEditor : BaseGump, IToolInfo
    {
        private string FilePath { get; set; }

        public BoxSession Session { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public GumpEditor(BoxSession session) : base(session.User, 0, 0, null)
        {
            User.SendMessage(52, $"Opening Gump Editor");

            Session = session;

            if (!Directory.Exists(@"UOBlackBox\Output\Gumps"))
            {
                Directory.CreateDirectory(@"UOBlackBox\Output\Gumps");
            }

            var idCheck = Directory.GetFiles(@"UOBlackBox\Output\Gumps");

            int id = 0;

            if (idCheck != null)
            {
                id = idCheck.Length;
            }

            FilePath = Path.Combine(@"UOBlackBox\Output\Gumps", $"{User.Name}Gump{id + 1}.cs");
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Gump Editor : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Used to make gumps");
            information.AppendLine("");
            information.AppendLine("First Gold Button - Add Element");
            information.AppendLine("Second Gold Button - Save Script");
            information.AppendLine("First Gold Button - Reset Gump");
            information.AppendLine("");
            information.AppendLine("Element Controls");
            information.AppendLine("");
            information.AppendLine("Gold Button - Edit Element");
            information.AppendLine("Red Button - Remove Element");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "UO Black Box : Gump Editor";

            Width = 350;

            Height = 60;

            if (Session.Layers.Count > 0)
            {
                var layerHeight = 75 + (25 * Session.Layers.Count);

                foreach (var layer in Session.Layers)
                {
                    if (Width < layer.Prams.Width + 210)
                    {
                        Width = layer.Prams.Width + 210;
                    }

                    if (Height < layer.Prams.Height + 75)
                    {
                        Height = layer.Prams.Height + 75;
                    }
                }

                if (Height < layerHeight)
                {
                    Height = layerHeight;
                }
            }

            AddBackground(X, Y, Width, Height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(Width, title, true), Y + 20, GumpCore.GoldText, title);

            // Add Element
            AddButton(X + 25, Y + 23, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 1, GumpButtonType.Reply, 0);

            // Save Element
            AddButton(X + 45, Y + 23, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 2, GumpButtonType.Reply, 0);

            // Load Element
            AddButton(X + 65, Y + 23, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 3, GumpButtonType.Reply, 0);

            // Layers
            if (Session.Layers != null && Session.Layers.Count > 0)
            {
                var count = 4;
                var offset = 1;

                foreach (var layer in Session.Layers)
                {
                    layer.AddGumpElement(this);

                    var id = layer.Prams.Art == 0 ? layer.Prams.ArtUp == 0 ? 0 : layer.Prams.ArtUp : layer.Prams.Art;

                    AddButton(X + Width - 175, Y + 25 + (25 * offset), GumpCore.RedUp, GumpCore.RedDown, count, GumpButtonType.Reply, 0);

                    count++;

                    AddButton(X + Width - 160, Y + 25 + (25 * offset), GumpCore.RndBtnUp, GumpCore.RndBtnDown, count, GumpButtonType.Reply, 0);

                    AddLabel(X + Width - 145, Y + 22 + (25 * offset), GumpCore.GoldText, $"{id} : {layer.Element}");

                    count++;
                    offset++;
                }
            }

            // Close
            GumpCore.SetGumpClose(this, Width);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                switch (info.ButtonID)
                {
                    case 1:
                        {
                            if (User.HasGump(typeof(GumpElements)))
                            {
                                User.CloseGump(typeof(GumpElements));
                            }

                            SendGump(new GumpElements(Session));

                            break;
                        }
                    case 2:
                        {
                            var idCheck = Directory.GetFiles(@"UOBlackBox\Output\Gumps");

                            int id = 0;

                            if (idCheck != null)
                            {
                                id = idCheck.Length;
                            }

                            File.WriteAllLines(FilePath, GumpScript.ScriptSetup(Session, id + 1));

                            User.SendMessage(52, "Script Saved!");

                            break;
                        }
                    case 3:
                        {
                            Session.Layers.Clear();

                            User.SendMessage(42, "Gump Cleared!");

                            break;
                        }
                    default:
                        {
                            if (info.ButtonID < 12345)
                            {
                                if (info.ButtonID % 2 == 0)
                                {
                                    if (Session.Layers.Count > info.ButtonID - 4)
                                    {
                                        Session.Layers.RemoveAt(info.ButtonID - 4);
                                    }
                                }
                                else
                                {
                                    var layer = Session.Layers[info.ButtonID - 5];

                                    var tuneGump = new GumpTune(Session, layer.Prams)
                                    {
                                        Layer = layer
                                    };

                                    SendGump(tuneGump);
                                }

                                Session.UpdateBox("Edit Element");
                            }
                            else
                            {
                                GumpCore.SendInfoGump(Session, this);

                                Session.UpdateBox("Info Selection");
                            }

                            break;
                        }
                }

                Refresh(true, false);
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }
    }
}

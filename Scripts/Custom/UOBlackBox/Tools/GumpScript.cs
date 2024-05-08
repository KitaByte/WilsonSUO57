using System;
using System.Text;

namespace Server.Services.UOBlackBox.Tools
{
    public static class GumpScript
    {
        public static string[] ScriptSetup(BoxSession session, int id)
        {
            var script = new StringBuilder();

            script.AppendLine($"//{session.User.Name}Gump{id} by UO Black Box © 2023");
            script.AppendLine("");
            script.AppendLine("using Server.Gumps;");
            script.AppendLine("");
            script.AppendLine($"public class {session.User.Name}Gump{id} : BaseGump");
            script.AppendLine("{");
            script.AppendLine($"     public {session.User.Name}Gump{id}(PlayerMobile user, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)");
            script.AppendLine("     {");
            script.AppendLine("     }");
            script.AppendLine("");
            script.AppendLine("     public override void AddGumpLayout()");
            script.AppendLine("     {");
            script.AppendLine("         Closable = true;");
            script.AppendLine("         Dragable = true;");
            script.AppendLine("         Resizable = false;");
            script.AppendLine("");

            if (session.Layers.Count > 0)
            {
                foreach (var layer in session.Layers)
                {
                    AddLayer(ref script, layer);
                }
            }

            script.AppendLine("     }");
            script.AppendLine("");
            script.AppendLine("     OnResponse(RelayInfo info)");
            script.AppendLine("     {");
            script.AppendLine("         base.OnResponse(info);"); // todo : Add Info Responses
            script.AppendLine("     }");
            script.AppendLine("}");

            return script.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        private static void AddLayer(ref StringBuilder s, GumpLayer l)
        {
            var p = l.Prams;

            switch (l.Element)
            {
                case Elements.Alpha:
                    s.AppendLine($"         AddAlphaRegion({p.LocX}, {p.LocY}, {p.Width}, {p.Height});");
                    break;
                case Elements.Background:
                    s.AppendLine($"         AddBackground({p.LocX}, {p.LocY}, {p.Width}, {p.Height}, {p.Art});");
                    break;
                case Elements.Button:
                    s.AppendLine($"         AddButton({p.LocX}, {p.LocY}, {p.ArtUp}, {p.ArtDown}, {p.ID}, GumpButtonType.Reply, 0);");
                    break;
                case Elements.Check:
                    s.AppendLine($"         AddCheck({p.LocX}, {p.LocY}, {p.ArtUp}, {p.ArtDown}, {p.IntState}, {p.ID});");
                    break;
                case Elements.Html:
                    s.AppendLine($"         AddHtml({p.LocX}, {p.LocY}, {p.Width}, {p.Height}, {p.Text}, {p.HasBack}, {p.HasBar});");
                    break;
                case Elements.Image:
                    s.AppendLine($"         AddImage({p.LocX}, {p.LocY}, {p.Art}, {p.Hue});");
                    break;
                case Elements.ImageTiled:
                    s.AppendLine($"         AddImageTiled({p.LocX}, {p.LocY}, {p.Width}, {p.Height}, {p.Art});");
                    break;
                case Elements.Item:
                    s.AppendLine($"         gump.AddItem({p.LocX}, {p.LocY}, {p.Art}, {p.Hue});");
                    break;
                case Elements.Label:
                    s.AppendLine($"         AddLabel({p.LocX}, {p.LocY}, {p.Hue}, {p.Text});");
                    break;
                case Elements.LabelCropped:
                    s.AppendLine($"         AddLabelCropped({p.LocX}, {p.LocY}, {p.Width}, {p.Height}, {p.Hue}, {p.Text});");
                    break;
                case Elements.Radio:
                    s.AppendLine($"         AddRadio({p.LocX}, {p.LocY}, {p.ArtUp}, {p.ArtDown}, {p.IntState}, {p.ID});");
                    break;
                case Elements.TextEntry:
                    s.AppendLine($"         AddTextEntry({p.LocX}, {p.LocY}, {p.Width}, {p.Height}, {p.Hue}, {p.ID}, {p.Text});");
                    break;
                case Elements.ToolTip:
                    s.AppendLine($"         AddTooltip({p.Text});");
                    break;
            }
        }
    }
}

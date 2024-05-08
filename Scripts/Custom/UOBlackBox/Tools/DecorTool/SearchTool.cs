using System.Collections.Generic;
using System.Linq;
using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class SearchTool : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        private string Message = "";

        private string LastSearch = "Search";

        private int LastCount = 0;

        public SearchTool(BoxSession session) : base(session.User, 0, 0, null)
        {
            User.SendMessage(52, $"Opening Search Tool");

            Session = session;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Search Tool : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Search Input - Enter Search");
            information.AppendLine("First Button - Search Small Art");
            information.AppendLine("Second Button - Search Med Art");
            information.AppendLine("Third Button - Search Large Art");
            information.AppendLine("Forth Button - Search XLarge Art");
            information.AppendLine("Fifth Button - Search All Art");
            information.AppendLine("Last Blue Button - Find Art Target");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "UO Black Box : Search Tool";

            var width = 250;
            var height = 105;

            AddBackground(X + 0, Y + 0, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // Search query
            AddTextEntry(X + 25, Y + 45, 120, 25, GumpCore.WhtText, 0, LastSearch);

            // Small
            AddButton(X + 140, Y + 49, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 1, GumpButtonType.Reply, 0);

            // Med
            AddButton(X + 155, Y + 49, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 2, GumpButtonType.Reply, 0);

            // large
            AddButton(X + 170, Y + 49, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 3, GumpButtonType.Reply, 0);

            // XLarge
            AddButton(X + 185, Y + 49, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 4, GumpButtonType.Reply, 0);

            // All
            AddButton(X + 200, Y + 49, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 5, GumpButtonType.Reply, 0);

            // Find Target
            AddButton(X + 215, Y + 49, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 6, GumpButtonType.Reply, 0);

            // Close
            GumpCore.SetGumpClose(this, width);

            // Default message to be displayed
            if (Message == "")
            {
                var start = $"{User.Name}, Awaiting Search!";

                AddLabel(X + GumpCore.GetCentered(width, start, true), Y + 70, GumpCore.GoldText, start);
            }
            else
            {
                if (LastCount > 0)
                {
                    AddLabel(X + GumpCore.GetCentered(width, Message, true), Y + 70, GumpCore.GoldText, Message);
                }
                else
                {
                    AddLabel(X + GumpCore.GetCentered(width, Message, true), Y + 70, 32, Message);
                }
            }
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                LastSearch = info.GetTextEntry(0).Text;

                if (!string.IsNullOrWhiteSpace(LastSearch) && LastSearch.Length < 100 && info.ButtonID < 6) // Search Lists
                {
                    if (LastSearch != "Search")
                    {
                        var searchResults = SearchArt(LastSearch, info.ButtonID - 1);

                        LastSearch = BoxCore.CapitalizeWords(LastSearch);

                        if (searchResults.Any())
                        {
                            if (searchResults.Count > 1)
                            {
                                SendGump(new ArtViewer(Session, searchResults, ArtCore.GetListName(info.ButtonID)));
                            }
                            else
                            {
                                SendGump(new ArtPopView(Session, searchResults[0], null));
                            }

                            LastCount = searchResults.Count;
                        }
                        else
                        {
                            OpenArtList(info.ButtonID);

                            LastCount = 0;
                        }

                        Message = $"*Found {LastCount} [{LastSearch}] in {ArtCore.GetListName(info.ButtonID)} Art*";
                    }
                    else
                    {
                        OpenArtList(info.ButtonID);
                    }
                }

                if (info.ButtonID < 7)
                {
                    if (info.ButtonID == 6) // Find Target Art
                    {
                        User.Target = new FindArt(this);
                    }

                    Session.UpdateBox("Search");
                }
                else
                {
                    GumpCore.SendInfoGump(Session, this);
                }

                Refresh(true, false);
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }

        public List<ArtEntity> SearchArt(string query, int artPool)
        {
            var artList = ArtCore.LoadArtList(artPool);

            query = query.ToLower();

            return artList.Where(art => art.Name.ToLower().Contains(query)
                    || art.ID.ToString().Contains(query)
                    || art.Hex.ToLower().Contains(query)).ToList();
        }

        private void OpenArtList(int id)
        {
            switch (id)
            {
                case 1: SendGump(new ArtViewer(Session, ArtCore.SmallArt, ArtCore.GetListName(id))); break;
                case 2: SendGump(new ArtViewer(Session, ArtCore.MedArt, ArtCore.GetListName(id))); break;
                case 3: SendGump(new ArtViewer(Session, ArtCore.LargeArt, ArtCore.GetListName(id))); break;
                case 4: SendGump(new ArtViewer(Session, ArtCore.XLargeArt, ArtCore.GetListName(id))); break;
                case 5: SendGump(new ArtViewer(Session, ArtCore.AllStaticArt, ArtCore.GetListName(id))); break;
                default:
                    {
                        GumpCore.SendInfoGump(Session, this);

                        break;
                    }
            }
        }

        public void SetLastSearch(string search)
        {
            LastSearch = search;
        }
    }
}

using System.Linq;

using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Services.UOBlackBox.Tools
{
    public class FindArt : Target
    {
        private readonly SearchTool Tool;

        public FindArt(SearchTool tool) : base (100, false, TargetFlags.None)
        {
            Tool = tool;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (from is PlayerMobile pm)
            {
                int id = 0;

                if (id == 0 && targeted is StaticTarget st)
                {
                    id = st.ItemID;
                }

                if (targeted is Static s)
                {
                    id = s.ItemID;
                }

                if (id == 0 && targeted is BoxStatic b)
                {
                    id = b.ItemID;
                }

                if (id == 0 && targeted is Item i)
                {
                    id = i.ItemID;
                }

                if (id > 0)
                {
                    var artEntity = Tool.SearchArt(id.ToString(), 4).First();

                    if (artEntity != null)
                    {
                        BaseGump.SendGump(new ArtPopView(Tool.Session, artEntity, null));

                        Tool.SetLastSearch(artEntity.ID.ToString());

                        Tool.Refresh(true, false);
                    }
                }
                else
                {
                    pm.SendMessage(32, "Did not find any art matching the target!");
                }
            }
        }
    }
}

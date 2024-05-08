using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Services.UOBlackBox.Tools
{
    public class ReplaceArt : Target
    {
        private ArtEntity Art;

        public ReplaceArt(ArtEntity art) : base (100, false, TargetFlags.None)
        {
            Art = art;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (from is PlayerMobile pm)
            {
                if (targeted is Static st)
                {
                    BoxCore.RunBBCommand(pm, $"area set name \"{Art.Name}\" itemid {Art.ID} hue {Art.Hue} where \"{st.GetType().Name}\"");
                }

                if (targeted is Item i)
                {
                    BoxCore.RunBBCommand(pm, $"area set name \"{Art.Name}\" itemid {Art.ID} hue {Art.Hue} where \"{i.GetType().Name}\"");
                }

                pm.SendMessage(52, $"Replace Art with {Art.ID}");
            }
        }
    }
}

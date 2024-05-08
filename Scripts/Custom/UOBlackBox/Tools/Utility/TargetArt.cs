using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Services.UOBlackBox.Tools
{
    internal class TargetArt : Target
    {
        private readonly MoveTool Tool;

        public TargetArt(MoveTool tool) : base(100, false, TargetFlags.None)
        {
            Tool = tool;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (from is PlayerMobile pm)
            {
                if (targeted is Item i)
                {
                    Tool.MoveHanle = i;

                    pm.SendMessage(52, "Item now linked!");

                    return;
                }

                if (targeted is Static DStatic)
                {
                    Tool.MoveHanle = DStatic;

                    pm.SendMessage(52, "Static now linked!");

                    return;
                }

                if (targeted is PlayerMobile tpm && tpm == pm)
                {
                    Tool.MoveHanle = tpm;

                    tpm.SendMessage(52, "You are now linked!");
                }
            }
        }
    }
}

using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System;

namespace Server.Custom.RegionBanner
{
    internal class RegionBannerGump : BaseGump
    {
        private string _RegionName;

        private Timer _AutoCloseTimer;

        public RegionBannerGump(PlayerMobile user, string name) : base(user, 50, 50, null)
        {
            _RegionName = name;

            if (string.IsNullOrEmpty(_RegionName))
            {
                _RegionName = $"{user.Map.Name} Wilds";
            }
        }

        public override void AddGumpLayout()
        {
            Closable = false;
            Resizable = false;
            Dragable = true;

            AddBackground(X, Y, 200, 100, 39925);

            AddAlphaRegion(X + 20, Y + 20, 160, 60);

            AddBackground(X + 15, Y + 15, 170, 70, 40000);

            AddLabel(X + CalculateOffset($"Entering"), Y + 27, 43, $"Entering");

            AddLabel(X + CalculateOffset($"{_RegionName}"), Y + 52, 53, $"{_RegionName}");

            _AutoCloseTimer = Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
            {
                Close();
            });
        }

        public override void Close()
        {
            StopAutoClose();

            base.Close();
        }

        public override void OnServerClose(NetState owner)
        {
            StopAutoClose();

            base.OnServerClose(owner);
        }

        private void StopAutoClose()
        {
            if (_AutoCloseTimer != null && _AutoCloseTimer.Running)
            {
                _AutoCloseTimer.Stop();

                _AutoCloseTimer = null;
            }
        }

        private int CalculateOffset(string text)
        {
            int textWidth = text.Length * 6;

            return (200 - textWidth) / 2;
        }
    }
}

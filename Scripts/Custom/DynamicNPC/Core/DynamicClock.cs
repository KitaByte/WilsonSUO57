using System;

namespace Server.Services.DynamicNPC
{
	internal class DynamicClock : Timer
	{
		public delegate void OnTickEventHandler();

		public event OnTickEventHandler OnTickClock;

		public DynamicClock() : base(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1))
		{
			Start();

			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole("Main Clock Started...", true);
			}
		}

		protected override void OnTick()
		{
			OnTickClock?.Invoke();
		}
	}
}

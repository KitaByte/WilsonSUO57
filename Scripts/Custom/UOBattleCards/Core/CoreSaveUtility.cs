namespace Server.Services.UOBattleCards.Core
{
	public class CoreSaveUtility
    {
		public static void EventSink_BeforeWorldSave(BeforeWorldSaveEventArgs e)
        {
            if (CoreUtility.GameTimer != null && CoreUtility.GameTimer.Running)
            {
                CoreUtility.StopTimer();
            }
        }

		public static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            CreatureUtility.SaveCreatureInfo();

            PlayerUtility.SaveGameInfo();

            if (CoreUtility.GameTimer == null || !CoreUtility.GameTimer.Running)
            {
                CoreUtility.StartTimer();
            }
        }

		public static void EventSink_Crashed(CrashedEventArgs e)
        {
            if (CoreUtility.GameTimer != null && CoreUtility.GameTimer.Running)
            {
                CoreUtility.StopTimer();
            }

            CreatureUtility.SaveCreatureInfo();

            PlayerUtility.SaveGameInfo();
        }

		public static void EventSink_Shutdown(ShutdownEventArgs e)
        {
            if (CoreUtility.GameTimer != null && CoreUtility.GameTimer.Running)
            {
                CoreUtility.StopTimer();
            }

            CreatureUtility.SaveCreatureInfo();

            PlayerUtility.SaveGameInfo();
        }
    }
}

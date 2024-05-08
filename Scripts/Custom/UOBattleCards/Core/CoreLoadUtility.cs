namespace Server.Services.UOBattleCards.Core
{
	public class CoreLoadUtility
    {
		public static void EventSink_ServerStarted()
        {
            CreatureUtility.LoadCreatureInfo();

            PlayerUtility.LoadGameInfo();

            if (CoreUtility.GameTimer == null || !CoreUtility.GameTimer.Running)
            {
                CoreUtility.StartTimer();
            }
        }
    }
}

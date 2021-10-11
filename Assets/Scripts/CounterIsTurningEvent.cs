public class CounterIsTurningEvent : GameEvent
{
	public bool isTurningTowardPlayers;

	public CounterIsTurningEvent(bool turningTowardPlayers)
	{
		isTurningTowardPlayers = turningTowardPlayers;
	}
}

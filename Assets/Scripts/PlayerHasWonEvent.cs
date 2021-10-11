public class PlayerHasWonEvent : GameEvent
{
	public int playerId;

	public PlayerHasWonEvent(int playerId)
	{
		this.playerId = playerId;
	}
}

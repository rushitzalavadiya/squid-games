public class PlayerLostEvent : GameEvent
{
	public int playerId;

	public string playerName;

	public bool isByCounter;

	public PlayerLostEvent(int playerId, string playerName, bool isByCounter)
	{
		this.playerId = playerId;
		this.playerName = playerName;
		this.isByCounter = isByCounter;
	}
}

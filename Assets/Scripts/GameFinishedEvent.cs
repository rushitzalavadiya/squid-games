using UnityEngine;

public class GameFinishedEvent : GameEvent
{
	public bool isGameOver;

	public GameFinishedEvent(bool gameOver, Transform winningPlayerTransform = null)
	{
		isGameOver = gameOver;
	}
}

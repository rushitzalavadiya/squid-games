using System;

public class PlayerScore : IComparable<PlayerScore>
{
	public Character characterScript;

	public float distance;

	public PlayerScore(Character characterScript, float distance)
	{
		this.characterScript = characterScript;
		this.distance = distance;
	}

	public int CompareTo(PlayerScore other)
	{
		if (other == null)
		{
			return 1;
		}
		return distance.CompareTo(other.distance);
	}
}

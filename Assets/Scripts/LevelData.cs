using System;

[Serializable]
public class LevelData
{
	public int colorId = -1;

	public string firstPlatformId;

	public string secondPlatformId;

	public string thirdPlatformId;

	public string fourthPlatformId;

	public bool firstPlatformShouldBeReversed;

	public bool secondPlatformShouldBeReversed;

	public bool thirdPlatformShouldBeReversed;

	public bool fourthPlatformShouldBeReversed;
}

public class ScreenTouchedEvent : GameEvent
{
	public bool isTouched;

	public ScreenTouchedEvent(bool touched)
	{
		isTouched = touched;
	}
}

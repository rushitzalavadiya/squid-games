using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenTouchManager : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	private EventManager eventManager;

	private void Start()
	{
		eventManager = EventManager.Instance;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		eventManager.QueueEvent(new ScreenTouchedEvent(touched: true));
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		eventManager.QueueEvent(new ScreenTouchedEvent(touched: false));
	}
}

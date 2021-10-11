using UnityEngine;
using UnityEngine.EventSystems;

namespace HSVPickerDemo
{
	public class HsvBoxSelector : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		public HSVDragger dragger;

		private RectTransform rectTransform;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		private void Update()
		{
		}

		private void PlaceCursor(PointerEventData eventData)
		{
			UnityEngine.Debug.Log(eventData.position + " " + rectTransform.position);
		}

		public void OnDrag(PointerEventData eventData)
		{
			PlaceCursor(eventData);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			PlaceCursor(eventData);
		}
	}
}

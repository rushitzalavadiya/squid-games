// using UnityEngine;
// using Voodoo.Sauce.Internal.GDPR;
// using Voodoo.Sauce.Internal.GDPR.UI;
//
// public class AdTrackingConditionalUI : MonoBehaviour
// {
// 	[SerializeField]
// 	private GameObject[] _objectsDisabledIfAdTracking;
//
// 	[SerializeField]
// 	private GameObject[] _objectsDisabledIfNotAdTracking;
//
// 	[SerializeField]
// 	private SimpleCheckbox[] _checkboxesLockedIfAdTracking;
//
// 	private void Start()
// 	{
// 		if (VoodooGDPR.HasLimitAdTrackingEnabled())
// 		{
// 			GameObject[] objectsDisabledIfAdTracking = _objectsDisabledIfAdTracking;
// 			for (int i = 0; i < objectsDisabledIfAdTracking.Length; i++)
// 			{
// 				objectsDisabledIfAdTracking[i].SetActive(value: false);
// 			}
// 			SimpleCheckbox[] checkboxesLockedIfAdTracking = _checkboxesLockedIfAdTracking;
// 			for (int i = 0; i < checkboxesLockedIfAdTracking.Length; i++)
// 			{
// 				checkboxesLockedIfAdTracking[i].Lock(value: false);
// 			}
// 		}
// 		else
// 		{
// 			GameObject[] objectsDisabledIfAdTracking = _objectsDisabledIfNotAdTracking;
// 			for (int i = 0; i < objectsDisabledIfAdTracking.Length; i++)
// 			{
// 				objectsDisabledIfAdTracking[i].SetActive(value: false);
// 			}
// 		}
// 	}
// }

// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;
// using Voodoo.Sauce.Internal.GDPR;
// using Voodoo.Sauce.Internal.GDPR.UI;
//
// public class VoodooGDPRPrivacyScreen : MonoBehaviour
// {
// 	[SerializeField]
// 	private SimpleCheckbox[] _checkboxesRequiredForPlaying;
//
// 	[SerializeField]
// 	private SimpleCheckbox[] _checkboxesRequiredForPlayingAfterInfoScreenSeen;
//
// 	[SerializeField]
// 	private SimpleCheckbox[] _checkboxesRequiredForLearnMore;
//
// 	[SerializeField]
// 	private SimpleCheckbox[] _checkboxesRequiredForPlayingWithoutAdTracking;
//
// 	[SerializeField]
// 	private Button _playBtn;
//
// 	[SerializeField]
// 	private Button _learnMoreBtn;
//
// 	private bool _infoPageHasBeenSeen;
//
// 	private void Start()
// 	{
// 		SimpleCheckbox[] checkboxesRequiredForLearnMore = _checkboxesRequiredForLearnMore;
// 		for (int i = 0; i < checkboxesRequiredForLearnMore.Length; i++)
// 		{
// 			checkboxesRequiredForLearnMore[i].OnToggle.AddListener(delegate
// 			{
// 				UpdateButtonsAvailability();
// 			});
// 		}
// 		checkboxesRequiredForLearnMore = _checkboxesRequiredForPlaying;
// 		for (int i = 0; i < checkboxesRequiredForLearnMore.Length; i++)
// 		{
// 			checkboxesRequiredForLearnMore[i].OnToggle.AddListener(delegate
// 			{
// 				UpdateButtonsAvailability();
// 			});
// 		}
// 		UpdateButtonsAvailability();
// 	}
//
// 	private void UpdateButtonsAvailability()
// 	{
// 		if (VoodooGDPR.HasLimitAdTrackingEnabled())
// 		{
// 			_playBtn.interactable = _checkboxesRequiredForPlayingWithoutAdTracking.All((SimpleCheckbox box) => box.IsChecked());
// 		}
// 		else if (_infoPageHasBeenSeen)
// 		{
// 			_playBtn.interactable = _checkboxesRequiredForPlayingAfterInfoScreenSeen.All((SimpleCheckbox box) => box.IsChecked());
// 			_learnMoreBtn.interactable = _checkboxesRequiredForLearnMore.All((SimpleCheckbox box) => box.IsChecked());
// 		}
// 		else
// 		{
// 			_playBtn.interactable = _checkboxesRequiredForPlaying.All((SimpleCheckbox box) => box.IsChecked());
// 			_learnMoreBtn.interactable = _checkboxesRequiredForLearnMore.All((SimpleCheckbox box) => box.IsChecked());
// 		}
// 	}
//
// 	public void ThingsToDoWhenInfoIsShown()
// 	{
// 		_infoPageHasBeenSeen = true;
// 		SimpleCheckbox[] checkboxesRequiredForPlaying = _checkboxesRequiredForPlaying;
// 		for (int i = 0; i < checkboxesRequiredForPlaying.Length; i++)
// 		{
// 			checkboxesRequiredForPlaying[i].SetChecked(value: false);
// 		}
// 	}
// }


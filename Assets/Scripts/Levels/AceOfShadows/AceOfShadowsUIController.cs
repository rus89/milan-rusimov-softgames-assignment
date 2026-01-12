using Cysharp.Threading.Tasks;
using Levels.AceOfShadows;
using PrimeTween;
using Softgames.Core.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Softgames.Levels.AceOfShadows
{
	public class AceOfShadowsUIController : MonoBehaviour
	{
		[SerializeField] private AceOfShadowsController _aceOfShadowsController;
		
		[Header("Buttons")]
		[SerializeField] private Button _backToMenuButton;
		
		[Header("Counters")]
		[SerializeField] private TMP_Text _counterA;
		[SerializeField] private TMP_Text _counterB;
		[SerializeField] private Slider _counterAFill;
		[SerializeField] private Slider _counterBFill;
		[SerializeField] private TMP_Text _messageText;
		
		private ISceneLoaderService _sceneLoaderService;
		private IAudioService _audioService;
		
		//-----------------------------------------------------------------------
		private void Awake()
		{
			_sceneLoaderService = ServiceLocator.GetService<ISceneLoaderService>();
			_audioService = ServiceLocator.GetService<IAudioService>();
			RegisterButtonCallbacks();
			RegisterEventListeners();
		}

		//-----------------------------------------------------------------------
		private void OnDestroy()
		{
			UnregisterButtonCallbacks();
			UnregisterEventListeners();
		}

		//-----------------------------------------------------------------------
		private void RegisterButtonCallbacks()
		{
			_backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
		}

		//-----------------------------------------------------------------------
		private void RegisterEventListeners()
		{
			if (_aceOfShadowsController != null)
			{
				_aceOfShadowsController.OnStacksUpdated += UpdateCounters;
				_aceOfShadowsController.OnGameStarted += OnGameStarted;
				_aceOfShadowsController.OnGameFinished += OnGameFinished;
			}
		}

		//-----------------------------------------------------------------------
		private void UnregisterButtonCallbacks()
		{
			_backToMenuButton.onClick.RemoveAllListeners();
		}

		//-----------------------------------------------------------------------
		private void UnregisterEventListeners()
		{
			if (_aceOfShadowsController != null)
			{
				_aceOfShadowsController.OnStacksUpdated -= UpdateCounters;
				_aceOfShadowsController.OnGameStarted -= OnGameStarted;
				_aceOfShadowsController.OnGameFinished -= OnGameFinished;
			}
		}

		//-----------------------------------------------------------------------
		private void OnBackToMenuButtonClicked()
		{
			_audioService.PlaySFX("buttonClick");
			_sceneLoaderService.LoadSceneAsync("MainMenu").Forget();
		}
		
		//-----------------------------------------------------------------------
		private void UpdateCounters(int countA, int countB)
		{
			_counterAFill.value = countA;
			_counterBFill.value = countB;
			_counterA.text = $"{countA}/144";
			_counterB.text = $"{countB}/144";
		}
		
		//-----------------------------------------------------------------------
		private void OnGameStarted()
		{
			_messageText.text = "";
		}
		
		//-----------------------------------------------------------------------
		private void OnGameFinished()
		{
			_messageText.text = "All cards moved!";
			Tween.Alpha(_messageText, 1f, 0.5f);
			Tween.PunchScale(_messageText.transform, Vector3.one * 0.2f, 0.5f);
		}
	}
}

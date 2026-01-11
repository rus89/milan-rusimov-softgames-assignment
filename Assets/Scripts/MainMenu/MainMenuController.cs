using Cysharp.Threading.Tasks;
using Softgames.Core;
using Softgames.Core.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Softgames.MainMenu
{
	public class MainMenuController : MonoBehaviour
	{
		[Header("Level Buttons")]
		[SerializeField] private Button _task1Button;
		[SerializeField] private Button _task2Button;
		[SerializeField] private Button _task3Button;
		
		private ISceneLoaderService _sceneLoaderService;

		//-----------------------------------------------------------------------
		private void Awake()
		{
			RegisterButtonListeners();
		}

		//-----------------------------------------------------------------------
		private void Start()
		{
			_sceneLoaderService = ServiceLocator.GetService<ISceneLoaderService>();
		}

		//-----------------------------------------------------------------------
		private void OnDestroy()
		{
			_task1Button.onClick.RemoveAllListeners();
			_task2Button.onClick.RemoveAllListeners();
			_task3Button.onClick.RemoveAllListeners();
		}

		//-----------------------------------------------------------------------
		private void RegisterButtonListeners()
		{
			_task1Button.onClick.AddListener(() => LoadScene("AceOfShadows"));
			_task2Button.onClick.AddListener(() => LoadScene("MagicWords"));
			_task3Button.onClick.AddListener(() => LoadScene("PhoenixFlame"));
		}

		//-----------------------------------------------------------------------
		private void LoadScene(string sceneName)
		{
			_task1Button.interactable = false;
			_task2Button.interactable = false;
			_task3Button.interactable = false;
			
			_sceneLoaderService.LoadSceneAsync(sceneName).Forget();
		}
	}
}

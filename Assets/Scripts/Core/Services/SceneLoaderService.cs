using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Softgames.Core.Services
{
	public class SceneLoaderService : ISceneLoaderService
	{
		private CanvasGroup _fadeOverlayCanvasGroup;
		private const float FADE_DURATION = 0.5f;
		
		//-----------------------------------------------------------------------
		public async UniTask InitializeAsync()
		{
			await UniTask.CompletedTask;
		}
		
		//-----------------------------------------------------------------------
		public void SetFadeOverlayCanvasGroup(CanvasGroup canvasGroup)
		{
			_fadeOverlayCanvasGroup = canvasGroup;
			if (_fadeOverlayCanvasGroup != null)
			{
				_fadeOverlayCanvasGroup.alpha = 0f;
				_fadeOverlayCanvasGroup.blocksRaycasts = false;
			}
		}

		//-----------------------------------------------------------------------
		public async UniTask LoadSceneAsync(string sceneName, bool additive = false)
		{
			await FadeIn();
			var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
			await SceneManager.LoadSceneAsync(sceneName, mode).ToUniTask();
			await FadeOut();
		}

		//-----------------------------------------------------------------------
		private async UniTask FadeIn()
		{
			_fadeOverlayCanvasGroup.blocksRaycasts = true;
			await Tween.Alpha(_fadeOverlayCanvasGroup, 0f, 1f, FADE_DURATION)
			           .ToYieldInstruction()
			           .ToUniTask();
		}
		
		//-----------------------------------------------------------------------
		private async UniTask FadeOut()
		{
			await Tween.Alpha(_fadeOverlayCanvasGroup, 1f, 0f, FADE_DURATION)
			           .ToYieldInstruction()
			           .ToUniTask();
			_fadeOverlayCanvasGroup.blocksRaycasts = false;
		}
	}
}
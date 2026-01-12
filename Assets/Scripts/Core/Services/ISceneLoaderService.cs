using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Softgames.Core.Services
{
	public interface ISceneLoaderService
	{
		 public UniTask LoadSceneAsync(string sceneName, bool additive = false);
		 public void SetFadeOverlayCanvasGroup(CanvasGroup canvasGroup);
	}
}
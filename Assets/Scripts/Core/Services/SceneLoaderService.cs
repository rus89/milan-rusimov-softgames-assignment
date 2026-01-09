using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Softgames.Core.Services
{
	public class SceneLoaderService : ISceneLoaderService
	{
		//-----------------------------------------------------------------------
		public async UniTask InitializeAsync()
		{
			await UniTask.CompletedTask;
		}

		//-----------------------------------------------------------------------
		public async UniTask LoadSceneAsync(string sceneName, bool additive = false)
		{
			var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
			await SceneManager.LoadSceneAsync(sceneName, mode).ToUniTask();
		}
	}
}
using Cysharp.Threading.Tasks;

namespace Softgames.Core.Services
{
	public interface ISceneLoaderService : IGameService
	{
		UniTask LoadSceneAsync(string sceneName, bool additive = false);
	}
}
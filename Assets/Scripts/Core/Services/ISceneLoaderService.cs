using Cysharp.Threading.Tasks;

namespace Softgames.Core.Services
{
	public interface ISceneLoaderService
	{
		 public UniTask LoadSceneAsync(string sceneName, bool additive = false);
	}
}
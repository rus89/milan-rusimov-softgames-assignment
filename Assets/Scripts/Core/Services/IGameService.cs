using Cysharp.Threading.Tasks;

namespace Softgames.Core.Services
{
	public interface IGameService
	{
		public UniTask InitializeAsync();
	}
}

using Cysharp.Threading.Tasks;
using Softgames.Levels.MagicWords.Data;

namespace Softgames.Core.Services
{
	public interface IMagicWordsService
	{
		UniTask<MagicWordsData[]> GetMagicWordsAsync();
	}
}
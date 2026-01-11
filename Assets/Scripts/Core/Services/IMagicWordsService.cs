using Cysharp.Threading.Tasks;
using Softgames.Levels.MagicWords.Data;
using UnityEngine;

namespace Softgames.Core.Services
{
	public interface IMagicWordsService
	{
		public UniTask<ChatDisplayData[]> GetMagicWordsAsync();
		public UniTask<Texture2D> GetAvatarTextureAsync(string url);
	}
}
using Cysharp.Threading.Tasks;

namespace Softgames.Core.Services
{
	public interface IAudioService
	{
		public UniTask InitializeAsync();
		public void PlayMusic(string id);
		public void PlaySFX(string id);
		public void SetVolumes(float musicVol, float sfxVol);
	}
}
using Cysharp.Threading.Tasks;
using Softgames.Core.Audio;
using UnityEngine;

namespace Softgames.Core.Services
{
	public class AudioService : IAudioService
	{
		private AudioSource _musicSource;
		private AudioSource _sfxSource;
		private AudioDatabase _musicDatabase;
		private AudioDatabase _sfxDatabase;
		
		//-----------------------------------------------------------------------
		public UniTask InitializeAsync()
		{
			return UniTask.CompletedTask;
		}

		//-----------------------------------------------------------------------
		public void SetUp(AudioSource audioProviderMusicSource, AudioSource audioProviderSfxSource, AudioDatabase audioProviderMusicDatabase, AudioDatabase audioProviderSfxDatabase)
		{
			_musicSource = audioProviderMusicSource;
			_sfxSource = audioProviderSfxSource;
			_musicDatabase = audioProviderMusicDatabase;
			_sfxDatabase = audioProviderSfxDatabase;
			
			_musicSource.playOnAwake = false;
			_sfxSource.playOnAwake = false;
			SetVolumes(0.5f, 1f);
		}

		//-----------------------------------------------------------------------
		public void PlayMusic(string id)
		{
			var clip = _musicDatabase.GetAudioClip(id);
			if (clip == null || _musicSource.clip == clip)
			{
				return;
			}

			_musicSource.clip = clip;
			_musicSource.loop = true;
			_musicSource.Play();
		}
		
		//-----------------------------------------------------------------------
		public void PlaySFX(string id)
		{
			var clip = _sfxDatabase.GetAudioClip(id);
			if (clip != null)
			{
				_sfxSource.PlayOneShot(clip);
			}
		}

		//-----------------------------------------------------------------------
		public void SetVolumes(float musicVol, float sfxVol)
		{
			_musicSource.volume = musicVol;
			_sfxSource.volume = sfxVol;
		}
	}
}
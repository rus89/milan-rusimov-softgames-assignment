using System;
using System.Collections.Generic;
using UnityEngine;

namespace Softgames.Core.Audio
{
	[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Softgames/AudioDatabase")]
	public class AudioDatabase : ScriptableObject
	{
		[Serializable]
		public class AudioEntry
		{
			public string Key;
			public AudioClip Clip;
		}

		public List<AudioEntry> AudioEntries;
		
		//-----------------------------------------------------------------------
		public AudioClip GetAudioClip(string key)
		{
			var entry = AudioEntries.Find(e => e.Key == key);
			return entry?.Clip;
		}
	}
}

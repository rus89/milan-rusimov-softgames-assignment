using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Softgames.Levels.MagicWords.Data;
using Softgames.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace Softgames.Core.Services
{
	public class MagicWordsService : IMagicWordsService
	{
		private const string API_URL = "https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords";
		private readonly Dictionary<string, Texture2D> _avatarTextures = new();
		
		//-----------------------------------------------------------------------
		public UniTask InitializeAsync()
		{
			return UniTask.CompletedTask;
		}
		
		//-----------------------------------------------------------------------
		public async UniTask<ChatDisplayData[]> GetMagicWordsAsync()
		{
			using var request = UnityWebRequest.Get(API_URL);
			try
			{
				await request.SendWebRequest().ToUniTask();

				if (request.result != UnityWebRequest.Result.Success)
				{
					Logging.LogError($"API Error: {request.error}");
					return Array.Empty<ChatDisplayData>();
				}

				string json = request.downloadHandler.text;
				var response = JsonUtility.FromJson<MagicWordsResponse>(json);

				if (response?.dialogue == null)
				{
					Logging.LogWarning("API returned valid JSON but missing 'dialogues' array.");
					return Array.Empty<ChatDisplayData>();
				}

				var avatarMap = new Dictionary<string, AvatarData>();
				if (response.avatars != null)
				{
					foreach (var avatarEntry in response.avatars)
					{
						bool isBrokenUrl = avatarEntry.url.Contains("timeout");
    
						if (isBrokenUrl)
						{
							continue;
						}
						
						avatarMap[avatarEntry.name] = avatarEntry;
					}
				}

				var resultList = new List<ChatDisplayData>();

				foreach (var line in response.dialogue)
				{
					string url = "";
					bool isRight = false;

					if (avatarMap.TryGetValue(line.name, out var avatarInfo))
					{
						url = avatarInfo.url;
						isRight = avatarInfo.position == "right";
					}

					resultList.Add(new ChatDisplayData
					{
						CharacterName = line.name,
						Text = line.text,
						AvatarUrl = url,
						IsRightAligned = isRight
					});
				}

				return resultList.ToArray();
			}
			catch (Exception e)
			{
				Logging.LogException(e);
				return Array.Empty<ChatDisplayData>();
			}
		}
		
		//-----------------------------------------------------------------------
		public async UniTask<Texture2D> GetAvatarTextureAsync(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				Logging.LogWarning("Avatar URL is null or empty.");
				return null;
			}
			
			if (_avatarTextures.TryGetValue(url, out var cachedTexture))
			{
				return cachedTexture;
			}
			
			using var request = UnityWebRequestTexture.GetTexture(url);
			try
			{
				await request.SendWebRequest();
            
				if (request.result == UnityWebRequest.Result.Success)
				{
					var texture = DownloadHandlerTexture.GetContent(request);
					_avatarTextures[url] = texture;
					return texture;
				}

				Logging.LogError($"Failed to load avatar: {url}");
				return null;
			}
			catch
			{
				Logging.LogError($"Failed to load avatar: {url}");
				return null;
			}
		}
	}
}
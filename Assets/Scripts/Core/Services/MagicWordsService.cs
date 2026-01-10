using System;
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
		
		//-----------------------------------------------------------------------
		public UniTask InitializeAsync()
		{
			return UniTask.CompletedTask;
		}
		
		//-----------------------------------------------------------------------
		public async UniTask<Levels.MagicWords.Data.MagicWordsData[]> GetMagicWordsAsync()
		{
			using var request = UnityWebRequest.Get(API_URL);
			try
			{
				await request.SendWebRequest();

				if (request.result != UnityWebRequest.Result.Success)
				{
					Logging.LogError($"API Error: {request.error}");
					return Array.Empty<MagicWordsData>();
				}

				string json = request.downloadHandler.text;
				var response = JsonUtility.FromJson<MagicWordsResponse>(json);
				
				if (response?.data == null)
				{
					Logging.LogWarning("API returned valid JSON but empty structure.");
					return Array.Empty<MagicWordsData>();
				}

				return response.data;
			}
			catch (Exception e)
			{
				Logging.LogException(e);
				return Array.Empty<MagicWordsData>();
			}
		}
	}
}
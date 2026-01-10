using System;

namespace Softgames.Levels.MagicWords.Data
{
	[Serializable]
	public class MagicWordsResponse
	{
		public MagicWordsData[] data;
	}
	
	[Serializable]
	public class MagicWordsData
	{
		public string character;
		public string text;
		public string avatar;
	}
}

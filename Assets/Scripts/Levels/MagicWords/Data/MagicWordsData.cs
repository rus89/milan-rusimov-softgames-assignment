using System;

namespace Softgames.Levels.MagicWords.Data
{
	[Serializable]
	public class MagicWordsResponse
	{
		public DialogueData[] dialogue;
		public AvatarData[] avatars;
	}
	
	[Serializable]
	public class DialogueData
	{
		public string name;
		public string text;
	}
	
	[Serializable]
	public class AvatarData
	{
		public string name;
		public string url;
		public string position;
	}
	
	public class ChatDisplayData
	{
		public string CharacterName;
		public string Text;
		public string AvatarUrl;
		public bool IsRightAligned;
	}
}

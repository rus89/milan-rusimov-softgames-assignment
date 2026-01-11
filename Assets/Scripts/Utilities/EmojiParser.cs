using System.Collections.Generic;
using System.Text;

namespace Softgames.Utilities
{
	public static class EmojiParser
	{
		private static readonly Dictionary<string, string> EmojiMap = new()
		{
			{ "{satisfied}",   "\uD83D\uDE0C" },
			{ "{intrigued}",   "\uD83E\uDD14" },
			{ "{neutral}",     "\uD83D\uDE10" },
			{ "{affirmative}", "\uD83D\uDC4D" },
			{ "{laughing}",    "\uD83D\uDE02" },
			{ "{win}",         "\uD83C\uDFC6" },
			{ "{happy}",       "\uD83D\uDE04" }
		};
		
		//-----------------------------------------------------------------------
		public static string ParseEmotions(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}

			StringBuilder emojiReplacedString = new StringBuilder(input);
			foreach (var emojiPair in EmojiMap)
			{
				emojiReplacedString.Replace(emojiPair.Key, emojiPair.Value);
			}

			return emojiReplacedString.ToString();
		}
	}
}

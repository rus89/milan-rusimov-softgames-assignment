using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Softgames.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Softgames.Levels.MagicWords
{
	public class ChatBubbleView : MonoBehaviour
	{
		[Header("UI References")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private RawImage _avatarImage;
        [SerializeField] private RectTransform _bubbleContainer;

        [Header("Styling")]
        [SerializeField] private Color _colorLeft = new Color(0.9f, 0.9f, 0.9f);
        [SerializeField] private Color _colorRight = new Color(0.8f, 1f, 0.8f);
        
        private CancellationTokenSource _cts;

        //-----------------------------------------------------------------------
        public void Configure(string characterName, string message, string avatarUrl, bool isMe)
        {
            _nameText.text = characterName;
            _messageText.text = message;

            var bgImage = _bubbleContainer.GetComponent<Image>();
            if (bgImage)
            {
                bgImage.color = isMe ? _colorRight : _colorLeft;
            }
            
            LoadAvatarAsync(avatarUrl).Forget();
        }

        //-----------------------------------------------------------------------
        private async UniTaskVoid LoadAvatarAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Logging.LogWarning("Avatar URL is null or empty.");
                return;
            }
            
            var token = this.GetCancellationTokenOnDestroy();
            using var request = UnityWebRequestTexture.GetTexture(url);
            try
            {
                await request.SendWebRequest().ToUniTask(cancellationToken: token);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var texture = DownloadHandlerTexture.GetContent(request);
                    _avatarImage.texture = texture;
                    _avatarImage.color = Color.white;
                }
                else
                {
                    Logging.LogWarning($"Failed to load avatar: {url}");
                }
            }
            catch (OperationCanceledException)
            {
                // Object destroyed during load. Ignore.
            }
            catch
            {
                // Network error. Ignore.
            }
        }
    }
}

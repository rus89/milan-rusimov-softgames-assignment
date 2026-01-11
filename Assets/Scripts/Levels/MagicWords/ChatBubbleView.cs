using Cysharp.Threading.Tasks;
using Softgames.Utilities;
using TMPro;
using UnityEngine;
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
        [SerializeField] private Color _colorLeft = new(0.9f, 0.9f, 0.9f);
        [SerializeField] private Color _colorRight = new(0.8f, 1f, 0.8f);

        //-----------------------------------------------------------------------
        public void Configure(string characterName, string message, UniTask<Texture2D> avatarTask, bool isRightAligned)
        {
            _nameText.text = characterName;
            _messageText.text = message;

            var bgImage = _bubbleContainer.GetComponent<Image>();
            if (bgImage)
            {
                bgImage.color = isRightAligned ? _colorRight : _colorLeft;
            }
            
            LoadAvatarFromTask(avatarTask).Forget();
        }
        
        //-----------------------------------------------------------------------
        private async UniTaskVoid LoadAvatarFromTask(UniTask<Texture2D> task)
        {
            var token = this.GetCancellationTokenOnDestroy();

            try
            {
                var texture = await task.AttachExternalCancellation(token);
        
                if (texture != null)
                {
                    _avatarImage.texture = texture;
                    _avatarImage.color = Color.white;
                }
            }
            catch
            {
                Logging.LogWarning("Failed to load avatar texture.");
            }
        }
    }
}

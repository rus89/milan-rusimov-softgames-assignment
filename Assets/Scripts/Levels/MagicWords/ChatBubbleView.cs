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
        [SerializeField] private Image _bubbleContainer;
        
        [Header("Layout")]
        [SerializeField] private RectTransform _avatarContainer;
        [SerializeField] private RectTransform _messageContainer;
        [SerializeField] private HorizontalLayoutGroup _rootLayout;

        [Header("Styling")]
        [SerializeField] private Color _colorLeft = new(0.9f, 0.9f, 0.9f);
        [SerializeField] private Color _colorRight = new(0.8f, 1f, 0.8f);

        //-----------------------------------------------------------------------
        public void Configure(string characterName, string message, UniTask<Texture2D> avatarTask, bool isRightAligned)
        {
            _nameText.text = characterName;
            _messageText.text = message;

            _bubbleContainer.color = isRightAligned ? _colorRight : _colorLeft;
            
            if (isRightAligned)
            {
                _rootLayout.childAlignment = TextAnchor.UpperRight;
                _messageText.alignment = TextAlignmentOptions.MidlineRight;
                _nameText.alignment = TextAlignmentOptions.Right;
                _avatarContainer.SetAsLastSibling();
            }
            else
            {
                _rootLayout.childAlignment = TextAnchor.UpperLeft;
                _messageText.alignment = TextAlignmentOptions.MidlineLeft;
                _nameText.alignment = TextAlignmentOptions.Left;
                _avatarContainer.SetAsFirstSibling();
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

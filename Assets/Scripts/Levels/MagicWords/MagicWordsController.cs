using Cysharp.Threading.Tasks;
using Softgames.Core;
using Softgames.Core.Services;
using Softgames.Levels.MagicWords.Data;
using Softgames.Utilities;
using UnityEngine;

namespace Softgames.Levels.MagicWords
{
    public class MagicWordsController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private ChatBubbleView _bubblePrefab;
        [SerializeField] private Transform _contentContainer;
        [SerializeField] private GameObject _loadingSpinner;
        [SerializeField] private GameObject _errorPanel;

        private IMagicWordsService _dataService;

        //-----------------------------------------------------------------------
        private void Start()
        {
            _dataService = ServiceLocator.GetService<IMagicWordsService>();
            LoadChatSequence().Forget();
        }

        //-----------------------------------------------------------------------
        private async UniTaskVoid LoadChatSequence()
        {
            _loadingSpinner.SetActive(true);
            _errorPanel.SetActive(false);
            ClearChat();
            
            var chatDisplayData = await _dataService.GetMagicWordsAsync();
            _loadingSpinner.SetActive(false);

            if (chatDisplayData == null || chatDisplayData.Length == 0)
            {
                _errorPanel.SetActive(true);
                return;
            }
            
            foreach (var item in chatDisplayData)
            {
                CreateBubble(item);
                await UniTask.Delay(100, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }

        //-----------------------------------------------------------------------
        private void CreateBubble(ChatDisplayData data)
        {
            if (data == null)
            {
                Logging.LogWarning("Received null data for chat bubble.");
                return;
            }

            string characterName = string.IsNullOrEmpty(data.CharacterName) ? "Unknown" : data.CharacterName;
            string text = string.IsNullOrEmpty(data.Text) ? "..." : data.Text;

            var bubbleObj = Instantiate(_bubblePrefab, _contentContainer);
            var parsedText = EmojiParser.ParseEmotions(text);
            var imageTask = _dataService.GetAvatarTextureAsync(data.AvatarUrl);
            bubbleObj.Configure(characterName, parsedText, imageTask, data.IsRightAligned);
        }

        //-----------------------------------------------------------------------
        private void ClearChat()
        {
            foreach (Transform child in _contentContainer)
            {
                Destroy(child.gameObject);
            }
        }
        
        //-----------------------------------------------------------------------
        public void OnRetryClicked()
        {
            LoadChatSequence().Forget();
        }
    }
}

using Cysharp.Threading.Tasks;
using Softgames.Core.Services;
using Softgames.Levels.MagicWords.Data;
using Softgames.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Softgames.Levels.MagicWords
{
    public class MagicWordsController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private ChatBubbleView _bubblePrefab;
        [SerializeField] private Transform _contentContainer;
        [SerializeField] private GameObject _loadingSpinner;
        [SerializeField] private GameObject _errorPanel;
        [SerializeField] private Button _backButton;

        private Button _retryButton;
        
        private IMagicWordsService _dataService;
        private ISceneLoaderService _sceneLoaderService;
        private IAudioService _audioService;
        
        //-----------------------------------------------------------------------
        private void Awake()
        {
            _backButton.onClick.AddListener(LoadMainMenuAsync);
            _retryButton = _errorPanel.GetComponentInChildren<Button>(true);
            if (_retryButton != null)
            {
                _retryButton.onClick.AddListener(OnRetryClicked);
            }
        }

        //-----------------------------------------------------------------------
        private void OnDestroy()
        {
            _backButton.onClick.RemoveAllListeners();
            if (_retryButton != null)
            {
                _retryButton.onClick.RemoveAllListeners();
            }
        }

        //-----------------------------------------------------------------------
        private void Start()
        {
            _dataService = ServiceLocator.GetService<IMagicWordsService>();
            _sceneLoaderService = ServiceLocator.GetService<ISceneLoaderService>();
            _audioService = ServiceLocator.GetService<IAudioService>();
            _audioService.PlayMusic("levelMusic");
            LoadChatSequence().Forget();
        }

        //-----------------------------------------------------------------------
        private void LoadMainMenuAsync()
        {
            _audioService.PlaySFX("buttonClick");
            _sceneLoaderService.LoadSceneAsync("MainMenu").Forget();
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
            _audioService.PlaySFX("chatBubble");
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
        private void OnRetryClicked()
        {
            _audioService.PlaySFX("buttonClick");
            LoadChatSequence().Forget();
        }
    }
}

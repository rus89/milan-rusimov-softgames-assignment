using Cysharp.Threading.Tasks;
using Softgames.Core.Audio;
using Softgames.Core.Services;
using Softgames.Utilities;
using UnityEngine;

namespace Softgames.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private bool _loadMenuOnStart = true;
        [SerializeField] private GameObject _globalManagersPrefab;
        
        private CanvasGroup _fadeOverlayCanvasGroup;
        private AudioProvider _audioProvider;

        //-----------------------------------------------------------------------
        private void Start()
        {
            BootstrapGameAsync().Forget();
        }
        
        //-----------------------------------------------------------------------
        private async UniTaskVoid BootstrapGameAsync()
        {
            Logging.Log("Starting boot initialization...");

            InitializeGlobalManagers();

            var sceneLoaderService = await InitializeSceneLoaderService();
            await InitializeAudioService();
            await InitializeMagicWordsService();
            
            Logging.Log("Boot initialization completed.");
            
            if (_loadMenuOnStart)
            {
                if (sceneLoaderService != null)
                {
                    await sceneLoaderService.LoadSceneAsync("MainMenu", true);
                }
            }
        }

        //-----------------------------------------------------------------------
        private void InitializeGlobalManagers()
        {
            if (_globalManagersPrefab != null)
            {
                var globals = Instantiate(_globalManagersPrefab);
                DontDestroyOnLoad(globals);
                
                var fadeOverlay = globals.GetComponentInChildren<CanvasGroup>(true);
                if (fadeOverlay != null)
                {
                    _fadeOverlayCanvasGroup = fadeOverlay;
                }
                
                var audioProvider = globals.GetComponentInChildren<AudioProvider>(true);
                if (audioProvider != null)
                {
                    _audioProvider = audioProvider;
                }
            }
        }

        //-----------------------------------------------------------------------
        private async UniTask<SceneLoaderService> InitializeSceneLoaderService()
        {
            var sceneLoaderService = new SceneLoaderService();
            
            if (_fadeOverlayCanvasGroup != null)
            {
                sceneLoaderService.SetFadeOverlayCanvasGroup(_fadeOverlayCanvasGroup);
            }
            
            ServiceLocator.RegisterService<ISceneLoaderService>(sceneLoaderService);
            await sceneLoaderService.InitializeAsync();
            return sceneLoaderService;
        }
        
        //-----------------------------------------------------------------------
        private async UniTask InitializeAudioService()
        {
            var audioService = new AudioService();

            if (_audioProvider != null)
            {
                audioService.SetUp(_audioProvider._musicSource, _audioProvider._sfxSource, _audioProvider. _musicDatabase, _audioProvider._sfxDatabase);
            }
            
            ServiceLocator.RegisterService<IAudioService>(audioService);
            await audioService.InitializeAsync();
        }
        
        //-----------------------------------------------------------------------
        private static async UniTask InitializeMagicWordsService()
        {
            var dataService = new MagicWordsService();
            ServiceLocator.RegisterService<IMagicWordsService>(dataService);
            await dataService.InitializeAsync();
        }
    }
}

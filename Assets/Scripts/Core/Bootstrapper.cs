using Cysharp.Threading.Tasks;
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
            }
        }

        //-----------------------------------------------------------------------
        private async UniTask<SceneLoaderService> InitializeSceneLoaderService()
        {
            var sceneLoaderService = new SceneLoaderService();
            ServiceLocator.RegisterService<ISceneLoaderService>(sceneLoaderService);
            await sceneLoaderService.InitializeAsync();
            if (_fadeOverlayCanvasGroup != null)
            {
                sceneLoaderService.SetFadeOverlayCanvasGroup(_fadeOverlayCanvasGroup);
            }
            return sceneLoaderService;
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

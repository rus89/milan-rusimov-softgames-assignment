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

        //-----------------------------------------------------------------------
        private void Start()
        {
            BootstrapGameAsync().Forget();
        }
        
        //-----------------------------------------------------------------------
        private async UniTaskVoid BootstrapGameAsync()
        {
            Logging.Log("Starting boot initialization...");

            if (_globalManagersPrefab != null)
            {
                var globals = Instantiate(_globalManagersPrefab);
                DontDestroyOnLoad(globals);
            }

            var sceneService = await InitializeSceneService();
            await InitializeMagicWordsService();
            
            Logging.Log("Boot initialization completed.");
            
            if (_loadMenuOnStart)
            {
                await sceneService.LoadSceneAsync("MainMenu", true);
            }
        }

        //-----------------------------------------------------------------------
        private static async UniTask<SceneLoaderService> InitializeSceneService()
        {
            var sceneService = new SceneLoaderService();
            ServiceLocator.RegisterService<ISceneLoaderService>(sceneService);
            await sceneService.InitializeAsync();
            return sceneService;
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

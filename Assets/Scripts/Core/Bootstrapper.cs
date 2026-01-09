using Cysharp.Threading.Tasks;
using Softgames.Core.Services;
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

            var sceneService = new SceneLoaderService();
            ServiceLocator.RegisterService(sceneService);
            
            await sceneService.InitializeAsync();
            
            Logging.Log("Boot initialization completed.");
            
            if (_loadMenuOnStart)
            {
                await sceneService.LoadSceneAsync("MainMenu", true);
            }
        }
    }
}

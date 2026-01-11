using Cysharp.Threading.Tasks;
using Softgames.Core;
using Softgames.Core.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Softgames.Levels.PhoenixFlame
{
    [RequireComponent(typeof(Animator))]
    public class PhoenixFireController : MonoBehaviour
    {
        [Header("Particle Systems")]
        [SerializeField] private ParticleSystem[] _fireParticles;
        
        [Header("Settings")]
        [SerializeField] private float _colorSmoothness = 5f;
        
        [Header("UI")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _changeColorButton;
        
        private readonly Color _orangeColor = new(1f, 0.4f, 0f, 1f);
        private readonly Color _greenColor = new(0f, 1f, 0.2f, 1f);
        private readonly Color _blueColor = new(0f, 0.6f, 1f, 1f);

        private Color _targetColor;
        private Color _currentColor;
        private Animator _animator;
        
        private static readonly int ColorStateHash = Animator.StringToHash("ColorState");
        
        private ISceneLoaderService _sceneLoader;

        //-------------------------------------------------------------------------
        private void Awake()
        {
            _sceneLoader = ServiceLocator.GetService<ISceneLoaderService>();
            _changeColorButton.onClick.AddListener(OnChangeColorClicked);
            _backButton.onClick.AddListener(LoadMainMenu);
            _animator = GetComponent<Animator>();
            _targetColor = _orangeColor;
            _currentColor = _orangeColor;
        }

        //-------------------------------------------------------------------------
        private void OnDestroy()
        {
            _changeColorButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
        }

        //-------------------------------------------------------------------------
        private void Update()
        {
            if (_currentColor != _targetColor)
            {
                _currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * _colorSmoothness);
            }
            
            foreach (var fireParticle in _fireParticles)
            {
                var main = fireParticle.main;
                main.startColor = _currentColor;
            }
        }

        //-------------------------------------------------------------------------
        private void OnChangeColorClicked()
        {
            int currentState = _animator.GetInteger(ColorStateHash);
            int nextState = (currentState + 1) % 3;
            
            _animator.SetInteger(ColorStateHash, nextState);
        }

        //-------------------------------------------------------------------------
        public void SetTargetColorState(int stateIndex)
        {
            switch (stateIndex)
            {
                case 0: _targetColor = _orangeColor; break;
                case 1: _targetColor = _greenColor; break;
                case 2: _targetColor = _blueColor; break;
            }
        }
        
        //-------------------------------------------------------------------------
        private void LoadMainMenu()
        {
            _sceneLoader.LoadSceneAsync("MainMenu").Forget();
        }
    }
}

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using Softgames.Core.Services;
using UnityEngine;

namespace Levels.AceOfShadows
{
    public class AceOfShadowsController : MonoBehaviour
    {
        public event Action<int, int> OnStacksUpdated;
        public event Action OnGameStarted;
        public event Action OnGameFinished;
        
        [Header("Configuration")]
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private int _totalCards;
        [SerializeField] private float _moveDuration;
        [SerializeField] private Vector3 _stackOffset = new(0, 0.005f, 0);

        [Header("References")]
        [SerializeField] private Transform _stackAPos;
        [SerializeField] private Transform _stackBPos;
        
        private readonly Stack<CardView> _stackA = new();
        private readonly Stack<CardView> _stackB = new();
        
        private IAudioService _audioService;
        
#if UNITY_EDITOR
        [Header("Debug / Testing")]
        [SerializeField, Range(1f, 100f)] private float _simulationSpeed = 1f;
        
        private float TimeMultiplier => _simulationSpeed;
        
        [ContextMenu("Finish Immediately")]
        public void DebugFinishImmediately()
        {
            _simulationSpeed = 100f;
        }
#else
        private float TimeMultiplier => 1f; 
#endif
        
        //-----------------------------------------------------------------------
        private void Start()
        {
            _audioService = ServiceLocator.GetService<IAudioService>();
            _audioService.PlayMusic("levelMusic");
            InitializeStacks();
            RunGameLoop().Forget();
        }

        //-----------------------------------------------------------------------
        private void InitializeStacks()
        {
            _stackA.Clear();
            _stackB.Clear();
            
            for (int i = 0; i < _totalCards; i++)
            {
                var cardObj = Instantiate(_cardPrefab, _stackAPos);
                var cardView = cardObj.GetComponent<CardView>();
                
                cardObj.transform.position = _stackAPos.position + (_stackOffset * i);
                
                cardView.SetCardSortingOrder(i);
                
                _stackA.Push(cardView);
            }
            
            UpdateCounters();
        }

        //-----------------------------------------------------------------------
        private void UpdateCounters()
        {
            OnStacksUpdated?.Invoke(_stackA.Count, _stackB.Count);
        }

        //-----------------------------------------------------------------------
        private async UniTask RunGameLoop()
        {
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
            OnGameStarted?.Invoke();
            
            while (_stackA.Count > 0)
            {
                await MoveCard(_stackA, _stackB, _stackBPos);
            }
            
            OnGameComplete();
        }

        //-----------------------------------------------------------------------
        private async UniTask MoveCard(Stack<CardView> stackA, Stack<CardView> stackB, Transform stackBPos)
        {
            var card = stackA.Pop();
            var newIndexInStackB = stackB.Count;
            var targetPosition = stackBPos.position + (_stackOffset * newIndexInStackB);
            card.SetCardSortingOrder(999);
            
            float actualDuration = _moveDuration / TimeMultiplier;
            
            _audioService.PlaySFX("cardMove");
            await Tween.Position(card.transform, targetPosition, actualDuration, Ease.InOutQuad)
                       .ToYieldInstruction()
                       .ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
            
            card.SetCardSortingOrder(newIndexInStackB);
            card.transform.SetParent(stackBPos);
            stackB.Push(card);
            
            UpdateCounters();
            
            int delayMs = (int)(100 / TimeMultiplier); 
            delayMs = Mathf.Max(1, delayMs); 
            await UniTask.Delay(delayMs, cancellationToken: this.GetCancellationTokenOnDestroy());
        }

        //-----------------------------------------------------------------------
        private void OnGameComplete()
        {
            _audioService.PlaySFX("gameComplete");
            OnGameFinished?.Invoke();
        }
    }
}

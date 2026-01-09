using TMPro;
using UnityEngine;

namespace Softgames.Core
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsValue;
        [SerializeField] private float _updateInterval;
        
        private float _accumulatedTime;
        private int _frames;
        private float _timeLeft;

        //-----------------------------------------------------------------------
        private void Start()
        {
            if (_fpsValue == null)
            {
                Logging.LogError("FPSCounter: TMP_Text component is not assigned.");
                enabled = false;
                return;
            }
            
            _timeLeft = _updateInterval;
        }
        
        //-----------------------------------------------------------------------
        private void Update()
        {
            _timeLeft -= Time.unscaledDeltaTime;
            _accumulatedTime += Time.timeScale / Time.unscaledDeltaTime;
            _frames++;
            
            if (_timeLeft <= 0.0f)
            {
                float fps = _accumulatedTime / _frames;
                
                _fpsValue.text = Mathf.RoundToInt(fps).ToString();
                _fpsValue.color = fps >= 50 ? Color.green : fps >= 30 ? Color.yellow : Color.red;
            
                _timeLeft = _updateInterval;
                _accumulatedTime = 0.0f;
                _frames = 0;
            }
        }
    }
}
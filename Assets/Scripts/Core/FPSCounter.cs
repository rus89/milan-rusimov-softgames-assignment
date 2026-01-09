using TMPro;
using UnityEngine;

namespace Softgames.Core
{
    public class FPSCounter : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TMP_Text _fpsValue;
        [SerializeField] private float _updateInterval = 0.5f;
        
        private float _accumulatedTime;
        private int _frames;

        //-----------------------------------------------------------------------
        private void Start()
        {
            if (_fpsValue == null)
            {
                Logging.LogError("FPSCounter: TMP_Text component is not assigned.");
                enabled = false;
            }
        }
        
        //-----------------------------------------------------------------------
        private void Update()
        {
            _accumulatedTime += Time.unscaledDeltaTime;
            _frames++;

            if (_accumulatedTime >= _updateInterval)
            {
                float fps = _frames / _accumulatedTime;
                _fpsValue.SetText("{0:0}", fps);
                _fpsValue.color = fps >= 50 ? Color.green : fps >= 30 ? Color.yellow : Color.red;

                _accumulatedTime = 0f;
                _frames = 0;
            }
        }
    }
}
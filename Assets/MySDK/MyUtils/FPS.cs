using UnityEngine;
using UnityEngine.UI;

namespace MyUtils
{
    [RequireComponent(typeof(Text))]
    public class FPS : MonoBehaviour
    {
        [SerializeField] private float _updateInterval = 0.5f;

        [SerializeField] private Text _fpsText;

        private float _accum = 0;
        private int _frames = 0;
        private float _timeLeft = 0;
        private float _fps = 0;

        private void Start()
        {
            _timeLeft = _updateInterval;
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            _accum += Time.timeScale / Time.deltaTime;
            _frames++;

            if (_timeLeft > 0)
            {
                return;
            }

            _fps = _accum / _frames;
            _timeLeft = _updateInterval;
            _accum = 0;
            _frames = 0;

            _fpsText.text = $"{_fps:F2}";
        }
    }
}

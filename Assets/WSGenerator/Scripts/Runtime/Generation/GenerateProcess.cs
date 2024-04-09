using System.Collections;
using UnityEngine;

namespace SkyCrush.WSGenerator
{
    public class GenerateProcess
    {
        public GenerateObjectInfo GenerateObjectInfo { get; private set; }
        public PoolContainer PoolContainer { get; private set; }
        public float Frequency { get; private set; }

        private Settings _settings;
        private Generator _generator;
        private Stage _stage;
        private bool _isPaused;
        private bool _isStopped;
        private GridManager _gridManager;

        public GenerateProcess(Settings settings, Generator generator, Stage stage, GenerateObjectInfo objectInfo, PoolContainer poolContainer, GridManager gridManager)
        {
            _settings = settings;
            _generator = generator;
            _stage = stage;
            GenerateObjectInfo = objectInfo;
            PoolContainer = poolContainer;
            _gridManager = gridManager;
        }

        public void UpdateFrequency(float frequency)
        {
            Frequency = frequency;
        }

        public void Start()
        {
            _generator.StartCoroutine(Generation());
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Unpause()
        {
            _isPaused = false;
        }

        public void Stop()
        {
            _isStopped = true;
        }

        private IEnumerator Generation()
        {
            var processTime = 0.0f;

            while (!_isStopped)
            {
                while (_isPaused) yield return null;

                processTime += Time.deltaTime;

                while (Frequency < _settings.MinFrequencyGenerationValue)
                {
                    if (processTime < _stage.Duration) yield return null;
                    else yield break;
                }

                Generate();

                var clampFrequency = Mathf.Clamp(Frequency, _settings.MinFrequencyGenerationValue, _settings.MaxFrequencyGenerationValue);
                var pastTime = 0.0f;
                var waitTime = (float) (1 / clampFrequency);
                while(pastTime < waitTime)
                {
                    waitTime = (float)(1 / clampFrequency);

                    yield return null;

                    pastTime += Time.deltaTime;
                }
            }
        }

        private void Generate()
        {
            var generatedObject = PoolContainer.Get();

            generatedObject.transform.position = _gridManager.GetRandomActiveGenerationPoint().transform.position;
        }
    }
}

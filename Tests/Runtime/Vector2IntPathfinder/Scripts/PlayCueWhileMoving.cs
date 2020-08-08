using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.Vector2IntPathfinder
{
    public class PlayCueWhileMoving : MonoBehaviour
    {
        public bool Moving => _wasMoving;

        [SerializeField] private AudioCue _cue;
        [SerializeField] float velThreshold = 1f;
        [SerializeField] float hysterisisPeriod = 0.2f;

        private Vector3 _prevPos;
        private float _lastTimeSwitched;
        private bool _wasMoving;

        private void OnEnable()
        {
            _prevPos = transform.position;
        }

        private void Update()
        {
            bool isMoving = false;
            Vector3 moveDelta = transform.position - _prevPos;
            if (moveDelta.sqrMagnitude > float.Epsilon)
            {
                isMoving = (moveDelta / Time.deltaTime).magnitude > velThreshold;
            }

            if (isMoving != _wasMoving)
            {
                if (Time.time - _lastTimeSwitched < hysterisisPeriod)
                {
                    return;
                }

                _wasMoving = isMoving;
                if (isMoving)
                {
                    _cue.Play(transform, () => { return this.Moving; });
                }
            }
            else
            {
                _lastTimeSwitched = Time.time;
            }
        }

        private void LateUpdate()
        {
            _prevPos = transform.position;
        }
    }
}
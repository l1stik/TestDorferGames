using System.Collections;
using _Project.Scripts.Managers;
using UnityEngine;
using Random = System.Random;

namespace _Project.Scripts.Entities
{
    public class LogEntity : MonoBehaviour
    {
        #region Variable

        [SerializeField]
        private float _speedRotation;

        [SerializeField] 
        private float _stopDelayTime;
        
        [SerializeField] 
        private float _duratioтRotation;
        
        private Transform _transform;

        private AnimationCurve _animationCurve;

        private Random _random;
        
        public KnifeManager knifeManager { private get; set; }

        #endregion
        
        public void Start()
        {
            _random = new Random();
            knifeManager.InstantiateKnifeForLog();
                
            _transform = gameObject.transform;
            
            _animationCurve = new AnimationCurve(
                new Keyframe(0, 1),
                new Keyframe(1, _random.Next(1, 2)),
                new Keyframe(2, _random.Next(1, 3)),
                new Keyframe(3, _random.Next(1, 4)),
                new Keyframe(4, _random.Next(1, 2)),
                new Keyframe(5, 1));

            var methods = _random.Next(0, 2);
            switch (methods)
            {
                case 0:
                    StartCoroutine(RotateRight(_duratioтRotation));
                    break;
                
                case 1:
                    StartCoroutine(RotateLeft(_duratioтRotation));
                    break;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(Shake());
        }
        
        private IEnumerator Shake()
        {
            yield return new WaitForSeconds(0.05f);
            
            var t = 0.0f;
            while ( t < 0.1 )
            {
                t += Time.deltaTime;
                
                transform.position =  new Vector3(0, Mathf.Sin(t  * 0.4f) * 1f, 0);

                yield return new WaitForFixedUpdate();
            }
        }
        
        private IEnumerator RotateRight(float duration)
        {
            var t = 0.0f;
            while ( t  < duration )
            {
                t += Time.deltaTime;
                _transform.Rotate(0, _animationCurve.Evaluate(t) * _speedRotation, 0);
                yield return new WaitForFixedUpdate();
            }
            StartCoroutine(StopDelay());
        }
        
        private IEnumerator RotateLeft(float duration)
        {
            var t = 0.0f;
            while ( t  < duration )
            {
                t += Time.deltaTime;

                _transform.Rotate(0, -_animationCurve.Evaluate(t) * _speedRotation, 0);
                yield return new WaitForFixedUpdate();
            }
            StartCoroutine(StopDelay());
        }
        
        private IEnumerator StopDelay()
        {
            float elapsedTime = 0;
            
            while (elapsedTime <= _stopDelayTime)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            
            var methods = _random.Next(0, 2);
            switch (methods)
            {
                case 0:
                    StartCoroutine(RotateRight(_duratioтRotation));
                    break;
                
                case 1:
                    StartCoroutine(RotateLeft(_duratioтRotation));
                    break;
            }
        }
    }
}
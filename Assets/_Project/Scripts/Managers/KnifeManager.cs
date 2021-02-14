using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Entities;
using _Project.Scripts.UI.HUD;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace _Project.Scripts.Managers
{
    public class KnifeManager: MonoBehaviour, IObserver<Unit>, IObserver<Collider>
    {
        #region Variable

        [SerializeField] 
        private LevelManager _levelManager;
        
        [SerializeField] 
        private CounterKnifesManager _сounterKnifesManager;
        
        [SerializeField] 
        private LogManager _logManager;
        
        [SerializeField] 
        private TopPanel _topPanel;
        
        [SerializeField] 
        private GameObject _knifePrefab;
        
        [SerializeField] 
        private GameObject _knifeForLogPrefab;
        
        [SerializeField] 
        private float _speedThrowKnife;
        
        [SerializeField] 
        private float _throwDelayTime = 0.2f;

        private const float DISTANCE = 2f;
        private const float SPEED_OCCURRENCE_KNIFE = 0.5f;

        private List<GameObject> _listKnifes;
        
        private IEnumerator _throwKnifeCoroutine;

        private IEnumerator _occurrenceKnifeCoroutine;
        
        private IDisposable unsubscriberMouseButtonDownEvent;
        
        private IDisposable unsubscriberOnTriggerEnter;
        
        private ParticleSystem _particleObject;

        private int _knifeСounter = 0;
        
        private bool _isThrowPause;
        
        private bool _isCollider;
       
        public GameObject log { private get; set; }
        
        public int countKnifes { private get; set; }

        #endregion

        private void Start()
        {
            _listKnifes = new List<GameObject>();
        }

        public void StartGame()
        {
            unsubscriberMouseButtonDownEvent = new InputHandler().SubscribeMouseButtonDownEvent(this);

            _сounterKnifesManager.delayTime = _throwDelayTime;
            
            _isCollider = false;
            _isThrowPause = false;
            
            InstantiateKnife();
        }
        
        public void EndGame()
        {
            unsubscriberMouseButtonDownEvent.Dispose();
            unsubscriberOnTriggerEnter.Dispose();
            
            _knifeСounter = 0;
            
            foreach (var go in _listKnifes)
            {
                Destroy(go);
            }
            _listKnifes.Clear();
        }
        
        public void InstantiateKnifeForLog()
        {
            const float RADIUS_LOG = 0.65f;
            
            var random = new Random();
            var center = log.transform.position;
            var countKnife = random.Next(1, 4);
            var angle = 0;

            switch (countKnife)
            {
                case 1:
                    angle = 90;
                    break;
                case 2:
                    angle = 180;
                    break;
                case 3:
                    angle = 120;
                    break;
            }
            
            for (var i = 0; i < countKnife; i++)
            {
                
                var ang = i * angle;
                var pos = GetPositionSpawn(center, RADIUS_LOG, ang);
                var rot = Quaternion.FromToRotation(center, center-pos);
                
               var knifeForLog = Instantiate(_knifeForLogPrefab, pos, rot);
               knifeForLog.transform.Rotate(0, 90, 0, Space.Self);
               knifeForLog.transform.parent = log.transform;
            }
        }
        
        private Vector3 GetPositionSpawn(Vector3 center, float radius,int ang)
        {
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            pos.z = center.z;
            return pos;
        }

        private void InstantiateKnife()
        {
            _knifeСounter++;
            
            var knife = Instantiate(_knifePrefab);
            unsubscriberOnTriggerEnter = knife.GetComponent<KnifeEntity>().SubscribeOnTriggerEnter(this);
            _particleObject = knife.GetComponent<ParticleSystem>();

            var logPosition = log.transform.position;
            knife.transform.position = new Vector3(logPosition.x, logPosition.y - DISTANCE, logPosition.z);

            _listKnifes.Add(knife);

            _occurrenceKnifeCoroutine = OccurrenceKnife();
            StartCoroutine(_occurrenceKnifeCoroutine);
        }
        
        private IEnumerator OccurrenceKnife()
        {
            const float beforeTime = SPEED_OCCURRENCE_KNIFE / DISTANCE;
            var t = 0.0f;
            
            while (t < beforeTime)
            {
                t += Time.deltaTime;
                if (_listKnifes != null)
                {
                    _listKnifes.Last().transform.position += Vector3.up * (SPEED_OCCURRENCE_KNIFE * Time.deltaTime);
                }
                else
                {
                    StopCoroutine(_occurrenceKnifeCoroutine);
                }
                yield return new WaitForFixedUpdate();
            }
        }
        
        private IEnumerator ThrowKnife()
        {
            while (!_isCollider)
            {
                if (_listKnifes != null)
                {
                    _listKnifes.Last().transform.position = new Vector3(0, 
                        _listKnifes.Last().transform.position.y + _speedThrowKnife, 0);
                }
                else
                {
                    StopCoroutine(_throwKnifeCoroutine);
                }
                yield return new WaitForFixedUpdate();
            }
            _isCollider = false;
        }

        private void CheckCollider(Component collider)
        {
            const string CURRENCY_TAG = "Currency";
            const string LOG_TAG = "Log";
            const string KNIFE_TAG = "Knife";
            const string LAST_KNIFE_TAG = "LastKnife";
            const string ANIMATION_REBOUND_KNIFE = "AnimationReboundKnife";


            var transformLastKnife = _listKnifes.Last().transform;
            
            switch (collider.transform.gameObject.tag)
            {
                case KNIFE_TAG:
                {
                    _isThrowPause = true;
                    _isCollider = true;
                    
                    StartCoroutine(DelayEndLvL(0.5f));
                    
                    _listKnifes.Last().GetComponent<Animation>().Play(ANIMATION_REBOUND_KNIFE);
                    _listKnifes.Last().GetComponent<BoxCollider>().enabled = false;
                    
                    Handheld.Vibrate();
                    
                    StopCoroutine(_throwKnifeCoroutine);
                    StopCoroutine(_occurrenceKnifeCoroutine);
                }
                    break;
                
                case LOG_TAG:
                {
                    Handheld.Vibrate();
                    transformLastKnife.parent = collider.transform;
                    
                        if (_knifeСounter < countKnifes)
                        {
                            _particleObject.Play();
                            InstantiateKnife();
                        }
                        else
                        {
                            _particleObject.Play();

                            transformLastKnife.tag = LAST_KNIFE_TAG;
                            
                            var hitColliders = Physics.OverlapSphere(transformLastKnife.position,
                                _listKnifes.Last().gameObject.transform.lossyScale.x).ToList();
                            
                            var knifeHit = false;
                            foreach (var hit in hitColliders.Where(hit => hit.CompareTag(KNIFE_TAG)))
                            {
                                knifeHit = true;
                            }

                            if (!knifeHit)
                            {
                                _logManager.Breakdown(); 
                                _knifeСounter = 0;
                            }
                        }
                } 
                    break;

                case CURRENCY_TAG:
                {
                    _topPanel.DisplayCurrencyCount();
                } 
                    break;
            }
        }
        
        private IEnumerator DelayThrowKnife()
        {
            float elapsedTime = 0;
            
            while (elapsedTime <= _throwDelayTime)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            _isThrowPause = false;
        }
        
        private IEnumerator DelayEndLvL(float delayTime)
        {
            
            float elapsedTime = 0;
            
            while (elapsedTime <= delayTime)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            _levelManager.UploadLevel(Result.Failure);
        }
        
        #region IObserverMethods

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Collider value)
        {
            _isCollider = true;
            CheckCollider(value);
        }

        public void OnNext(Unit value)
        {
            if (_isThrowPause) return;
           
            _isThrowPause = true;
            
            _throwKnifeCoroutine = ThrowKnife();
            StartCoroutine(_throwKnifeCoroutine);
                
            StartCoroutine(DelayThrowKnife());
        }

        #endregion
    }
}
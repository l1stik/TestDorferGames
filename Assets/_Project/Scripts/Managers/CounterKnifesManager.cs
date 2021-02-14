using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Managers
{
    public class CounterKnifesManager : MonoBehaviour, IObserver<Unit>
    {
        #region VARIABLE

        [SerializeField]
        private KnifeManager _knifeManager;
        
        [SerializeField]
        private Image _imagePrefab;
        
        [SerializeField]
        private Sprite _imageOutlineKnife;
        
        [SerializeField] 
        private int _countKnifes;

        private List<Image> _listImage;

        private int _counterKnifes;
        
        private IDisposable _unsubscriber;
        
        private bool _isDelay;
        
        public float delayTime { private get; set; }

        #endregion
        
        public void CreateInstanceCounter()
        {
            _isDelay = false;
            
            _unsubscriber = new InputHandler().SubscribeMouseButtonDownEvent(this);

            _counterKnifes = _countKnifes - 1;
            _knifeManager.countKnifes = _countKnifes;
            
            _listImage = new List<Image>();

            for (var i = 0; i < _countKnifes; i++)
            {
                _listImage.Add(Instantiate(_imagePrefab, transform));
            }
        }

        public void ReduceKnifeCounter()
        {
            if (_listImage != null)
            {
                _listImage[_counterKnifes--].GetComponent<Image>().sprite = _imageOutlineKnife;
            }
        }

        public void EndGame()
        {
            _unsubscriber.Dispose();
            
            foreach (var image in _listImage)
            {
                Destroy(image.gameObject);
            }
            _listImage.Clear();

            _isDelay = false;
        }
        
        private IEnumerator Delay()
        {
            float elapsedTime = 0;
            
            while (elapsedTime <= delayTime)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            _isDelay = false;
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

        public void OnNext(Unit value)
        {
            if (_isDelay) return;
           
            _isDelay = true;
            
            ReduceKnifeCounter();
            StartCoroutine(Delay());
        }
        
        #endregion
    }
}
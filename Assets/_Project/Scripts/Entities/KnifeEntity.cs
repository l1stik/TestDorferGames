using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace _Project.Scripts.Entities
{
    public class KnifeEntity : MonoBehaviour
    {
        public IDisposable SubscribeOnTriggerEnter(IObserver<Collider> observer)
        {
            return this.OnTriggerEnterAsObservable().Subscribe(observer.OnNext);
        }
    }
}
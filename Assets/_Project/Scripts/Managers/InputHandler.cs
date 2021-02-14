using System;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public  class InputHandler 
    {
          public IDisposable SubscribeMouseButtonDownEvent(IObserver<Unit> observer)
          {
              return Observable.EveryUpdate()
                  .Where(_ => Input.GetMouseButtonDown(0))
                  .Subscribe(_ =>
                  {
                      observer.OnNext(Unit.Default);
                  });
          }
    }
}
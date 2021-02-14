using UnityEngine;

namespace _Project.Scriptable_Objects
{
    [CreateAssetMenu]
    public class CurrencyManager : ScriptableObject
    {
        #region Variable

        [SerializeField] 
        private int _chance;

        #endregion

        public bool IsShowObject => SetCurrencyObject();

        private bool SetCurrencyObject()
        { 
            var random = new System.Random();

            var randValue = random.Next(0, 100); 
            return randValue <= _chance;
        }
    }
}
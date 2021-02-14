using _Project.Scriptable_Objects;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Entities
{
    public class CurrencyEntity : MonoBehaviour
    {
        #region Variable
        
        [SerializeField] 
        private CurrencyManager _currencyManager;

        #endregion
        
        private void Start()
        {
            gameObject.SetActive(_currencyManager.IsShowObject);
        }

        public void OnTriggerEnter(Collider collider)
        {
            gameObject.SetActive(false);
            DataManager.SetCurrencyCount(1);
        }

    }
}
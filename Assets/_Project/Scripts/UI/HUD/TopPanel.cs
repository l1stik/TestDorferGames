using _Project.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.HUD
{
    public class TopPanel : MonoBehaviour
    {
        #region Variable

        [SerializeField] 
        private TMP_Text _currencyCountTMPText;

        #endregion
        
        public void DisplayCurrencyCount()
        {
            _currencyCountTMPText.text = DataManager.GetCurrencyCount().ToString();
        }
    }
}
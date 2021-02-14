using _Project.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Windows
{
    public class MenuWindow : MonoBehaviour
    {
        #region Variable
        
        [SerializeField] 
        private UIManager _uiManager;
        
        [SerializeField] 
        private Button _toStartWindowButton;
        
        [SerializeField]
        private TMP_Text _recordLvLTMPText;

        #endregion
        
        public void Start()
        {
            _toStartWindowButton.onClick.AddListener(() =>
            {
                _uiManager.SetStepGame(StepGame.GameWindow);
            });
            DisplayGlobalRecord();
        }
        
        public void DisplayGlobalRecord()
        {
            _recordLvLTMPText.text = "BEST RECORD" + " " + DataManager.GetRecord().ToString();
        }
    }
}
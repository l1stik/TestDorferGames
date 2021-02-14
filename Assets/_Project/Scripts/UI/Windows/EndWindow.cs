using _Project.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Windows
{
    public class EndWindow : MonoBehaviour
    {
        #region Variable
        
        [SerializeField] 
        private UIManager _uiManager;
        
        [SerializeField] 
        private Button _toMenuWindowButton;
        
        [SerializeField]
        private TMP_Text _recordTMPText;

        #endregion
        
        public void Start()
        {
            _toMenuWindowButton.onClick.AddListener(() =>
            {
                _uiManager.SetStepGame(StepGame.MenuWindow);
            });
        }
        
        public void SetRecord(string record)
        {
            _recordTMPText.text = "YOU PASSED UP TO" + " " + record + " " + "LEVEL";
        }
    }
}
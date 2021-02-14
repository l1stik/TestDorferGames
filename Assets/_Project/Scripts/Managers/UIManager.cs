using _Project.Scripts.UI.HUD;
using _Project.Scripts.UI.Windows;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public enum StepGame
    {
        MenuWindow,
        GameWindow,
        EndWindow
    }
    
    public class UIManager : MonoBehaviour
    {
        #region Variable
        
        [SerializeField] 
        private LevelManager _levelManager;
      
        [SerializeField] 
        private MenuWindow _menuWindow;
        
        [SerializeField] 
        private TopPanel _topPanel;
        
        [SerializeField] 
        private RectTransform _menuPanel;
        
        [SerializeField] 
        private RectTransform _endPanel;
        
        [SerializeField] 
        private RectTransform _knifesCounterPanel;
        
        private int _record = 0;

        #endregion
        
        private void Awake()
        {
            _topPanel.gameObject.SetActive(true);
            _topPanel.DisplayCurrencyCount();
            SetStepGame(StepGame.MenuWindow);
        }

        public void SetStepGame(StepGame stepGame)
        {
            switch (stepGame)
            {
                case StepGame.MenuWindow:
                {
                    _menuWindow.DisplayGlobalRecord();
                    
                    _knifesCounterPanel.gameObject.SetActive(true);
                    _menuPanel.gameObject.SetActive(true);
                    _endPanel.gameObject.SetActive(false);
                }
                    break;
                
                case StepGame.GameWindow:
                {
                    _knifesCounterPanel.gameObject.SetActive(true);
                    _menuPanel.gameObject.SetActive(false);
                    
                    _levelManager.LoadLevel();
                }
                    break;
                
                case StepGame.EndWindow:
                {
                    _knifesCounterPanel.gameObject.SetActive(false);
                    _menuPanel.gameObject.SetActive(false);
                    _endPanel.gameObject.SetActive(true);
                    
                }
                    break;
            }
        }
    }
}
using _Project.Scriptable_Objects;
using _Project.Scripts.UI.Windows;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public enum Result
    {
        Win,
        Failure
    }
    
    public class LevelManager : MonoBehaviour
    {
        #region Variable
        
        [SerializeField]
        private LogData _bossLogData;
        
        [SerializeField]
        private LogData _simpleLogData;
        
        [SerializeField]
        private int _countLvL;
        
        [SerializeField] 
        private KnifeManager _knifeManager;
      
        [SerializeField] 
        private UIManager _uiManager;
         
        [SerializeField] 
        private LogManager _logManager;
        
        [SerializeField] 
        private EndWindow _endWindow;
        
        [SerializeField] 
        private CounterKnifesManager _counterKnifesManager;

        private int _localRecord = 0;
        
        private int _currentCounterLvL = 1;
        
        #endregion
        
        public void LoadLevel()
        {
            _counterKnifesManager.CreateInstanceCounter();
            _logManager.CreateSimpleLog(_simpleLogData.Skin, _simpleLogData.SemicircleSkin, 0f);
            _knifeManager.StartGame(); 
        }
        
        public void LoadBossLvL()
        {
            _counterKnifesManager.CreateInstanceCounter();
            _logManager.CreateBossLog(_bossLogData.Skin, _bossLogData.SemicircleSkin,0f);
            _knifeManager.StartGame();
        }
        
        public void UploadLevel(Result result)
        {
            switch (result)
            {
                case Result.Win:
                {
                    ++_localRecord;
                    
                    if (_currentCounterLvL < _countLvL)
                    {
                        _counterKnifesManager.EndGame();
                        LoadLevel();
                        
                        _currentCounterLvL++;
                    }
                    else
                    {
                        _counterKnifesManager.EndGame();
                        LoadBossLvL();
                        
                        _currentCounterLvL = 1;
                    }
                } 
                    break;
                
                case Result.Failure:
                {
                    _knifeManager.EndGame();
                    _logManager.EndGame();
                    _counterKnifesManager.EndGame();
                    
                    _endWindow.SetRecord(_localRecord.ToString());
                    DataManager.SetRecord(_localRecord);

                    _uiManager.SetStepGame(StepGame.EndWindow);
                    
                    _localRecord = 0;
                } 
                    break;
            }
        }
    }
}
using System.Collections;
using _Project.Scripts.Entities;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class LogManager : MonoBehaviour
    {
        #region Variable
        
        [SerializeField] 
        private KnifeManager _knifeManager;
       
        [SerializeField] 
        private LevelManager _levelManager;
        
        [SerializeField] 
        private GameObject _logPrefab;
        
        [SerializeField] 
        private GameObject _semicircleUp;
        
        [SerializeField] 
        private GameObject _breakdownKnife;
        
        [SerializeField] 
        private GameObject _semicircleDown;
        
        [SerializeField] 
        private Material _logMaterial;
        
        [SerializeField] 
        private Material _semicircleMaterial;
        
        private GameObject _log;
        
        private GameObject semicircleUp;
        
        private GameObject semicircleDown;
       
        private GameObject breakdownKnife;
        
        private Texture2D _logTexture;
        
        private Texture2D _bossLogTexture;
      
        private Texture2D _semicircleTexture;

        #endregion

        public void CreateSimpleLog(Texture2D logSkin, Texture2D semicircleSkin, float speed)
        {
            _log = Instantiate(_logPrefab);

            _logMaterial.mainTexture = logSkin;
            _semicircleMaterial.mainTexture = semicircleSkin;
            _log.GetComponent<LogEntity>().knifeManager = _knifeManager;
            _knifeManager.log = _log;
        }
        
        public void CreateBossLog(Texture2D logSkin, Texture2D semicircleSkin, float speed)
        {
            _log = Instantiate(_logPrefab);

            _logMaterial.mainTexture = logSkin;
            _semicircleMaterial.mainTexture = semicircleSkin;
            _log.GetComponent<LogEntity>().knifeManager = _knifeManager;
            _knifeManager.log = _log;
        }
        
        public void Breakdown()
        {
            EndGame();
            
            breakdownKnife = Instantiate(_breakdownKnife);
            breakdownKnife.transform.position = _log.transform.position;
            
            semicircleUp = Instantiate(_semicircleUp);
            semicircleDown = Instantiate(_semicircleDown);
            
            var logPos = _log.transform.position;
            var localScale = _log.transform.localScale;
            
            semicircleUp.transform.position = new Vector3(logPos.x, logPos.y - localScale.y, logPos.z);
            semicircleDown.transform.position = new Vector3(logPos.x, logPos.y - localScale.y, logPos.z);
            
            StartCoroutine(FallSemicircleUp());
            StartCoroutine(FallSemicircleDown());
            
        }
        
        private IEnumerator FallSemicircleUp()
        {
            var animationCurveX = new AnimationCurve(
                new Keyframe(0.1f, 0.001f),
                new Keyframe(0.2f, 0.002f),
                new Keyframe(0.3f, 0.003f),
                new Keyframe(0.4f, -0.002f),
                new Keyframe(0.5f, -0.003f),
                new Keyframe(0.6f, -0.004f),
                new Keyframe(0.7f, -0.005f),
                new Keyframe(0.8f, -0.006f),
                new Keyframe(0.9f, -0.007f),
                new Keyframe(1, -0.008f));
            
            var animationCurveY = new AnimationCurve(
                new Keyframe(0.1f, 0.01f),
                new Keyframe(0.2f, 0.02f),
                new Keyframe(0.3f, 0.03f),
                new Keyframe(0.4f, -0.02f),
                new Keyframe(0.5f, -0.03f),
                new Keyframe(0.6f, -0.04f),
                new Keyframe(0.7f, -0.05f),
                new Keyframe(0.8f, -0.06f),
                new Keyframe(0.9f, -0.07f),
                new Keyframe(1, -0.08f));
            
            
            var t = 0.0f;
            while ( t  < 1.5f )
            {
                t += Time.deltaTime;
                var semicircleUpPosition = semicircleUp.transform.position;
                semicircleUp.transform.position = new Vector3(semicircleUpPosition.x + animationCurveX.Evaluate(t),
                    semicircleUpPosition.y + animationCurveY.Evaluate(t), semicircleUpPosition.z);
                
                var angle = 20f * t;
                semicircleUp.transform.Rotate(angle, angle, angle);
                
                yield return new WaitForFixedUpdate();
            }
            Destroy(semicircleUp);
           
            Destroy(breakdownKnife);
            
            _levelManager.UploadLevel(Result.Win);
        }
        
        private IEnumerator FallSemicircleDown()
        {
            var animationCurveX = new AnimationCurve(
                new Keyframe(0.1f, -0.005f),
                new Keyframe(0.2f, -0.007f),
                new Keyframe(0.3f, -0.009f),
                new Keyframe(0.4f, 0.002f),
                new Keyframe(0.5f, 0.003f),
                new Keyframe(0.6f, 0.004f),
                new Keyframe(0.7f, 0.005f),
                new Keyframe(0.8f, 0.006f),
                new Keyframe(0.9f, 0.007f),
                new Keyframe(1, 0.008f));
            
            var animationCurveY = new AnimationCurve(
                new Keyframe(0.1f, 0.001f),
                new Keyframe(0.2f, 0.002f),
                new Keyframe(0.3f, 0.003f),
                new Keyframe(0.4f, -0.02f),
                new Keyframe(0.5f, -0.03f),
                new Keyframe(0.6f, -0.04f),
                new Keyframe(0.7f, -0.05f),
                new Keyframe(0.8f, -0.06f),
                new Keyframe(0.9f, -0.07f),
                new Keyframe(1, -0.08f));
            
            
            var t = 0.0f;
            while ( t  < 1.5f )
            {
                t += Time.deltaTime;
                var semicircleDownPosition = semicircleDown.transform.position;
                semicircleDown.transform.position = new Vector3(semicircleDownPosition.x + animationCurveX.Evaluate(t), 
                    semicircleDownPosition.y + animationCurveY.Evaluate(t), semicircleDownPosition.z);
                
                var angle = -15f * t;
                semicircleDown.transform.Rotate(angle, angle, angle);
                
                yield return new WaitForFixedUpdate();
            }
            Destroy(semicircleDown);
        }

        public void EndGame()
        {
            Destroy(_log);
        }
    }
}
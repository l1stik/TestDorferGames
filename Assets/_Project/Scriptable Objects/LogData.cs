using UnityEngine;

namespace _Project.Scriptable_Objects
{
    [CreateAssetMenu]
    public class LogData : ScriptableObject
    {
        #region Variable
        
        [SerializeField]
        private Texture2D _skin;
        
        [SerializeField]
        private Texture2D _semicircleSkin;
        
        [SerializeField]
        private float _speed;
        
        #endregion
        
        public Texture2D Skin => _skin;
        public Texture2D SemicircleSkin => _semicircleSkin;
        public float Speed => _speed;
        
    }
}
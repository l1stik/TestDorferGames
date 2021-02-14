using UnityEngine;

namespace _Project.Scripts.Entities
{
    public class BreakdownKnife : MonoBehaviour
    {
        [SerializeField] 
        private Animation _animation;
        
        private void Start()
        {
            const string BREAK_DOWN_KNIFE_ANIMATION = "BreakDownKnife";
            
            _animation.Play(BREAK_DOWN_KNIFE_ANIMATION);
        }
    }
}
using UnityEngine;

namespace Code.Combat
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AttackData", order = 0)]
    public class AttackDataSO : ScriptableObject
    {
        public float damage;
        public AnimationClip attackClip;
        public Vector2 movement;
        public Vector2 knockBackPower;
        public AnimationCurve movementCurve;
    }
}
using UnityEngine;

namespace Code.Entities
{
    [CreateAssetMenu(fileName = "ParamSO", menuName = "SO/Entity/Param", order = 0)]
    public class AnimParamSO : ScriptableObject
    {
        public string paramName;
        public int hashValue;

        private void OnValidate()
        {
            hashValue = Animator.StringToHash(paramName);
        }
    }
}
using Code.Entities;
using UnityEngine;

namespace Code.Effect
{
    public class ParticlePlayer : MonoBehaviour, IPlayableVFX
    {
        [field:SerializeField]public AnimParamSO VFXName { get; private set; }
        [SerializeField] private ParticleSystem particle;
        
        public void PlayVFX()
        {
            particle.Play();
        }
    }
}
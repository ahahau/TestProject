using Code.Entities;

namespace Code.Effect
{
    public interface IPlayableVFX
    {
        public AnimParamSO VFXName { get; }
        public void PlayVFX();
    }
}
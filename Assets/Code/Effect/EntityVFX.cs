using System.Collections.Generic;
using System.Linq;
using Code.Entities;
using UnityEngine;

namespace Code.Effect
{
    public class EntityVFX : MonoBehaviour, IEntityComponent
    {
        private Dictionary<int, IPlayableVFX> _vfxDict;
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
            IPlayableVFX[] vfxList = GetComponentsInChildren<IPlayableVFX>();
            _vfxDict = vfxList.ToDictionary(vfx => vfx.VFXName.hashValue, vfx => vfx);
        }

        public void PlayVFX(int hashValue)
        {
            if (_vfxDict.TryGetValue(hashValue, out IPlayableVFX vfx))
            {
                vfx.PlayVFX();
            }
        }
    }
}
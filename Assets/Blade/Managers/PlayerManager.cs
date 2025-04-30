using Blade.Core.Dependencies;
using Blade.Entities;
using Blade.Players;
using UnityEngine;

namespace Blade.Managers
{
    [DefaultExecutionOrder(-1)]
    public class PlayerManager : MonoBehaviour
    {
        [Inject, SerializeField] private Player player;
        [SerializeField] private EntityFinderSO playerFinder;

        private void Awake()
        {
            playerFinder.SetTarget(player);
        }
    }
}
using System;
using Code.Entities;
using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        
        public EntityMover Mover { get; private set; }
        public EntityRenderer Renderer { get; private set; }


        private void Awake()
        {
            Mover = GetComponentInChildren<EntityMover>();
            Renderer = GetComponentInChildren<EntityRenderer>();
            
            Mover.Initialize(this);
            Renderer.Initialize(this);
        }

        

        

       

       

        

       
    }
}
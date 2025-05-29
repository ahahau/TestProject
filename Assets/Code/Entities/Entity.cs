using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        protected Dictionary<Type, IEntityComponent> _components = new Dictionary<Type, IEntityComponent>();

        protected virtual void Awake()
        {
            IEntityComponent[] components = GetComponentsInChildren<IEntityComponent>(true);
            foreach (IEntityComponent compo in components)
            {
                _components.Add(compo.GetType(), compo);   
            }

            InitializeComponents();
            AfterInitialize();
        }

        protected virtual void InitializeComponents()
        {
            foreach (IEntityComponent compo in _components.Values)
            {
                compo.Initialize(this);
            }
        }
        
        protected virtual void AfterInitialize()
        {
            foreach (IEntityComponent compo in _components.Values)
            {
                if (compo is IAfterInitialize afterInit)
                {
                    afterInit.AfterInitialize();
                }
            }
        }

        public IEntityComponent GetCompo(Type type)
        {
            if (_components.TryGetValue(type, out IEntityComponent component))
            {
                return component;
            }
            return default;
        }
        
        public T GetCompo<T>(bool isDerived = false) where T : IEntityComponent
        {
            if(_components.TryGetValue(typeof(T), out IEntityComponent component))
            {
                return (T)component;
            }

            if (isDerived == false) return default;
            
            Type findType = _components.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (findType != default)
            {
                return (T)_components[findType];
            }
            
            return default;
        }
    }
}
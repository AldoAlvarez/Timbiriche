using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generic.Interfaces
{
    public interface IPoolable
    {
        bool Active { get; }
        public System.Action OnEnable { get; set; }
        public System.Action OnDisable { get; set; }

        void Enable();
        void Disable();
    }
}
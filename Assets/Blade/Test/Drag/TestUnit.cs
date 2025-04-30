using System;
using UnityEngine;

namespace Blade.Test.Drag
{
    public class TestUnit : MonoBehaviour
    {
        [SerializeField] private GameObject selectedIcon;

        private void Start()
        {
            SetSelected(false);
        }

        public void SetSelected(bool isSelected) => selectedIcon.SetActive(isSelected);
    }
}
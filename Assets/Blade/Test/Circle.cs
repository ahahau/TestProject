using UnityEngine;

namespace Blade.Test
{
    public class Circle : Shape
    {
        public float radius;
        public override float CalculateArea()
        {
            return radius * radius * Mathf.PI;
        }
    }
}
using UnityEngine;

namespace Blade.Test
{
    public class AreaCalculator
    {
        public float GetArea(Shape shape)
        {
            return shape.CalculateArea();
        }
    }
}
using UnityEngine;

namespace Code.Test
{
    public class AreaCalculator
    {
        public float GetArea(Shape shape)
        {
            return shape.CalculateArea();
        }
    }

    public abstract class Shape
    {
        public abstract float CalculateArea();
    }
    
    public class Rectangle : Shape
    {
        public float width;
        public float height;
        public override float CalculateArea()
        {
            return width * height;
        }
    }
    public class Circle : Shape
    {
        public float radius;
        public override float CalculateArea()
        {
            return radius * radius * Mathf.PI;
        }
    }

}
using Diana;

namespace TestDriver
{
    internal class PositionComponent : IComponent
    {
        public PositionComponent(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
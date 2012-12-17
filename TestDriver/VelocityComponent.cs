using Diana;

namespace TestDriver
{
    internal class VelocityComponent : IComponent
    {
        public VelocityComponent(double dx, double dy)
        {
            Dx = dx;
            Dy = dy;
        }

        public double Dx { get; set; }
        public double Dy { get; set; }
    }
}
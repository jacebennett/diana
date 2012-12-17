using System;
using Diana;

namespace TestDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            var world = new World();

            world.AddSystem(new MovementSystem());
            world.AddSystem(new RenderingSystem());

            world.AddEntity((e) =>
            {
                e.AddComponent(new PositionComponent(0, 0));
                e.AddComponent(new VelocityComponent(0.7, 0.3));
            });

            world.AddEntity((e) =>
            {
                e.AddComponent(new PositionComponent(24, 7));
            });

            world.AddEntity((e) =>
            {
                e.AddComponent(new PositionComponent(0, 0));
                e.AddComponent(new VelocityComponent(-0.64, 0.42));
            });

            double delta = 0;

            while (true)
            {
                world.Process(delta);

                Console.Write(">");
                var input = Console.ReadLine();
                if (input != null && input.ToLowerInvariant() == "q")
                    return;

                delta = 0.5;
            }
        }
    }
}

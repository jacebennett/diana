using System;
using Diana;
using Diana.Systems;

namespace TestDriver
{
    internal class RenderingSystem : EntityProcessingSystem
    {
        private ComponentMapper<PositionComponent> _positionMapper;
        public override void Initialize()
        {
            base.Initialize();
            _positionMapper = new ComponentMapper<PositionComponent>(World);
            
            _entitySelector = entity => _positionMapper.Has(entity);
        }

        protected override void ProcessEntity(Entity entity, double delta)
        {
            var position = _positionMapper.Map(entity);

            Console.WriteLine("Rendering Entity #{0} ({1})", entity.Id, entity.Guid);
            Console.WriteLine("\tpos: ({0}, {1})", position.X, position.Y);
        }
    }
}
using System;
using System.Collections.Generic;
using Diana;
using Diana.Systems;

namespace TestDriver
{
    internal class MovementSystem : EntityProcessingSystem
    {
        private ComponentMapper<PositionComponent> _positionMapper;
        private ComponentMapper<VelocityComponent> _velocityMapper;

        public override void Initialize()
        {
            base.Initialize();

            _positionMapper = new ComponentMapper<PositionComponent>(World);
            _velocityMapper = new ComponentMapper<VelocityComponent>(World);

            _entitySelector = entity => _positionMapper.Has(entity) && _velocityMapper.Has(entity);
        }

        protected override void ProcessEntity(Entity entity, double delta)
        {
            Console.WriteLine("Moving Entity #{0} ({1})", entity.Id, entity.Guid);

            var position = _positionMapper.Map(entity);
            var velocity = _velocityMapper.Map(entity);

            position.X += velocity.Dx*delta;
            position.Y += velocity.Dy*delta;
        }
        
    }
}
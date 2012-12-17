using System;
using System.Collections.Generic;

namespace Diana.Systems
{
    public abstract class EntityProcessingSystem : EntitySystem
    {
        protected override void ProcessEntities(IEnumerable<Entity> activeEntities, double delta)
        {
            foreach (var entity in activeEntities)
            {
                ProcessEntity(entity, delta);
            }
        }

        protected abstract void ProcessEntity(Entity entity, double delta);
    }
}

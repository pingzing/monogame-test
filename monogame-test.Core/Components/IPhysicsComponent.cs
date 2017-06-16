using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;
using monogame_test.Core.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Core.Components
{
    public interface IPhysicsComponent
    {        
        void Update(GameTime deltaTime, Entity entity, TestMap map);
    }
}

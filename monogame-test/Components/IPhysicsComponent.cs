using Microsoft.Xna.Framework;
using monogame_test.Entities;
using monogame_test.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Components
{
    public interface IPhysicsComponent
    {        
        void Update(GameTime deltaTime, Entity entity, TestMap map);
    }
}

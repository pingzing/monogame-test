using Microsoft.Xna.Framework;
using monogame_test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Components
{
    public interface IPhysicsComponent
    {
        //TODO: Pass in a reference to our world...whatever form that takes
        void Update(GameTime deltaTime, Entity entity);
    }
}

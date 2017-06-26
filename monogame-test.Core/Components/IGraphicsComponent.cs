using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;
using System.Threading.Tasks;

namespace monogame_test.Core.Components
{
    public interface IGraphicsComponent : IDrawableComponent
    {
        Task LoadAsync();        
    }
}

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Core.Content
{
    /// <summary>
    /// Static class for global stuff that everything should have easy, read-only access to.
    /// </summary>
    public static class GlobalAssets
    {    
        public static void Load(ContentManager contentManager)
        {
            BBoxOutline = contentManager.Load<Texture2D>("bbox_outline");
            CollisionOverlay = contentManager.Load<Texture2D>("CollisionOverlay");
        }

        public static Texture2D BBoxOutline { get; private set; }
        public static Texture2D CollisionOverlay { get; private set; }
    }
}

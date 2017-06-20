using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Core.RenderHelpers
{
    public static class BoundingBoxHelper
    {
        public static void DrawRectangle(Rectangle rect, Texture2D tex, Color color, SpriteBatch spriteBatch, bool solid, int thickness)
        {
            if (!solid)
            {

                Vector2 Position = new Vector2(rect.X, rect.Y);
                int border = thickness;

                int borderWidth = (int)(rect.Width) + (border * 2);
                int borderHeight = (int)(rect.Height) + (border);

                DrawStraightLine(new Vector2((int)rect.X, (int)rect.Y), new Vector2((int)rect.X + rect.Width, (int)rect.Y), tex, color, spriteBatch, thickness); //top bar 
                DrawStraightLine(new Vector2((int)rect.X, (int)rect.Y + rect.Height), new Vector2((int)rect.X + rect.Width, (int)rect.Y + rect.Height), tex, color, spriteBatch, thickness); //bottom bar 
                DrawStraightLine(new Vector2((int)rect.X, (int)rect.Y), new Vector2((int)rect.X, (int)rect.Y + rect.Height), tex, color, spriteBatch, thickness); //left bar 
                DrawStraightLine(new Vector2((int)rect.X + rect.Width, (int)rect.Y), new Vector2((int)rect.X + rect.Width, (int)rect.Y + rect.Height), tex, color, spriteBatch, thickness); //right bar 
            }
            else
            {
                spriteBatch.Draw(tex, new Vector2((float)rect.X, (float)rect.Y), rect, color, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.6f);
            }

        }

        public static void DrawStraightLine(Vector2 VecA, Vector2 VecB, Texture2D tex, Color color, SpriteBatch spriteBatch, int thickness)
        {
            Rectangle rec;
            if (VecA.X < VecB.X) // horiz line 
            {
                rec = new Rectangle((int)VecA.X, (int)VecA.Y, (int)(VecB.X - VecA.X), thickness);
            }
            else //vert line 
            {
                rec = new Rectangle((int)VecA.X, (int)VecA.Y, thickness, (int)(VecB.Y - VecA.Y));
            }

            //spriteBatch.Draw(tex, rec, color);
            spriteBatch.Draw(tex, rec, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.6f);
        }
    }
}

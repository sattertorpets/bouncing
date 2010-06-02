using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Bounce
{
    public class BackgroundImagery
    {

        #region Draw

        public void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.BackToFront,
                SaveStateMode.None, camera.ProjectionMatrix);

            spriteBatch.End();
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace BepuPhysicsHelicopter
{
    public class Camera : GameEntity
    {
        public Matrix projection;
        public Matrix view;

        public override void Draw(GameTime gameTime)
        {
        }
        public override void LoadContent()
        {
        }
        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            // view is set using the setView() method
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), Game1.Instance.GraphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
        }

        public Matrix getProjection()
        {
            return projection;
        }

        public Matrix getView()
        {
            return view;
        }

        public Matrix setView(Vector3 look, Vector3 target)
        {
            view = Matrix.CreateLookAt(Position - look, target + Vector3.Up, Vector3.Up);
            return view;
        }
    }
}
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
    public class GameEntity
    {
        public string modelName;
        public Model model = null;
        public Vector3 Position = Vector3.Zero;

        public Vector3 velocity = Vector3.Zero;
        public Vector3 diffuse = new Vector3(1, 1, 1);
        public Quaternion quaternion;

        public Vector3 Look = new Vector3(0, 0, -1);

        public Matrix worldTransform = Matrix.Identity;
        public Matrix localTransform = Matrix.Identity;


        public virtual void LoadContent()
        {
            model = Game1.Instance.Content.Load<Model>(modelName);
        }

        public virtual void Update(GameTime gameTime)
        {
            worldTransform = Matrix.CreateTranslation(Position);
        }


        public virtual void Draw(GameTime gameTime)
        {
            if (model != null)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.DiffuseColor = diffuse;
                        effect.World = localTransform * worldTransform;
                        effect.Projection = Game1.Instance.Camera.getProjection();
                        effect.View = Game1.Instance.Camera.getView();
                    }
                    mesh.Draw();
                }
            }
        }

        public virtual void UnloadContent()
        {
        }
    }
}


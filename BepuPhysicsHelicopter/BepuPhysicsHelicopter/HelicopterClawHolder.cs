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
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;

namespace BepuPhysicsHelicopter
{
    public class HelicopterClawHolder : GameEntity
    {
        public BepuEntity clawHolder;
        Random random = new Random();

        public BepuEntity createClawHolder(Vector3 position, float height, float radius)
        {
            clawHolder = new BepuEntity();
            clawHolder.modelName = "cyl1";
            clawHolder.LoadContent();
            clawHolder.body = new Cylinder(position, height, radius, 1);
            clawHolder.localTransform = Matrix.CreateScale(radius, radius, height);
            //clawHolder.body.Orientation = Quaternion.CreateFromAxisAngle(Vector3.Left, 0.5f);
            //clawHolder.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            clawHolder.body.BecomeKinematic();
            Game1.Instance.Space.Add(clawHolder.body);
            Game1.Instance.Children.Add(clawHolder);
            return clawHolder;
        }
    }
}

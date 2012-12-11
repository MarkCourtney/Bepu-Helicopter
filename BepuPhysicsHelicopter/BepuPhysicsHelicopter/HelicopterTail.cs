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
using BEPUphysics.Entities.Prefabs;


namespace BepuPhysicsHelicopter
{
    public class HelicopterTail
    {
        public BepuEntity tail;

        public BepuEntity createTail(Vector3 pos, Vector3 offSet, float width, float height, float length)
        {
            tail = new BepuEntity();
            tail.modelName = "cube";
            tail.LoadContent();
            tail.body = new Box(pos + offSet, width, length, height, 1);
            tail.localTransform = Matrix.CreateScale(width, length, height);
            Game1.Instance.Space.Add(tail.body);
            Game1.Instance.Children.Add(tail);
            return tail;
        }
    }
}

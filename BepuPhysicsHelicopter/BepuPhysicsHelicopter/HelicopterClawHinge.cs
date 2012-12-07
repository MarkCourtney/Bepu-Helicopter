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
    public class HelicopterClawHinge : GameEntity
    {
        public BepuEntity clawHinge1, clawHinge2, clawHinge3, clawHinge4;
        
        public BepuEntity createClawHinge1(Vector3 position, Vector3 offSet, float width, float height, float length)
        {
            clawHinge1 = new BepuEntity();
            clawHinge1.modelName = "cube";
            clawHinge1.LoadContent();
            clawHinge1.body = new Box(position + offSet, width, height, length, 10);
            clawHinge1.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(clawHinge1.body);
            Game1.Instance.Children.Add(clawHinge1);
            return clawHinge1;
        }

        public BepuEntity createClawHinge2(Vector3 position, Vector3 offSet, float width, float height, float length)
        {
            clawHinge2 = new BepuEntity();
            clawHinge2.modelName = "cube";
            clawHinge2.LoadContent();
            clawHinge2.body = new Box(position + offSet, width, height, length, 10);
            clawHinge2.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(clawHinge2.body);
            Game1.Instance.Children.Add(clawHinge2);
            return clawHinge2;
        }


        public BepuEntity createClawHinge3(Vector3 position, Vector3 offSet, float width, float height, float length)
        {
            clawHinge3 = new BepuEntity();
            clawHinge3.modelName = "cube";
            clawHinge3.LoadContent();
            clawHinge3.body = new Box(position + offSet, width, height, length, 10);
            clawHinge3.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(clawHinge3.body);
            Game1.Instance.Children.Add(clawHinge3);
            return clawHinge3;
        }


        public BepuEntity createClawHinge4(Vector3 position, Vector3 offSet, float width, float height, float length)
        {
            clawHinge4 = new BepuEntity();
            clawHinge4.modelName = "cube";
            clawHinge4.LoadContent();
            clawHinge4.body = new Box(position + offSet, width, height, length, 10);
            clawHinge4.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(clawHinge4.body);
            Game1.Instance.Children.Add(clawHinge4);
            return clawHinge4;
        }
    }
}

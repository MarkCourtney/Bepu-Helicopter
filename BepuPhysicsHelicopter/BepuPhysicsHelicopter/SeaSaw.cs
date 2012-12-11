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
    public class seeSaw
    {
        public BepuEntity seeSawBoard, seeSawHolder, seeSawStopper;
        Random random = new Random();

        public BepuEntity createSeeSawBoard(Vector3 position, float width, float height, float length)
        {
            seeSawBoard = new BepuEntity();
            seeSawBoard.modelName = "cube";
            seeSawBoard.LoadContent();
            seeSawBoard.body = new Box(position, width, height, length, 1);
            seeSawBoard.localTransform = Matrix.CreateScale(width, height, length);
            seeSawBoard.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            Game1.Instance.Space.Add(seeSawBoard.body);
            Game1.Instance.Children.Add(seeSawBoard);
            return seeSawBoard;
        }

        public BepuEntity createSeeSawHolder(Vector3 position, float width, float height, float length)
        {
            seeSawHolder = new BepuEntity();
            seeSawHolder.modelName = "cube";
            seeSawHolder.LoadContent();
            seeSawHolder.body = new Box(position, width, height, length, 20);
            seeSawHolder.body.BecomeKinematic();
            seeSawHolder.localTransform = Matrix.CreateScale(width, height, length);
            seeSawHolder.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            Game1.Instance.Space.Add(seeSawHolder.body);
            Game1.Instance.Children.Add(seeSawHolder);
            return seeSawHolder;
        }

        public BepuEntity createSeeSawContainer(Vector3 position, float width, float height, float length)
        {
            seeSawStopper = new BepuEntity();
            seeSawStopper.modelName = "cube";
            seeSawStopper.LoadContent();
            seeSawStopper.body = new Box(position, width, height, length, 20);
            seeSawStopper.body.BecomeKinematic();
            seeSawStopper.localTransform = Matrix.CreateScale(width, height, length);
            seeSawStopper.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            Game1.Instance.Space.Add(seeSawStopper.body);
            Game1.Instance.Children.Add(seeSawStopper);
            return seeSawStopper;
        }
    }
}

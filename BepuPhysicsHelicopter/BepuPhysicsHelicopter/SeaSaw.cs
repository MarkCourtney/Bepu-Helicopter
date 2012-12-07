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
using BEPUphysics.MathExtensions;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.DataStructures;

namespace BepuPhysicsHelicopter
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SeaSaw
    {
        public BepuEntity seaSawBoard, seaSawHolder, seaSawStopper;
        Random random = new Random();

        public BepuEntity createSeaSawBoard(Vector3 position, float width, float height, float length)
        {
            seaSawBoard = new BepuEntity();
            seaSawBoard.modelName = "cube";
            seaSawBoard.LoadContent();
            seaSawBoard.body = new Box(position, width, height, length, 1);
            seaSawBoard.localTransform = Matrix.CreateScale(width, height, length);
            seaSawBoard.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            Game1.Instance.Space.Add(seaSawBoard.body);
            Game1.Instance.Children.Add(seaSawBoard);
            return seaSawBoard;
        }

        public BepuEntity createSeaSawHolder(Vector3 position, float width, float height, float length)
        {
            seaSawHolder = new BepuEntity();
            seaSawHolder.modelName = "cube";
            seaSawHolder.LoadContent();
            seaSawHolder.body = new Box(position, width, height, length, 1);
            seaSawHolder.body.BecomeKinematic();
            seaSawHolder.localTransform = Matrix.CreateScale(width, height, length);
            seaSawHolder.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            Game1.Instance.Space.Add(seaSawHolder.body);
            Game1.Instance.Children.Add(seaSawHolder);
            return seaSawHolder;
        }

        public BepuEntity createSeaSawStopper(Vector3 position, float width, float height, float length)
        {
            seaSawStopper = new BepuEntity();
            seaSawStopper.modelName = "cube";
            seaSawStopper.LoadContent();
            seaSawStopper.body = new Box(position, width, height, length, 1);
            seaSawStopper.body.BecomeKinematic();
            seaSawStopper.localTransform = Matrix.CreateScale(width, height, length);
            seaSawStopper.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            Game1.Instance.Space.Add(seaSawStopper.body);
            Game1.Instance.Children.Add(seaSawStopper);
            return seaSawStopper;
        }

        public BepuEntity createSeaSawContainer(Vector3 position, float width, float height, float length)
        {
            seaSawStopper = new BepuEntity();
            seaSawStopper.modelName = "cube";
            seaSawStopper.LoadContent();
            seaSawStopper.body = new Box(position, width, height, length, 1);
            seaSawStopper.body.BecomeKinematic();
            seaSawStopper.localTransform = Matrix.CreateScale(width, height, length);
            seaSawStopper.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            Game1.Instance.Space.Add(seaSawStopper.body);
            Game1.Instance.Children.Add(seaSawStopper);
            return seaSawStopper;
        }
    }
}

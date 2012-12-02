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
using BEPUphysics.Collidables.Events;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace BepuPhysicsHelicopter
{
    public class BepuEntity : GameEntity
    {
        public BEPUphysics.Entities.Entity body;
        AudioEmitter emitter = new AudioEmitter();
        AudioListener listener = new AudioListener();

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            worldTransform = body.WorldTransform;
        }

    }
}

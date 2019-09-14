using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace ShootShapesUp
{
    class EnemyBullet : Entity
    {
        public EnemyBullet(Texture2D Image, Vector2 position, Vector2 velocity)
        {
            image = Image;
            Position = position;
            Velocity = velocity;
            Radius = 8;
            Scale = 2f;
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            // delete bullets that go off-screen
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
    }
}

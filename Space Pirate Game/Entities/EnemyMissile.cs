using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class EnemyMissile : Entity
    {
        readonly float acceleration = 1f;
        readonly float max_velocity = 10f;
        int BoostFrames = 100;

        public EnemyMissile(Vector2 position)
        {
            image = GameRoot.Missile;
            Position = position;
            Orientation = Velocity.ToAngle();
            Velocity = (PlayerShip.Instance.Position - Position).ScaleTo(acceleration);
            Radius = 8;
            Scale = 2f;
        }

        public override void Update()
        {
            if (BoostFrames > 0)
            {
                Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration);

                if (Velocity.X > max_velocity) Velocity.X = max_velocity;
                if (Velocity.Y > max_velocity) Velocity.Y = max_velocity;
                if (Velocity.X < -max_velocity) Velocity.X = -max_velocity;
                if (Velocity.Y < -max_velocity) Velocity.Y = -max_velocity;
                Velocity = Velocity * 0.8f;
                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();
                BoostFrames--;
            }
            else
            {
                Orientation += 0.025f;
            }

            Position += Velocity;

            // delete bullets that go off-screen
            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class PlayerShip : Entity
    {
        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();

                return instance;
            }
        }
        static int cooldownFrames = 30;
        int cooldownRemaining = 0;
        int framesUntilRespawn = 0;
        public new bool IsDead { get { return framesUntilRespawn > 0; } }
        public int Lives;
        public bool IsOver;
        static public int HealthMax = 3, Health = HealthMax;
        bool LeftOrRight = false;
        static public int Exp, Level, NeededExp = 10;

        static Random rand = new Random();

        private PlayerShip()
        {
            image = GameRoot.Player;
            Position = new Vector2((GameRoot.ScreenSize.X / 2), (GameRoot.ScreenSize.Y / 2) + 100);
            Radius = 8;
            Lives = 3;
            Scale = 2f;
            Exp = 0;
            Level = 1;
        }

        public override void Update()
        {
            cooldownFrames = (60 - (Level * 5));
            if (Exp >= NeededExp)
            {
                if (Level == 10)
                {
                    Exp = NeededExp;
                }
                else
                {
                    Level += 1;
                    Exp = 0;
                }
            }

            if (Level % 2 == 0)
                HealthMax = 3 + Level / 2;

            if (Health > HealthMax)
                Health = HealthMax;

            if (IsDead)
            {
                --framesUntilRespawn;
                return;
            }

            var aim = Input.GetAimDirection();

            if (cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldownFrames;
                float aimAngle = aim.ToAngle();
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                Vector2 vel = 11f * new Vector2((float)Math.Cos(aimAngle), (float)Math.Sin(aimAngle));

                if (LeftOrRight == false)
                {
                    Vector2 offset = Vector2.Transform(new Vector2(40, -15), aimQuat);
                    EntityManager.Add(new PlayerBullet(Position + offset, vel));
                    LeftOrRight = true;
                }
                else
                {
                    Vector2 offset = Vector2.Transform(new Vector2(40, 15), aimQuat);
                    EntityManager.Add(new PlayerBullet(Position + offset, vel));
                    LeftOrRight = false;
                }

                GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;

            float speed = 0.5f;

            if (Input.GetMovementDirection() == Vector2.Zero)
            {
                if (Velocity.X > 1) Velocity.X = Velocity.X - speed;
                if (Velocity.Y > 1) Velocity.Y = Velocity.Y - speed;
                if (Velocity.X < -1) Velocity.X = Velocity.X + speed;
                if (Velocity.Y < -1) Velocity.Y = Velocity.Y + speed;
            }

            Velocity = Velocity + speed * Input.GetMovementDirection();

            float max_velocity = 15f;

            if (Velocity.X > max_velocity) Velocity.X = max_velocity;
            if (Velocity.Y > max_velocity) Velocity.Y = max_velocity;
            if (Velocity.X < -max_velocity) Velocity.X = -max_velocity;
            if (Velocity.Y < -max_velocity) Velocity.Y = -max_velocity;

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            Orientation = aim.ToAngle();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                base.Draw(spriteBatch);
        }

        public void LevelUp()
        {
            if (Level == 10)
            {
                Exp = NeededExp;
            }
            else
            {
                Level += 1;
                Exp = 0;
                cooldownFrames = (60 - (Level * 2));
            }
        }

        public void WasShot()
        {
            if (Health > 0)
            {
                Health = Health - 1;
            }
            if (Health == 0)
            {
                Kill();
                GameRoot.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);
            }
        }

        public void Kill()
        {
            Lives--;
            if (Lives == 0)
            {
                IsOver = true;
            }
            Health = HealthMax;

            EntityManager.enemies.ForEach(x => x.IsExpired = true);
            EntityManager.enemybullets.ForEach(x => x.IsExpired = true);
            EntityManager.enemymissiles.ForEach(x => x.IsExpired = true);
            EntityManager.playerbullets.ForEach(x => x.IsExpired = true);
            EntityManager.bossentities.ForEach(x => x.IsExpired = true);
            EntityManager.pickups.ForEach(x => x.IsExpired = true);

            EnemySpawner.Reset();
            framesUntilRespawn = 60;
            Position = new Vector2(GameRoot.ScreenSize.X / 2, GameRoot.ScreenSize.Y / 2);
            Exp = 0;
        }
    }
}

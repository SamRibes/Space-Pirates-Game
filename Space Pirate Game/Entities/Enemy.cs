using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class Enemy : Entity
    {
        public static Random rand = new Random();

        private List<IEnumerator<int>> EnemyTypes = new List<IEnumerator<int>>();
        public bool OnScreen;
        public int PointValue;
        public int Health;
        bool LeftorRight;
        public int Level;
        int cooldownFrames;
        int cooldownRemaining = 0;
        float Bullet_Velocity;

        public Enemy(Texture2D image, Vector2 position, int lvl)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            PointValue = 0;
            OnScreen = false;
            Level = lvl;
            if (Position.X < 0) { LeftorRight = false; } else { LeftorRight = true; }
        }

        public static Enemy CreateBasic(Vector2 position, int Level)
        {
            var enemy = new Enemy(GameRoot.Basic, position, Level);
            enemy.AddEnemyType(enemy.BasicEnemy());
            enemy.PointValue = 1;
            if (Level == 1)
            {
                enemy.Health = 1;
                enemy.cooldownFrames = 90;
            }
            else
            {
                enemy.Health = 2;
                enemy.cooldownFrames = 80;
            }
            return enemy;
        }

        public static Enemy CreateAsteroid(Vector2 position, int Level)
        {
            var enemy = new Enemy(GameRoot.Asteroid, position, Level);
            enemy.AddEnemyType(enemy.Asteroid());
            enemy.PointValue = 3;
            if (Level == 1)
            {
                enemy.Health = 3;
            }
            else
            {
                enemy.Health = 4;
            }
            return enemy;
        }

        public static Enemy CreateTwinShotEnemy(Vector2 position, int Level)
        {
            var enemy = new Enemy(GameRoot.TwinShot, position, Level);
            enemy.AddEnemyType(enemy.TwinShotEnemy());
            enemy.PointValue = 2;
            if (Level == 1)
            {
                enemy.Health = 2;
                enemy.cooldownFrames = 80;
            }
            else
            {
                enemy.Health = 3;
                enemy.cooldownFrames = 70;
            }
            return enemy;
        }

        public override void Update()
        {
            ApplyEnemyTypes();
            Position += Velocity;

            while (OnScreen == false)
            {
                if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                    OnScreen = true;
            }
        }

        public static Enemy CreateMissleEnemy(Vector2 position, int Level)
        {
            var enemy = new Enemy(GameRoot.MissileEnemy, position, Level);
            enemy.AddEnemyType(enemy.MissleEnemy());
            enemy.PointValue = 1;
            enemy.Health = 2;
            enemy.cooldownFrames = 300;
            return enemy;
        }

        private void AddEnemyType(IEnumerable<int> EnemyType)
        {
            EnemyTypes.Add(EnemyType.GetEnumerator());
        }

        private void ApplyEnemyTypes()
        {
            for (int i = 0; i < EnemyTypes.Count; i++)
            {
                if (!EnemyTypes[i].MoveNext())
                    EnemyTypes.RemoveAt(i--);
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
                WasKilled();
            }
        }

        public void WasKilled()
        {
            IsExpired = true;
            GameRoot.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);
            int chance = rand.Next(0, 5);
            if (chance == 0)
            {
                EntityManager.Add(PickUp.CreateHeart(Position));
            }
            else
            {
                switch (PointValue)
                {
                    case 1:
                        EntityManager.Add(PickUp.CreateSmallScrap(Position));
                        break;
                    case 2:
                        EntityManager.Add(PickUp.CreateMediumScrap(Position));
                        break;
                    case 3:
                        EntityManager.Add(PickUp.CreateLargeScrap(Position));
                        break;
                }
            }
        }

        #region EnemyTypes
        IEnumerable<int> BasicEnemy(float acceleration = 2.5f)
        {
            Scale = 2f;
            Bullet_Velocity = 2f;

            while (true)
            {
                if (LeftorRight == true)
                { Velocity.X = -acceleration; }
                else
                { Velocity.X = acceleration; }

                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                var aim = (PlayerShip.Instance.Position - Position);
                if (OnScreen)
                    if (cooldownRemaining <= 0)
                    {
                        GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
                        cooldownRemaining = cooldownFrames;
                        float aimAngle = aim.ToAngle();
                        Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                        Vector2 vel = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle), (float)Math.Sin(aimAngle));

                        Vector2 offset;
                        offset = Vector2.Transform(new Vector2(Radius, 0), aimQuat);
                        EntityManager.Add(new EnemyBullet(GameRoot.BasicBullet, Position + offset, vel));
                    }

                if (cooldownRemaining > 0)
                    cooldownRemaining--;
                yield return 0;
            }
        }

        IEnumerable<int> Asteroid(float acceleration = 2.5f)
        {
            Scale = 2f;

            while (true)
            {
                if (LeftorRight == true)
                { Velocity.X = -acceleration; }
                else
                { Velocity.X = acceleration; }

                Orientation += 0.025f;
                yield return 0;
            }
        }

        IEnumerable<int> TwinShotEnemy(float acceleration = 2.5f)
        {
            Scale = 2f;
            Bullet_Velocity = 2f;

            while (true)
            {
                if (LeftorRight == true)
                { Velocity.X = -acceleration; }
                else
                { Velocity.X = acceleration; }

                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                var aim = (PlayerShip.Instance.Position - Position);
                if (OnScreen)
                    if (cooldownRemaining <= 0)
                    {
                        GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
                        cooldownRemaining = cooldownFrames;
                        float aimAngle = aim.ToAngle();
                        Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0,
                        aimAngle);
                        Vector2 vel1 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + 0.5f), (float)Math.Sin(aimAngle + 0.5f));
                        Vector2 vel2 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle - 0.5f), (float)Math.Sin(aimAngle - 0.5f));
                        Vector2 offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                        EntityManager.Add(new EnemyBullet(GameRoot.TwinBullet, Position + offset, vel1));
                        offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                        EntityManager.Add(new EnemyBullet(GameRoot.TwinBullet, Position + offset, vel2));
                    }

                if (cooldownRemaining > 0)
                    cooldownRemaining--;
                yield return 0;
            }
        }

        IEnumerable<int> MissleEnemy(float acceleration = 2.5f)
        {
            Scale = 2f;

            while (true)
            {
                if (LeftorRight == true)
                { Velocity.X = -acceleration; }
                else
                { Velocity.X = acceleration; }

                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                var aim = (PlayerShip.Instance.Position - Position);
                if (OnScreen)
                    if (cooldownRemaining <= 0)
                    {
                        cooldownRemaining = cooldownFrames;
                        float aimAngle = aim.ToAngle();
                        Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                        Vector2 vel = 1 * new Vector2((float)Math.Cos(aimAngle), (float)Math.Sin(aimAngle));

                        Vector2 offset;
                        offset = Vector2.Transform(new Vector2(Radius, 0), aimQuat);
                        EntityManager.Add(new EnemyMissile(Position + offset));

                        GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
                    }

                if (cooldownRemaining > 0)
                    cooldownRemaining--;
                yield return 0;
            }
        }
        #endregion
    }
}

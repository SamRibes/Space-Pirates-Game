using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class Boss : Entity
    {
        public static Random rand = new Random();

        private List<IEnumerator<int>> BossTypes = new List<IEnumerator<int>>();
        public int Health;
        int cooldownFrames;
        int cooldownRemaining = 0;
        float Bullet_Velocity;
        int wait = 200;
        int ID;

        public Boss(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
        }

        public static Boss CreateBoss(Vector2 position)
        {
            var boss = new Boss(GameRoot.Boss_Body, position);
            boss.AddBossType(boss.Boss_Body_Entity());
            boss.Health = 60;
            boss.ID = 1;
            return boss;
        }

        public static Boss CreateBossTurret1(Vector2 position)
        {
            var boss = new Boss(GameRoot.Boss_Turret_1, position);
            boss.AddBossType(boss.BossTurret1());
            boss.Health = 30;
            return boss;
        }

        public static Boss CreateBossTurret2(Vector2 position)
        {
            var boss = new Boss(GameRoot.Boss_Turret_2, position);
            boss.AddBossType(boss.BossTurret2());
            boss.Health = 30;
            return boss;
        }

        public static Boss CreateBossTurret3(Vector2 position)
        {
            var boss = new Boss(GameRoot.Boss_Turret_1, position);
            boss.AddBossType(boss.BossTurret3());
            boss.Health = 30;
            return boss;
        }

        public static Boss CreateBossTurret4(Vector2 position)
        {
            var boss = new Boss(GameRoot.Boss_Turret_2, position);
            boss.AddBossType(boss.BossTurret4());
            boss.Health = 30;
            return boss;
        }

        public static Boss CreateDeadBoss(Vector2 position)
        {
            var boss = new Boss(GameRoot.Dead_Boss, position);
            boss.AddBossType(boss.DeadBoss());
            boss.Orientation = EntityManager.BossOrientation;
            boss.Health = 1000000;
            return boss;
        }


        public override void Update()
        {
            wait--;
            ApplyBossTypes();
            Position += Velocity;
            if (IsDead)
            {
                if (ID == 1) { EntityManager.Add(Boss.CreateDeadBoss(EntityManager.BossPosition)); }
                IsExpired = true;
            }
        }

        private void AddBossType(IEnumerable<int> BossType)
        {
            BossTypes.Add(BossType.GetEnumerator());
        }

        private void ApplyBossTypes()
        {
            for (int i = 0; i < BossTypes.Count; i++)
            {
                if (!BossTypes[i].MoveNext())
                    BossTypes.RemoveAt(i--);
            }
        }

        public void WasShot()
        {
            if(wait <= 0)
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
            
        }

        public void WasKilled()
        {
            IsDead = true;
            GameRoot.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);
        }

        #region BossTypes
        IEnumerable<int> Boss_Body_Entity()
        {
            float usedRadius = GameRoot.Boss_Body.Width / 1.5f;
            Scale = 2f;
            Bullet_Velocity = 5f;
            EntityManager.Add(Boss.CreateBossTurret1(new Vector2(Position.X + usedRadius, Position.Y)));
            EntityManager.Add(Boss.CreateBossTurret2(new Vector2(Position.X - usedRadius, Position.Y)));
            EntityManager.Add(Boss.CreateBossTurret3(new Vector2(Position.X, Position.Y + usedRadius)));
            EntityManager.Add(Boss.CreateBossTurret4(new Vector2(Position.X, Position.Y - usedRadius)));

            while (true)
            {
                Orientation += 0.005f;
                EntityManager.BossOrientation = Orientation;
                EntityManager.BossPosition = Position;
                yield return 0;
            }
        }

        IEnumerable<int> DeadBoss()
        {
            float usedRadius = GameRoot.Boss_Body.Width / 1.5f;
            Scale = 2f;

            while (true)
            {
                Orientation += 0.005f;
                EntityManager.BossOrientation = Orientation;
                EntityManager.BossPosition = Position;
                yield return 0;
            }
        }

        IEnumerable<int> BossTurret1()
        {
            Scale = 2f;
            cooldownFrames = 30;
            Bullet_Velocity = 5f;
            float radius = GameRoot.Boss_Turret_1.Width / 2;
            Texture2D img = GameRoot.BasicBullet;
            while (true)
            {
                Orientation -= 0.01f;
                float Angle = EntityManager.BossOrientation;
                Position.X = GameRoot.ScreenSize.X / 2 + (float)Math.Cos(Angle) * (GameRoot.Boss_Body.Width / 1.5f);
                Position.Y = GameRoot.ScreenSize.Y / 2 + (float)Math.Sin(Angle) * (GameRoot.Boss_Body.Width / 1.5f);

                if (wait <= 0)
                    if (cooldownRemaining <= 0)
                    {
                        GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
                        cooldownRemaining = cooldownFrames;
                        float aimAngle = Orientation + 1.57f;
                        Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                        Vector2 vel1 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle), (float)Math.Sin(aimAngle));
                        Vector2 vel2 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + 1.57f), (float)Math.Sin(aimAngle + 1.57f));
                        Vector2 vel3 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 2)), (float)Math.Sin(aimAngle + (1.57f * 2)));
                        Vector2 vel4 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 3)), (float)Math.Sin(aimAngle + (1.57f * 3)));

                        Vector2 offset = Vector2.Transform(new Vector2(radius, 0), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel1));
                        offset = Vector2.Transform(new Vector2(0, radius), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel2));
                        offset = Vector2.Transform(new Vector2(-radius, 0), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel3));
                        offset = Vector2.Transform(new Vector2(0, -radius), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel4));
                    }

                if (cooldownRemaining > 0)
                    cooldownRemaining--;
                yield return 0;
            }
        }
        IEnumerable<int> BossTurret2()
        {
            Scale = 2f;
            cooldownFrames = 100;
            Bullet_Velocity = 5f;
            float radius = GameRoot.Boss_Turret_1.Width / 2;
            Texture2D img = GameRoot.TwinBullet;
            while (true)
            {
                Orientation -= 0.01f;
                float Angle = EntityManager.BossOrientation + 1.57f;
                Position.X = GameRoot.ScreenSize.X / 2 + (float)Math.Cos(Angle) * (GameRoot.Boss_Body.Width / 1.5f);
                Position.Y = GameRoot.ScreenSize.Y / 2 + (float)Math.Sin(Angle) * (GameRoot.Boss_Body.Width / 1.5f);

                if (wait <= 0)
                    if (cooldownRemaining <= 0)
                    {
                        GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
                        cooldownRemaining = cooldownFrames;
                        float aimAngle = Orientation + 1.57f;
                        Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                        Vector2 vel1 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle), (float)Math.Sin(aimAngle));
                        Vector2 vel2 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f)), (float)Math.Sin(aimAngle + (1.57f)));
                        Vector2 vel3 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 2)), (float)Math.Sin(aimAngle + ((1.57f * 2))));
                        Vector2 vel4 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 3)), (float)Math.Sin(aimAngle + (1.57f * 3)));

                        Vector2 vel5 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f / 2)), (float)Math.Sin(aimAngle + (1.57f / 2)));
                        Vector2 vel6 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + 1.57f + (1.57f / 2)), (float)Math.Sin(aimAngle + 1.57f + (1.57f / 2)));
                        Vector2 vel7 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 2) + (1.57f / 2)), (float)Math.Sin(aimAngle + (1.57f * 2) + (1.57f / 2)));
                        Vector2 vel8 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 3) + (1.57f / 2)), (float)Math.Sin(aimAngle + (1.57f * 3) + (1.57f / 2)));

                        Vector2 offset = Vector2.Transform(new Vector2(radius, 0), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel1));
                        offset = Vector2.Transform(new Vector2(0, radius), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel2));
                        offset = Vector2.Transform(new Vector2(-radius, 0), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel3));
                        offset = Vector2.Transform(new Vector2(0, -radius), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel4));

                        offset = Vector2.Transform(new Vector2(radius / 1.5f, radius / 1.5f), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel5));
                        offset = Vector2.Transform(new Vector2(-radius / 1.5f, radius / 1.5f), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel6));
                        offset = Vector2.Transform(new Vector2(-radius / 1.5f, -radius / 1.5f), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel7));
                        offset = Vector2.Transform(new Vector2(radius / 1.5f, -radius / 1.5f), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel8));
                    }

                if (cooldownRemaining > 0)
                    cooldownRemaining--;
                yield return 0;
            }
        }
        IEnumerable<int> BossTurret3()
        {
            Scale = 2f;
            cooldownFrames = 30;
            Bullet_Velocity = 5f;
            float radius = GameRoot.Boss_Turret_1.Width / 2;
            Texture2D img = GameRoot.BasicBullet;
            while (true)
            {
                Orientation -= 0.01f;
                float Angle = EntityManager.BossOrientation + (1.57f * 2);
                Position.X = GameRoot.ScreenSize.X / 2 + (float)Math.Cos(Angle) * (GameRoot.Boss_Body.Width / 1.5f);
                Position.Y = GameRoot.ScreenSize.Y / 2 + (float)Math.Sin(Angle) * (GameRoot.Boss_Body.Width / 1.5f);

                if (wait <= 0)
                    if (cooldownRemaining <= 0)
                    {
                        GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
                        cooldownRemaining = cooldownFrames;
                        float aimAngle = Orientation + 1.57f;
                        Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                        Vector2 vel1 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle), (float)Math.Sin(aimAngle));
                        Vector2 vel2 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + 1.57f), (float)Math.Sin(aimAngle + 1.57f));
                        Vector2 vel3 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 2)), (float)Math.Sin(aimAngle + (1.57f * 2)));
                        Vector2 vel4 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 3)), (float)Math.Sin(aimAngle + (1.57f * 3)));

                        Vector2 offset = Vector2.Transform(new Vector2(radius, 0), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel1));
                        offset = Vector2.Transform(new Vector2(0, radius), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel2));
                        offset = Vector2.Transform(new Vector2(-radius, 0), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel3));
                        offset = Vector2.Transform(new Vector2(0, -radius), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel4));
                    }

                if (cooldownRemaining > 0)
                    cooldownRemaining--;
                yield return 0;
            }
        }
        IEnumerable<int> BossTurret4()
        {
            Scale = 2f;
            cooldownFrames = 100;
            Bullet_Velocity = 5f;
            float radius = GameRoot.Boss_Turret_1.Width / 2;
            Texture2D img = GameRoot.TwinBullet;
            while (true)
            {
                Orientation -= 0.01f;
                float Angle = EntityManager.BossOrientation + (1.57f * 3);
                Position.X = GameRoot.ScreenSize.X / 2 + (float)Math.Cos(Angle) * (GameRoot.Boss_Body.Width / 1.5f);
                Position.Y = GameRoot.ScreenSize.Y / 2 + (float)Math.Sin(Angle) * (GameRoot.Boss_Body.Width / 1.5f);

                if (wait <= 0)
                    if (cooldownRemaining <= 0)
                    {
                        GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
                        cooldownRemaining = cooldownFrames;
                        float aimAngle = Orientation + 1.57f;
                        Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                        Vector2 vel1 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle), (float)Math.Sin(aimAngle));
                        Vector2 vel2 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f)), (float)Math.Sin(aimAngle + (1.57f)));
                        Vector2 vel3 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 2)), (float)Math.Sin(aimAngle + ((1.57f * 2))));
                        Vector2 vel4 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 3)), (float)Math.Sin(aimAngle + (1.57f * 3)));

                        Vector2 vel5 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f / 2)), (float)Math.Sin(aimAngle + (1.57f / 2)));
                        Vector2 vel6 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + 1.57f + (1.57f / 2)), (float)Math.Sin(aimAngle + 1.57f + (1.57f / 2)));
                        Vector2 vel7 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 2) + (1.57f / 2)), (float)Math.Sin(aimAngle + (1.57f * 2) + (1.57f / 2)));
                        Vector2 vel8 = Bullet_Velocity * new Vector2((float)Math.Cos(aimAngle + (1.57f * 3) + (1.57f / 2)), (float)Math.Sin(aimAngle + (1.57f * 3) + (1.57f / 2)));

                        Vector2 offset = Vector2.Transform(new Vector2(radius, 0), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel1));
                        offset = Vector2.Transform(new Vector2(0, radius), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel2));
                        offset = Vector2.Transform(new Vector2(-radius, 0), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel3));
                        offset = Vector2.Transform(new Vector2(0, -radius), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel4));

                        offset = Vector2.Transform(new Vector2(radius / 1.5f, radius / 1.5f), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel5));
                        offset = Vector2.Transform(new Vector2(-radius / 1.5f, radius / 1.5f), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel6));
                        offset = Vector2.Transform(new Vector2(-radius / 1.5f, -radius / 1.5f), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel7));
                        offset = Vector2.Transform(new Vector2(radius / 1.5f, -radius / 1.5f), aimQuat);
                        EntityManager.Add(new EnemyBullet(img, Position + offset, vel8));
                    }

                if (cooldownRemaining > 0)
                    cooldownRemaining--;
                yield return 0;
            }
            #endregion
        }
    }
}

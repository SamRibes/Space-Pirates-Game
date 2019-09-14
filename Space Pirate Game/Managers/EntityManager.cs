using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    static class EntityManager
    {
        public static List<Entity> entities = new List<Entity>();
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<PlayerBullet> playerbullets = new List<PlayerBullet>();
        public static List<EnemyBullet> enemybullets = new List<EnemyBullet>();
        public static List<EnemyMissile> enemymissiles = new List<EnemyMissile>();
        public static List<Boss> bossentities = new List<Boss>();
        public static List<PickUp> pickups = new List<PickUp>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

        public static int Count { get { return entities.Count; } }

        public static Vector2 BossPosition;
        public static float BossOrientation;
        public static Random rand = new Random();

        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is PlayerBullet)
                playerbullets.Add(entity as PlayerBullet);
            else if (entity is Enemy)
                enemies.Add(entity as Enemy);
            else if (entity is EnemyBullet)
                enemybullets.Add(entity as EnemyBullet);
            else if (entity is EnemyMissile)
                enemymissiles.Add(entity as EnemyMissile);
            else if (entity is Boss)
                bossentities.Add(entity as Boss);
            else if (entity is PickUp)
                pickups.Add(entity as PickUp);
        }

        public static void Update()
        {
            isUpdating = true;
            HandleCollisions();

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

            entities = entities.Where(x => !x.IsExpired).ToList();
            playerbullets = playerbullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();
            enemybullets = enemybullets.Where(x => !x.IsExpired).ToList();
            enemymissiles = enemymissiles.Where(x => !x.IsExpired).ToList();
            pickups = pickups.Where(x => !x.IsExpired).ToList();
            bossentities = bossentities.Where(x => !x.IsExpired).ToList();
        }

        static void HandleCollisions()
        {
            // handle collisions between bullets and enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < playerbullets.Count; j++)
                {
                    if (IsColliding(enemies[i], playerbullets[j]))
                    {
                        enemies[i].WasShot();
                        playerbullets[j].IsExpired = true;
                    }
                }

            // handle collisions between bullets and enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < playerbullets.Count; j++)
                {
                    if (IsColliding(enemies[i], playerbullets[j]))
                    {
                        enemies[i].WasShot();
                        playerbullets[j].IsExpired = true;
                    }
                }

            // handle collisions between enemy bullets and player
            for (int i = 0; i < enemybullets.Count; i++)
            {
                if (IsColliding(enemybullets[i], PlayerShip.Instance))
                {
                    enemybullets[i].IsExpired = true;
                    PlayerShip.Instance.WasShot();
                    break;
                }
            }

            // handle collisions between the player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                if (IsColliding(PlayerShip.Instance, enemies[i]))
                {
                    PlayerShip.Instance.WasShot();
                    enemies[i].IsExpired = true;
                    break;
                }
            }

            // handle collisions between the player and pickups
            for (int i = 0; i < pickups.Count; i++)
            {
                if (IsColliding(PlayerShip.Instance, pickups[i]))
                {
                    switch (pickups[i].GetPointValue())
                    {
                        case 1:
                            PlayerShip.Exp += 1;
                            break;
                        case 2:
                            PlayerShip.Exp += 2;
                            break;
                        case 3:
                            PlayerShip.Exp += 3;
                            break;
                        case 4:
                            PlayerShip.Exp += 4;
                            break;
                        case 5:
                            PlayerShip.Health++;
                            break;
                    }
                    pickups[i].IsExpired = true;

                }
            }

            // handle collisions between the player and Boss parts
            for (int i = 0; i < bossentities.Count; i++)
            {
                if (IsColliding(PlayerShip.Instance, bossentities[i]))
                {
                    PlayerShip.Instance.WasShot();
                }
            }

            // handle collisions between the player bullets and Boss parts
            for (int i = 0; i < bossentities.Count; i++)
                for (int j = 0; j < playerbullets.Count; j++)
                {
                    if (IsColliding(bossentities[i], playerbullets[j]))
                    {
                        bossentities[i].WasShot();
                        playerbullets[j].IsExpired = true;
                    }
                }

            // handle collisions between the player and enemy missiles
            for (int i = 0; i < enemymissiles.Count; i++)
            {
                if (IsColliding(PlayerShip.Instance, enemymissiles[i]))
                {
                    PlayerShip.Instance.WasShot();
                    GameRoot.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);
                    EntityManager.enemymissiles[i].IsExpired = true;
                }
            }
        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }
    }
}

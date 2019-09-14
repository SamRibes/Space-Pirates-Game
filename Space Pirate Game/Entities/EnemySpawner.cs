using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    static class EnemySpawner
    {
        static Random rand = new Random();
        static float inverseSpawnChance = 300;
        static bool BossSpawned = false;

        public static void Update(int Stage)
        {
            switch (Stage)
            {
                case 1:
                    if (!PlayerShip.Instance.IsDead && EntityManager.enemies.Count < 20)
                    {

                        if (inverseSpawnChance > 15)
                            inverseSpawnChance -= 0.005f;
                        if (rand.Next((int)inverseSpawnChance) == 0)
                        {
                            EntityManager.Add(Enemy.CreateBasic(GetSpawnPosition(), Stage));
                        }

                        if (rand.Next((int)inverseSpawnChance) == 0)
                        {
                            EntityManager.Add(Enemy.CreateAsteroid(GetSpawnPosition(), Stage));
                        }

                        if (rand.Next((int)inverseSpawnChance) == 0)
                        {
                            EntityManager.Add(Enemy.CreateTwinShotEnemy(GetSpawnPosition(), Stage));
                        }
                    }
                    break;
                case 2:
                    if (inverseSpawnChance > 10)
                        inverseSpawnChance -= 0.05f;
                    if (!PlayerShip.Instance.IsDead && EntityManager.enemies.Count < 20)
                    {
                        if (inverseSpawnChance > 20)
                            inverseSpawnChance -= 0.005f;
                        if (rand.Next((int)inverseSpawnChance) == 0)
                        {
                            EntityManager.Add(Enemy.CreateBasic(GetSpawnPosition(), Stage));
                        }

                        if (rand.Next((int)inverseSpawnChance) == 0)
                        {
                            EntityManager.Add(Enemy.CreateAsteroid(GetSpawnPosition(), Stage));
                        }

                        if (rand.Next((int)inverseSpawnChance) == 0)
                        {
                            EntityManager.Add(Enemy.CreateTwinShotEnemy(GetSpawnPosition(), Stage));
                        }

                        if (rand.Next((int)inverseSpawnChance) == 0)
                        {
                            EntityManager.Add(Enemy.CreateMissleEnemy(GetSpawnPosition(), Stage));
                        }
                    }
                    break;

                case 3:
                    if (!PlayerShip.Instance.IsDead)
                    {
                        if (BossSpawned == false)
                        {
                            EntityManager.Add(Boss.CreateBoss(new Vector2(GameRoot.ScreenSize.X / 2, GameRoot.ScreenSize.Y / 2)));
                            BossSpawned = true;
                        }
                    }
                    break;
            }
        }

        private static Vector2 GetSpawnPosition()
        {
            bool LeftorRight = false;
            if (rand.Next(0, 2) == 0) { LeftorRight = false; } else { LeftorRight = true; }
            Vector2 pos;

            if (LeftorRight == false)
            {
                pos.X = -100;
            }
            else
            {
                pos.X = (int)GameRoot.ScreenSize.X + 100;
            }

            pos.Y = rand.Next((int)GameRoot.ScreenSize.Y);

            return pos;
        }
        public static void Reset()
        {
            inverseSpawnChance = 300;
            BossSpawned = false;
        }
    }
}

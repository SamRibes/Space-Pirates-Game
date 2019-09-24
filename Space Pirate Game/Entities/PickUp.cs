using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class PickUp : Entity
    {
        private List<IEnumerator<int>> PickUpTypes = new List<IEnumerator<int>>();
        public int FramesTillDespawn;
        public static int PointValue;

        public PickUp(Texture2D image, Vector2 position, int pointValue)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            FramesTillDespawn = 300;
            PointValue = pointValue;
        }

        public int GetPointValue()
        {
            return PointValue;
        }

        public static PickUp CreateSmallScrap(Vector2 position)//1 Scrap Point
        {
            var pickup = new PickUp(GameRoot.Coin1, position, 1);
            pickup.AddPickUpType(pickup.SmallScrap());
            return pickup;
        }
        public static PickUp CreateMediumScrap(Vector2 position)//2 Scrap Points
        {
            var pickup = new PickUp(GameRoot.Coin2, position, 2);
            pickup.AddPickUpType(pickup.MediumScrap());
            return pickup;
        }
        public static PickUp CreateLargeScrap(Vector2 position)//3 Scrap Points
        {
            var pickup = new PickUp(GameRoot.Coin3, position, 3);
            pickup.AddPickUpType(pickup.LargeScrap());
            return pickup;
        }
        public static PickUp CreateMassiveScrap(Vector2 position)//4 Scrap Points
        {
            var pickup = new PickUp(GameRoot.Coin4, position, 4);
            pickup.AddPickUpType(pickup.MassiveScrap());
            return pickup;
        }
        public static PickUp CreateHeart(Vector2 position)//4 Scrap Points
        {
            var pickup = new PickUp(GameRoot.Heart, position, 5);
            pickup.AddPickUpType(pickup.Heart());
            return pickup;
        }

        public override void Update()
        {
            ApplyPickUpTypes();
            FramesTillDespawn--;
            if (FramesTillDespawn == 0)
                IsExpired = true;

            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }

        private void AddPickUpType(IEnumerable<int> PickUpType)
        {
            PickUpTypes.Add(PickUpType.GetEnumerator());
        }

        private void ApplyPickUpTypes()
        {
            for (int i = 0; i < PickUpTypes.Count; i++)
            {
                if (!PickUpTypes[i].MoveNext())
                    PickUpTypes.RemoveAt(i--);
            }
        }

        #region Pick up types
        IEnumerable<int> SmallScrap()
        {
            Scale = 2f;
            while (true)
            {
                Orientation += 0.025f;
                yield return 0;
            }
        }
        IEnumerable<int> MediumScrap()
        {
            Scale = 2f;
            while (true)
            {
                Orientation += 0.025f;
                yield return 0;
            }
        }
        IEnumerable<int> LargeScrap()
        {
            Scale = 2f;
            while (true)
            {
                Orientation += 0.025f;
                yield return 0;
            }
        }
        IEnumerable<int> MassiveScrap()
        {
            Scale = 2f;
            while (true)
            {
                Orientation += 0.025f;
                yield return 0;
            }
        }
        IEnumerable<int> Heart()
        {
            Scale = 2f;
            while (true)
            {
                Orientation += 0.025f;
                yield return 0;
            }
        }
        #endregion

    }
}

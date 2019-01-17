using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameProject
{
    class Map
    {
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        private List<NonCollidable> NCT = new List<NonCollidable>();
        private List<torches> ATS = new List<torches>();
        private List<Chests> Chests = new List<Chests>();

        public List<CollisionTiles> CollisionTiles
        {
            get { return collisionTiles; }
        }

        private int[,] m_mapmain;
        private int m_chestlimit;

        private int width, height;
        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public Map()
        {
        }

        public void Generate(int[,] map, int size, ContentManager Content, int chestlimit)
        {
            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    if (number > 0)
                        collisionTiles.Add(new CollisionTiles(number, new Rectangle(x * size, y * size, size, size)));

                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }

            m_mapmain = map;
            m_chestlimit = chestlimit;

            ChestGenerate(map, size, Content, chestlimit);
        }

        private void ChestGenerate(int[,] map, int size, ContentManager Content, int chestlimit)
        {
            for (int i = chestlimit; i > 0; )
                for (int x = 0; x < map.GetLength(1); x++)
                    for (int y = 0; y < map.GetLength(0); y++)
                    {
                        if (map[y, x] != 0)
                        {
                            if (y - 1 >= 0)
                                if (map[y - 1, x] == 0)
                                {
                                    if (i > 0)
                                    {
                                        if (Game1.RNG.Next(0, map.GetLength(1) * 2) == 0)
                                        {
                                            Chests.Add(new Chests(Content.Load<Texture2D>("Chest"), new Rectangle(x * size, (y - 1) * 32, size, size), 12, 3, Content.Load<Texture2D>("lightaura")));
                                            i--;
                                            for (int o = 0; o < Chests.Count - 1; o++)
                                            {
                                                BoundingSphere temp = new BoundingSphere(new Vector3(Chests[o].Position, 0), (width / map.GetLength(0)) * 4);
                                                if (temp.Contains(new Vector3(Chests[Chests.Count - 1].rect.Center.X, Chests[Chests.Count - 1].rect.Center.Y, 0)) == ContainmentType.Contains)
                                                {
                                                    Chests.RemoveAt(Chests.Count - 1);
                                                    i++;
                                                    o--;
                                                }
                                            }
                                        }
                                    }
                                }
                        }
                    }
        }

        public void genBackground(int[,] map, int size)
        {
            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    if (number > 0)
                        NCT.Add(new NonCollidable(number, new Rectangle(x * size, y * size, size, size)));

                    //width = (x + 1) * size;
                    //height = (y + 1) * size;
                }
        }




        public void updateme(GameTime gt, ContentManager Content, List<PlayerClass> playera, GameMode gamemode)
        {

            for (int i = 0; i < Chests.Count; i++)
            {
                Chests[i].update(gt, Content, playera);
            }

            for (int x = 0; x < playera.Count; x++)
            {
                for (int y = 0; y < playera.Count; y++)
                {
                    for (int i = 0; i < playera[x].Projectiles.Count; i++)
                    {
                        if (y != x)
                        {
                            if (playera[x].Projectiles[i].Collision.Intersects(playera[y].CollBox) && playera[x].Projectiles[i].Active && playera[y].Alive)
                            {
                                playera[y].Alive = false;
                                playera[x].Projectiles[i].vel = new Vector2(Game1.RNG.Next(-1, 2), Game1.RNG.Next(-3, 0));
                                playera[x].Projectiles[i].vel.Normalize();
                            }

                        }
                        if (playera[y].CollBox.Intersects(playera[x].Projectiles[i].Collision))
                        {
                            if (playera[x].Projectiles[i].Active == false)
                            {

                                playera[y].Quiver.add(Content);
                                playera[x].Projectiles.RemoveAt(i);
                                i--;
                            }

                        }
                    }
                }
            }

            //int temp = 0;
            //for (int i = 0; i < Chests.Count; i++)
            //{
            //    if (Chests[i].state == true)
            //        temp++;
            //}
            //if (temp == Chests.Count)
            //{
            //    for (int i = 0; i < Chests.Count; i++)
            //        Chests.RemoveAt(0);
            //    ChestGenerate(m_mapmain, width / m_mapmain.GetLength(1), Content, m_chestlimit);

            //}

            for (int i = 0; i < ATS.Count; i++)
            {
                ATS[i].updateTiles(gt);
                ATS[i].updateme(gt, gamemode);
            }


        }

        public void genAnimated(int[,] map, int size)
        {
            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int numberAnim = map[y, x];

                    if (numberAnim > 0)
                        ATS.Add(new torches(numberAnim, new Rectangle(x * size, y * size, size, size), 5, 5, Color.White));

                    //width = (x + 1) * size;
                    //height = (y + 1) * size;
                }
        }


        public void DrawNCT(SpriteBatch sb)
        {
            for (int i = 0; i < NCT.Count; i++)
                NCT[i].Draw(sb);
        }
        public void DrawATS(SpriteBatch sb)
        {
            for (int i = 0; i < ATS.Count; i++)
                ATS[i].Draw(sb);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            DrawNCT(spriteBatch);
            DrawATS(spriteBatch);
            foreach (CollisionTiles tile in collisionTiles)
                tile.Draw(spriteBatch);
            for (int i = 0; i < Chests.Count; i++)
                Chests[i].drawme(spriteBatch);
        }


        public void DrawMaskChest(SpriteBatch sb)
        {
            for (int i = 0; i < Chests.Count; i++)
            {
                Chests[i].Drawmask(sb);

            }
            for (int i = 0; i < ATS.Count; i++)
                ATS[i].DrawMask(sb);

        }
    }
}

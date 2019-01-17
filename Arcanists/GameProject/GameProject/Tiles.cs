using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameProject
{
    class Tiles
    {
        protected Texture2D texture;

        public Texture2D txr
        {
            get
            {
            return texture;
            }
            set
            {
             texture = value;
            }
        }
        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }
        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }

    class CollisionTiles : Tiles
    {
        public CollisionTiles(int i, Rectangle newRectangle)
        {
            texture = Content.Load<Texture2D>("sTiles//sTile" + i);
            this.Rectangle = newRectangle;
        }
    }

    class NonCollidable : Tiles
    {   
        public NonCollidable(int i, Rectangle newRectangle)
        {
            texture = Content.Load<Texture2D>("bTiles//bTile" + i);
            this.Rectangle = newRectangle;
        }
    }

    class AnimatedTiles : Tiles
    {
        protected Rectangle m_srcRect;
        protected float m_updateTrigger;
        protected int m_framesPerSecond;
        protected int m_frames;


        public AnimatedTiles(int i, Rectangle newRectangle, int fps, int HowManyFrames)
        {

            texture = Content.Load<Texture2D>("AnimatedTile" + i);
            m_frames = HowManyFrames;
            m_framesPerSecond = fps;
            m_updateTrigger = 0;
            m_srcRect = new Rectangle(0, 0, texture.Width/m_frames, texture.Height);

            Rectangle = newRectangle;
        }

        public void updateTiles(GameTime gt)
        {
            m_updateTrigger += (float)gt.ElapsedGameTime.TotalSeconds * m_framesPerSecond;

            if (m_updateTrigger >= 1)
            {
                m_updateTrigger = 0;
                m_srcRect.X += m_srcRect.Width;
                if (m_srcRect.X == texture.Width)
                    m_srcRect.X = 0;
            }

        }
        

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, m_srcRect, Color.White);
        }

    }

    


    class torches : AnimatedTiles
    {
        public static Texture2D m_aura;
        private Color m_tint;
        private float m_radius;
        public static readonly Random RNG = new Random();
        private float m_timer;

        public torches(int i, Rectangle newRectangle, int fps, int HowManyFrames,Color tint)
            :base(i,newRectangle, fps, HowManyFrames)
        {
            m_tint = tint;
            m_radius = 1f;
            m_timer = 0.5f;
           
        }

        public void updateme(GameTime gt, GameMode gamemode)
        {
            switch (gamemode)
            {
                case GameMode.normal:
                    m_radius = 80f;
                    break;
                case GameMode.oneLightInTheDark:
                    m_radius = 1f;
                    break;
                case GameMode.strobe:
                    m_timer -= (float)gt.ElapsedGameTime.TotalSeconds;
                    if (m_timer < 0)
                    {
                        m_radius = 5f;
                        m_tint = new Color(Game1.RNG.Next(0, 255), Game1.RNG.Next(0, 255), Game1.RNG.Next(0, 255));
                        m_timer = 0.5f;
                    }
                    break;
            }



            base.updateTiles(gt);
        }


        public void DrawMask(SpriteBatch sb)
        {
            sb.Draw(m_aura,
                new Vector2(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2),null,
                m_tint * 1f, 0f,
                new Vector2(m_aura.Width / 2, m_aura.Height / 2),m_radius,
                SpriteEffects.None, 0f);
        }


    }


    class CrumbleTiles : Tiles
    {   
    }

    class TimedTiles : Tiles
    {


    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    enum playerState
    {
        facingRight,
        facingLeft
    }

    class PlayerClass : LitSprite
    {
        private bool hasJumped = true;
        private PlayerIndex m_playerIndex;
        private playerState m_pState;
        protected bool triggerdown;
        protected List <Projectile> m_Projectile;
        public List<Projectile> Projectiles
        {
            get { return m_Projectile; }
        }
        protected Quiver m_Quiver;
        public Quiver Quiver
        {
            get
            {
                return m_Quiver;
            }
            set
            {
                m_Quiver = value;
            }
        }
        protected RotationInd m_Arrow;
        protected int m_currentPower;
        protected bool m_timerStart;
        protected int m_scale;
        public int scale
        {
            get
            {
                return m_scale;
            }
            set
            {
                m_scale = value;
            }

        }
        public bool timerStart
        {
            get
            {
                return m_timerStart;
            }
            set
            {
                m_timerStart = value;
            }

        }
        public int currentPower
            {
                get
                {
                    return m_currentPower;
                }
                set
                {
                    m_currentPower = value;
                }
            }
        private float m_timer;
        public BoundingBox CollBox
        {
            get { return new BoundingBox(new Vector3(m_rect.X, m_rect.Y, 0), new Vector3(m_rect.X + m_rect.Width, m_rect.Y + m_rect.Height, 0)); }
        }

        public bool Alive { get; set; }
        private Color m_tint;

        public PlayerClass(Texture2D txr, Rectangle rect, int fps, int HowManyframes, ContentManager Content,  Color tint, Texture2D lightaura)
            :base(txr, rect, fps, HowManyframes, lightaura)
        {          
            m_Projectile = new List<Projectile>();
            m_Quiver = new Quiver(Content.Load<Texture2D>("EmptyOrb"), new Rectangle(m_rect.X + 5, m_rect.Y - 12, 40, 12), Content);
            m_currentPower = -1;
            Alive = true;
            m_tint = tint;
        }

        public void updatePlayer(GameTime gt, PlayerIndex playerindex, ContentManager Content, int charSelect, Rectangle screensize)
        {

            if (Alive == true)
            {
                switch (charSelect)
                {
                    case 0:
                        m_tint = Color.CornflowerBlue;
                        m_Arrow = new RotationInd(Content.Load<Texture2D>("Player3Aim"), new Rectangle(0, 0, 50, 50));
                        break;
                    case 1:
                        m_tint = Color.GreenYellow;
                        m_Arrow = new RotationInd(Content.Load<Texture2D>("Player1Aim"), new Rectangle(0, 0, 50, 50));
                        break;
                    case 2:
                        m_tint = Color.Red;
                        m_Arrow = new RotationInd(Content.Load<Texture2D>("Player2Aim"), new Rectangle(0, 0, 50, 50));
                        break;
                    case 3:
                        m_tint = Color.Purple;
                        m_Arrow = new RotationInd(Content.Load<Texture2D>("Player4Aim"), new Rectangle(0, 0, 50, 50));
                        break;
                }








                m_Arrow.Position = new Vector2(m_rect.Center.X, m_rect.Center.Y);






                if (m_timer >= 5)
                {
                    reset();
                }

                if (m_timerStart == true)
                {
                    m_timer += (float)gt.ElapsedGameTime.TotalSeconds;
                }

                m_Arrow.updateme(gt, playerindex);
                m_Quiver.Position = new Vector2(m_rect.X + 5, m_rect.Y - 12);
                m_Quiver.updateme(gt);
                
                m_pos += m_vel;

                m_rect.X = (int)m_pos.X;
                m_rect.Y = (int)m_pos.Y;

                if (m_vel.Y > 1)
                    hasJumped = true;

                m_playerIndex = playerindex;
                if (Alive)
                    Input(gt, Content);
                m_updateTrigger += (float)gt.ElapsedGameTime.TotalSeconds * m_framesPerSecond;

                if (m_updateTrigger >= 1)
                {
                    m_updateTrigger = 0;
                    m_srcRect.X += m_srcRect.Width;
                    if (m_srcRect.X == m_txr.Width)
                        m_srcRect.X = 0;
                }

                if (m_vel.Y < 15)
                    m_vel.Y += 0.4f;
            }

            for (int i = 0; i < m_Projectile.Count; i++)
            {
                m_Projectile[i].updateme(gt, screensize);
            }
        }

     

        public void reset()
        {        
                if (m_scale == 0)
                {
                    rect = new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height / 2);
                }
                else if (m_scale == 1)
                {
                    rect = new Rectangle(rect.X, rect.Y, rect.Width * 2, rect.Height * 2);
                    
                }
                m_currentPower = -1;
                m_timer = 0;
                
                m_timerStart = false;           
        }

        private void Input(GameTime gameTime, ContentManager Content)
        {
            if (GamePad.GetState(m_playerIndex).ThumbSticks.Left.X > 0)
            {
                m_pState = playerState.facingRight;
                m_vel.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            }
            else if (GamePad.GetState(m_playerIndex).ThumbSticks.Left.X < 0)
            {
                m_pState = playerState.facingLeft;
                m_vel.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            }
            else m_vel.X = 0f;



            if (GamePad.GetState(m_playerIndex).Triggers.Right > 0f && triggerdown == false && m_Quiver.ammo.Count > 0 && (GamePad.GetState(m_playerIndex).ThumbSticks.Right.X != 0 || GamePad.GetState(m_playerIndex).ThumbSticks.Right.Y != 0))
            {
                Vector2 direction = new Vector2(m_Arrow.Position.X - m_rect.X, m_Arrow.Position.Y - m_rect.Y);
                direction = Vector2.Normalize(direction) * 300;
                m_Projectile.Add(new Projectile(Content.Load<Texture2D>("Missile"), new Rectangle(m_rect.Center.X, m_rect.Center.Y, (m_rect.Width / 3) * 2, (10 + (m_rect.Height / 3) * 2)), 5, 1, m_Arrow.Rotation,m_tint, Content.Load<Texture2D>("lightaura")));
                m_Quiver.ammo.Remove(m_Quiver.ammo[m_Quiver.ammo.Count - 1]);
                triggerdown = true;
            }
            else if (GamePad.GetState(m_playerIndex).Triggers.Right == 0f)
            {
                triggerdown = false;
            }

            if (GamePad.GetState(m_playerIndex).Buttons.A == ButtonState.Pressed && hasJumped == false)
            {
                m_pos.Y -= 5f;
                m_vel.Y = -9f;
                hasJumped = true;
            }

        }

        public void Collision(Rectangle newRectangle, Rectangle screenSize, int xOffset, int yOffset, ContentManager Content)
        {
            if (m_pos.Y + m_rect.Height < 0)
                m_pos.Y = screenSize.Height - 1;
            if (m_pos.Y > screenSize.Height)
                m_pos.Y = 1 - m_rect.Height;
            if (m_pos.X + m_rect.Width < 0)
                m_pos.X = screenSize.Width - 1;
            if (m_pos.X > screenSize.Width)
                m_pos.X = 1 - m_rect.Width;


            //if top of player rectangle touches bottom of tiles
            if (m_rect.TouchTopOf(newRectangle))
            {
                //reset position of player and stop moving up
                m_rect.Y = newRectangle.Y - m_rect.Height;
                m_vel.Y = 0f;
                hasJumped = false;
            }

            #region Projectile Colission
            for (int i = 0; i < m_Projectile.Count; i++)
            {
                if (m_Projectile[i].Active && m_Projectile[i].Collision.Intersects(new BoundingBox(new Vector3(newRectangle.X, newRectangle.Y, 0), new Vector3(newRectangle.X + newRectangle.Width, newRectangle.Y + newRectangle.Height, 0))))
                {
                    m_Projectile[i].vel = Vector2.Zero;
                    m_Projectile[i].Active = false;
                }


                

            }
            #endregion

            if (GamePad.GetState(m_playerIndex).ThumbSticks.Left.X > 0)
            {
                

             //if left of player rectangle touches right of tiles
                if (m_rect.TouchleftOf(newRectangle))
                {
                    m_vel.X = 0;
                }
            }


            if (GamePad.GetState(m_playerIndex).ThumbSticks.Left.X < 0)
            {
                //if right of player rectangle touches right of tiles
                if (m_rect.TouchRightOf(newRectangle))
                {
                    //reset position of player
                    m_pos.X = newRectangle.X + newRectangle.Width + 1;
                }
            }


            //if bottom of player rectangle touches top of tiles
            if (m_rect.TouchBottomOf(newRectangle))
                //stop velocity
                m_vel.Y = 1f;
        }

        public override void drawme(SpriteBatch sb)
        {
            for (int i = 0; i < m_Projectile.Count; i++)
            {
                m_Projectile[i].drawme(sb);
            }

            if (Alive)
            {
                m_Quiver.drawme(sb);
                m_Arrow.drawme(sb);

                if (m_pState == playerState.facingRight)
                {
                    sb.Draw(m_txr, m_rect, m_srcRect, Color.White);
                }

                if (m_pState == playerState.facingLeft)
                {
                    sb.Draw(m_txr, m_rect, m_srcRect, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
                //sb.DrawString(Game1.debug, "" + (int)m_timer, new Vector2(m_rect.X, m_rect.Y), Color.White);
            }
        }


        public override void  Drawmask(SpriteBatch sb)
        {
            
                for (int i = 0; i < m_Projectile.Count; i++)
                {
                    m_Projectile[i].Drawmask(sb);
                }
            
            base.Drawmask(sb);
        }

    }
}

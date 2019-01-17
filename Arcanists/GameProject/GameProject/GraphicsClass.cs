using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    class BasicGraphic
    {
        protected  Texture2D m_txr;

        public Texture2D sTxr
        {
            get
            {
                return m_txr;
            }
            set
            {
                m_txr = value;
            }

        }


        protected  Rectangle m_rect;

        public Rectangle rect
        {
            get
            {
                return m_rect;
            }
            set
            {
                m_rect = value;
            }

        }


        public BasicGraphic(Texture2D txr, Rectangle rect)
        {
            m_txr = txr;
            m_rect = rect;
        }

        public BasicGraphic(Texture2D txr, int xPos, int yPos, int width, int height)
            : this(txr, new Rectangle(xPos, yPos, width, height))
        {

        }

        public virtual void drawme(SpriteBatch sb)
        {
            sb.Draw(m_txr, m_rect, Color.White);
        }
    }

    class MovingGraphic : BasicGraphic
    {
        protected Vector2 m_pos;
        protected Vector2 m_vel;

        public Vector2 vel
        {
            get { return m_vel; }
            set { m_vel = value; }
        }

        public Vector2 Position
        {
            get { return m_pos; }
            set { m_pos = value; }
        }

        public MovingGraphic(Texture2D txr, Rectangle rect)
            :base(txr, rect)
        {
            m_pos = new Vector2(rect.X, rect.Y);
            m_vel = Vector2.Zero;
        }

        public virtual void updateme(GameTime gt)
        {
            m_pos = m_pos + m_vel;
            m_rect.X = (int)m_pos.X;
            m_rect.Y = (int)m_pos.Y;
        }

        public override void drawme(SpriteBatch sb)
        {

            sb.Draw(m_txr, m_rect, Color.White);
        }
    }

    class AnimatedGraphic : MovingGraphic
    {
        protected Rectangle m_srcRect;
        protected float m_updateTrigger;
        protected int m_framesPerSecond;
        protected int m_frames;

        public AnimatedGraphic(Texture2D spritesheet, Rectangle srcRect, int fps, int amountOfFrames)
            :base(spritesheet, srcRect)
        {
            m_frames = amountOfFrames;
            m_framesPerSecond = fps;
            m_updateTrigger = 0;

            m_srcRect = new Rectangle(0, 0, (int)(spritesheet.Width / amountOfFrames), spritesheet.Height);

            m_pos = new Vector2(srcRect.X, srcRect.Y);
            m_vel = Vector2.Zero;
        }

        public override void updateme(GameTime gt)
        {
            m_updateTrigger += (float)gt.ElapsedGameTime.TotalSeconds * m_framesPerSecond;

            if (m_updateTrigger >= 1)
            {
                m_updateTrigger = 0;
                m_srcRect.X += m_srcRect.Width;
                if (m_srcRect.X == m_txr.Width)
                    m_srcRect.X = 0;
            }

            base.updateme(gt);
        }


        public override void drawme(SpriteBatch sb)
        {

            sb.Draw(m_txr, m_rect, m_srcRect, Color.White);
        }
    }

    class LitSprite : AnimatedGraphic
    {
        private Color m_tint;
        private float m_timer;
        private Texture2D m_lightAura;
        public Texture2D txr
        {
            get
            {
                return m_lightAura;
            }
            set
            {
                m_lightAura = value;
            }

        }


        private Vector2 m_lightOffset;

        public LitSprite(Texture2D txr, Rectangle rect, int framecount, int fps, Texture2D lightaura)
            : base(txr, rect, framecount, fps)
        {
            m_lightAura = lightaura;
            m_lightOffset = new Vector2(m_lightAura.Bounds.Width / 2 - m_srcRect.Center.X, m_lightAura.Bounds.Height / 2 - m_srcRect.Center.Y);

            m_timer = 0.5f;

            m_tint = Color.White;
        }

        public void update(GameTime gt, GameMode gamemode)
        {
            if (gamemode == GameMode.strobe)
            {
                m_timer -= (float)gt.ElapsedGameTime.TotalMinutes;

                if (m_timer <= 0)
                {
                    m_tint = new Color(Game1.RNG.Next(0, 255), Game1.RNG.Next(0, 255), Game1.RNG.Next(0, 255));
                }

            }
 	        base.updateme(gt);
        }

        public override void drawme(SpriteBatch sb)
        {
            //sb.Draw(m_lightAura, m_pos, null, Color.White, 0, m_lightOffset, 1, SpriteEffects.None, 0);
            base.drawme(sb);
        }

        public virtual void Drawmask(SpriteBatch sb)
        {
            
                sb.Draw(m_lightAura, m_pos, null, m_tint*1f, 0, m_lightOffset, 1, SpriteEffects.None, 0);
            
        }
    }

    class Projectile : LitSprite
    {
        private float rotation;


        public bool Active { get; set; }
        private Vector2 centre;

        public Vector2 m_centre
        {
            get
            {
                return centre;
            }
            set
            {
                centre = value;
            }
        }
        public BoundingSphere Collision
        {
            get { return new BoundingSphere(new Vector3(m_pos, 0), m_rect.Width/2); }

        }
        private Color m_tint = Color.White;
        
        public Projectile(Texture2D spritesheet, Rectangle srcRect, int fps, int howmanyframes, float rot,Color tint, Texture2D lightaura)
            : base(spritesheet, srcRect, fps, howmanyframes, lightaura)
        {
            centre = new Vector2(m_srcRect.Width / 2, m_srcRect.Width / 2);
            rotation = rot;
            m_vel = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));
            m_vel.Normalize();
            m_vel *= 10;
            Active = true;
            m_tint = tint;
        }

        public void updateme(GameTime gt, Rectangle screensize)
        {
            if (Active)
            {
                m_vel.Y += 0.10f;

                

                rotation = (float)Math.Atan2(m_vel.X, -m_vel.Y);

                if (m_pos.Y + m_rect.Height < 0)
                    m_pos.Y = screensize.Height - 1;
                if (m_pos.Y > screensize.Height)
                    m_pos.Y = 1 - m_rect.Height;
                if (m_pos.X + m_rect.Width < 0)
                    m_pos.X = screensize.Width - 1;
                if (m_pos.X > screensize.Width)
                    m_pos.X = 1 - m_rect.Width;

                base.updateme(gt);
            }
        }   

        public override void drawme(SpriteBatch sb)
        {
            m_rect.X = (int)m_pos.X;
            m_rect.Y = (int)m_pos.Y;

            //sb.Draw(m_txr, m_rect, m_srcRect, Color.White);
            sb.Draw(m_txr, m_rect, m_srcRect,  m_tint, rotation, centre, SpriteEffects.None, 0);
            
        }


       
    }

    class rain : MovingGraphic
    {
        private float m_rot;
        private Vector2 m_centre;
        

        public rain(Texture2D txr, Rectangle rect)
            :base(txr, rect)
        {
            m_vel.Y = 10;
            m_vel.X = 4;

            m_pos.X = Game1.RNG.Next(960);
            m_pos.Y = Game1.RNG.Next(768);

            m_centre = new Vector2(m_rect.Width / 2, m_rect.Width / 2);
        }


        public override void updateme(GameTime gt)
        {
            m_rot = (float)Math.Atan2(m_vel.X, -m_vel.Y);

            if (m_pos.Y > 768)
            {
                m_pos.Y = 0;
                m_pos.X = Game1.RNG.Next(960);
            }

            base.updateme(gt);
        }

        public override void drawme(SpriteBatch sb)
        {
            m_rect.X = (int)m_pos.X;
            m_rect.Y = (int)m_pos.Y;

            sb.Draw(m_txr, m_rect, m_rect, Color.White *0.5f, m_rot, m_centre, SpriteEffects.None, 0);           
        }
    }

    enum Direction
    {
        East,
        South,
        West,
        North
    }

    class Quiver : MovingGraphic
    {
        private List<MovingGraphic> m_ammo;

        public List<MovingGraphic> ammo
        {
            get
            {
                return m_ammo;
            }
            set
            {
                m_ammo = value;
            }
        }

        public Quiver(Texture2D txr, Rectangle rect, ContentManager Content)
            : base(txr, rect)
        {
            m_ammo = new List<MovingGraphic>();
            returnAmmo(Content);
        }

        public override void updateme(GameTime gt)
        {
            for (int i = 0; i < m_ammo.Count; i++)
            {
                m_ammo[i].Position = new Vector2(Position.X + (i * 14), Position.Y);
                m_ammo[i].updateme(gt);
            }
            base.updateme(gt);
        }

        public void add(ContentManager Content)
        {
            if (m_ammo.Count == 0)
            {
                m_ammo.Add(new MovingGraphic(Content.Load<Texture2D>("Ammo"), new Rectangle((int)Position.X, (int)Position.Y, 12, 12)));
            }
            else if (m_ammo.Count == 1)
            {
                m_ammo.Add(new MovingGraphic(Content.Load<Texture2D>("Ammo"), new Rectangle((int)Position.X + 14, (int)Position.Y, 12, 12)));
            }
            else if (m_ammo.Count == 2)
            {
                m_ammo.Add(new MovingGraphic(Content.Load<Texture2D>("Ammo"), new Rectangle((int)Position.X + 28, (int)Position.Y, 12, 12)));
            }
        }

        public void returnAmmo(ContentManager Content)
        {
            m_ammo.Clear();
            m_ammo.Add(new MovingGraphic(Content.Load<Texture2D>("Ammo"), new Rectangle((int)Position.X, (int)Position.Y, 12, 12)));
            m_ammo.Add(new MovingGraphic(Content.Load<Texture2D>("Ammo"), new Rectangle((int)Position.X + 14, (int)Position.Y, 12, 12)));
            m_ammo.Add(new MovingGraphic(Content.Load<Texture2D>("Ammo"), new Rectangle((int)Position.X + 28, (int)Position.Y, 12, 12)));
        }

        public override void drawme(SpriteBatch sb)
        {
            for (int i = 0; i < m_ammo.Count; i++)
            {
                m_ammo[i].drawme(sb);
            }

            base.drawme(sb);
        }
    }

    class RotationInd : MovingGraphic
    {
        private float m_rot;
        protected Vector2 m_centre;
        private float m_facing;

        public Vector2 centre
        {
            get
            {
                return m_centre;
            }
            set
            {
                m_centre = value;
            }

        }
        public float Rotation
        {
            get { return m_rot; }
        }


        public RotationInd(Texture2D txr, Rectangle rect)
            : base(txr, rect)
        {
            m_centre = new Vector2(-(txr.Width / 2), txr.Height/2);
            m_pos.X = 300;
            m_pos.Y = 300;
        }

        public void updateme(GameTime gt, PlayerIndex Player)
        {
            Vector2 direction = Vector2.Normalize(new Vector2((GamePad.GetState(Player).ThumbSticks.Right.X), (GamePad.GetState(Player).ThumbSticks.Right.Y)));
            m_rot = (float)Math.Atan2(direction.Y, direction.X);
            m_rot *= -1;

            

            m_facing = MathHelper.PiOver4 * (float)Math.Round(m_rot / MathHelper.PiOver4);
        
        }

        public override void drawme(SpriteBatch sb)
        {
            m_rect.X = (int)m_pos.X;
            m_rect.Y = (int)m_pos.Y;

            sb.Draw(m_txr, m_rect, null, Color.White, m_facing, m_centre, SpriteEffects.None, 0);  
        }
    }

    class Chests : LitSprite
    {
        
        private List<Powerup> m_powerUp;
        private bool m_bool = false;
        private bool state;
        
        private  int k;

        public Chests(Texture2D txr, Rectangle srcRect, int fps, int HowManyFrames, Texture2D lightaura)
            : base(txr, srcRect, fps, HowManyFrames, lightaura)
        {
            m_powerUp = new List<Powerup>();
            state = false;
            m_srcRect = new Rectangle(0, 0, (int)(txr.Width / HowManyFrames), txr.Height);
        }

        public void update(GameTime gt, ContentManager Content, List<PlayerClass> player)
        {
            for (int h = 0; h < player.Count; h++)
            {
                for (int i = 0; i < m_powerUp.Count; i++)
                {
                    m_powerUp[i].updateme(gt, player[h].rect);
                }

                if (player[h].rect.Intersects(m_rect))
                {
                    state = true;
                }

                if (state)
                {
                    if (m_srcRect.X != m_txr.Width - (m_txr.Width / m_frames))
                    {
                        m_updateTrigger += (float)gt.ElapsedGameTime.TotalSeconds * m_framesPerSecond;
                        if (m_updateTrigger >= 1)
                        {
                            m_srcRect.X += m_srcRect.Width;
                            m_updateTrigger = 0;
                        }
                    }

                    if (m_srcRect.X == m_txr.Width - (m_txr.Width / m_frames) && m_bool == false)
                    {

                        k = Game1.RNG.Next(0, 2);
                        player[h].currentPower = k;
                        if (k == 1)
                            m_powerUp.Add(new ArrowPowerup(Content.Load<Texture2D>("AmmoPU"), new Rectangle(m_rect.X + (m_rect.Width / 4), m_rect.Y, 17, 17), 1, 1));
                        //m_powerUp.Add(new ResizePowerup(Content.Load<Texture2D>("PowerupPH"), new Rectangle(m_rect.X + (m_rect.Width / 4), m_rect.Y, 17, 17), 1, 1));
                        else if (k == 0)
                            m_powerUp.Add(new ResizePowerup(Content.Load<Texture2D>("PowerupPH"), new Rectangle(m_rect.X + (m_rect.Width / 4), m_rect.Y, 17, 17), 1, 1));
                        m_bool = true;
                    }




                    for (int i = 0; i < m_powerUp.Count; i++)
                    {
                        if (m_powerUp[i].endPos >= 0.25f)
                        {
                            if (player[h].rect.Intersects(m_powerUp[i].rect) && player[h].timerStart == false)
                            {

                                m_powerUp[i].ApplyPowerup(player[h], Content);
                                m_powerUp.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
                }
            }
        
        public override void drawme(SpriteBatch sb)
        {
           

            sb.Draw(m_txr, m_rect, m_srcRect, Color.White);
            for (int i = 0; i < m_powerUp.Count; i++)
            {
                m_powerUp[i].drawme(sb);
            }
        }

    }

    enum PowerState
    {
        on,
        off
    }

    abstract class Powerup : AnimatedGraphic
    {
        protected float m_endPos;
        protected PowerState m_pState;
        protected Rectangle m_playerRect;

        public float endPos
        {
            get
            {
                return m_endPos;
            }
            set
            {
                m_endPos = value;
            }

        }



        public Powerup(Texture2D txr, Rectangle rect, int fps, int amountOfFrames)
            :base(txr, rect, fps,amountOfFrames)
        {
            m_pState = PowerState.off;
            
        }

        public virtual void updateme(GameTime gt, Rectangle PlayerRect)
        {
            m_playerRect = PlayerRect;

            if (m_endPos < 0.25f)
            {
                m_endPos += (float)gt.ElapsedGameTime.TotalSeconds;
                m_pos.Y = m_pos.Y - 2;
            }
            else if (m_endPos >= 0.25f)
            {
                m_pState = PowerState.on;
            }

            base.updateme(gt);
        }

        public virtual void ApplyPowerup(PlayerClass player, ContentManager Content)
        {
            
        }

        public override void drawme(SpriteBatch sb)
        {
            base.drawme(sb);
        }    
    }

    class ArrowPowerup : Powerup
    {
        public ArrowPowerup(Texture2D txr, Rectangle rect, int fps, int amountOfFrames)
            :base(txr, rect, fps, amountOfFrames)
        {

            

        }

        public override void updateme(GameTime gt,Rectangle PlayerRect)
        {
            base.updateme(gt,PlayerRect);
        }

        public override void ApplyPowerup(PlayerClass player, ContentManager Content)
        {
            player.Quiver.returnAmmo(Content);
            player.currentPower = 1;
            base.ApplyPowerup(player, Content);
        }
        public override void drawme(SpriteBatch sb)
        {

            base.drawme(sb);
        }
    }

    class ResizePowerup : Powerup
    {

        private int i;
        

        public ResizePowerup(Texture2D txr, Rectangle rect, int fps, int amountOfFrames)
            :base(txr, rect, fps, amountOfFrames)
        {
          
           
        }

        public override void updateme(GameTime gt, Rectangle PlayerRect)
        {
            
            base.updateme(gt,PlayerRect);
        }


        public override void ApplyPowerup(PlayerClass player, ContentManager Content)
        {
            
            i = Game1.RNG.Next(0, 2);
            player.scale = i;
            
            if (i == 0)
            {
                player.rect = new Rectangle(player.rect.X, player.rect.Y, player.rect.Width * 2, player.rect.Height * 2);                   
            }
            else if (i == 1)
            {
                player.rect = new Rectangle(player.rect.X, player.rect.Y, player.rect.Width / 2, player.rect.Height / 2);  
            }
            player.timerStart = true;
            player.currentPower = 0;
            base.ApplyPowerup(player, Content);
        }     

        public override void drawme(SpriteBatch sb)
        {
           
            //base.drawme(sb);
            //sb.DrawString(Game1.debug,(int)m_timer + "", new Vector2(0, 0), Color.White);
            sb.Draw(m_txr, m_rect, m_srcRect, Color.DarkSeaGreen);
        }


    }

    class Wings : Powerup
    {
        public Wings(Texture2D txr, Rectangle rect, int fps, int howmanyframes)
            :base(txr, rect, fps, howmanyframes)
        {

        }
        public override void updateme(GameTime gt, Rectangle PlayerRect)
        {
            base.updateme(gt, PlayerRect);
        }

        public override void ApplyPowerup(PlayerClass player, ContentManager Content)
        {
            
            base.ApplyPowerup(player, Content);
        }

        public override void drawme(SpriteBatch sb)
        {
            base.drawme(sb);
        }
    }

  


    
}

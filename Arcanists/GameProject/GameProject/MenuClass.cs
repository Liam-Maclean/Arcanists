using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    class MenuClass
    {
        private BasicGraphic m_background;
        private BasicGraphic m_title;
        private BasicGraphic m_buttonA;
        private BasicGraphic m_stick;
        private StringVar m_stickstring;
        private StringVar m_instA;
        private List<string> m_string;
        private List<Vector2> m_vel;
        private List<Vector2> m_pos;
        private List<StringVar> menuString;
        private Color m_color;


        private int currentSelect;

        public int currentSel
        {
            get
            {
                return currentSelect;
            }
            set
            {
                currentSelect = value;
            }

        }



        public MenuClass(ContentManager Content)
        {
            m_stick = new BasicGraphic(Content.Load<Texture2D>("ChoicePic"), new Rectangle(720, 700, 45, 45));
            m_stickstring = new StringVar(Game1.debug, "Select", new Vector2(770, 715), Color.White);
            m_background = new BasicGraphic(Content.Load<Texture2D>("background"), new Rectangle(0, 0, 960, 768));
            m_title = new BasicGraphic(Content.Load<Texture2D>("Arcanist"), new Rectangle(260, 100, 508, 108));
            m_instA = new StringVar(Game1.debug, "Select", new Vector2(150, 715), Color.White);
            m_buttonA = new BasicGraphic(Content.Load<Texture2D>("AButtonGame"), new Rectangle(100, 700, 50, 50));
            m_string = new List<string>();
            m_vel = new List<Vector2>();
            m_pos = new List<Vector2>();
            menuString = new List<StringVar>();

            currentSelect = 0;
            m_color = Color.White;

            menuString.Add(new StringVar(Game1.debug,("Play"), new Vector2(480, 350), Color.White));
            menuString.Add(new StringVar(Game1.debug, ("Options"), new Vector2(468, 450), Color.White));
            menuString.Add(new StringVar(Game1.debug, ("Exit"), new Vector2(480, 550), Color.White));

        }

        public void updateme(GameTime gt,  GamePadState gp_Curr, GamePadState gp_Old, bool cammoveup, bool cammovedown, Game1 game)
        {
            if (cammovedown == false && cammoveup == false)
            {
                if (gp_Curr.ThumbSticks.Left.Y < 0 && gp_Old.ThumbSticks.Left.Y >= 0)
                {
                    currentSelect++;
                }
                else if (gp_Curr.ThumbSticks.Left.Y > 0 && gp_Old.ThumbSticks.Left.Y <= 0)
                {
                    currentSelect--;
                }
                for (int i = 0; i < menuString.Count; i++)
                {
                    if (currentSelect == i)
                    {
                        menuString[currentSelect].color = Color.Red;
                    }
                    else
                    {
                        menuString[i].color = Color.White;
                    }
                }

                if (currentSelect == 2)
                {
                    if (gp_Curr.Buttons.A == ButtonState.Pressed)
                    {
                        game.Exit();
                    }
                }


                if (currentSelect < 0)
                {
                    currentSelect = 2;
                }
                else if (currentSelect > 2)
                {
                    currentSelect = 0;
                }
            }
        }

        public void drawme(SpriteBatch sb)
        {
            m_background.drawme(sb);
            m_title.drawme(sb);
            m_buttonA.drawme(sb);
            m_stick.drawme(sb);
            m_stickstring.drawme(sb);
            m_instA.drawme(sb);
            for (int i = 0; i < menuString.Count; i++)
            {
                menuString[i].drawme(sb);
            }
        }
    }

    class CharacterSelect
    {
        private BasicGraphic m_background;
        private BasicGraphic m_scroll;
        private fadingGraphic m_stick;
        private fadingString m_stickstring;
        private fadingGraphic m_title;
        private List<CharGraphic> m_splash;
        public List<CharGraphic> splash
        {
            get
            {
                return m_splash;
            }
            set
            {
                m_splash = value;
            }
        }
        private fadingGraphic m_ButtonA;
        private fadingString m_instA;
        private List<fadingString> m_chardata;
        private List<Texture2D> m_splashTxr;
        private List<selection> m_charSelect;

        public CharacterSelect(ContentManager Content)
        {

            m_stick = new fadingGraphic(Content.Load<Texture2D>("ChoicePic"), new Rectangle(720, -150, 45, 45));
            m_stickstring = new fadingString(Game1.debug, "Select", new Vector2(770, -130), Color.White);
            m_background = new BasicGraphic(Content.Load<Texture2D>("Background2"), new Rectangle(0, -768, 960, 768));
            m_scroll = new BasicGraphic(Content.Load<Texture2D>("CharSelect"), new Rectangle(0, -768, 960, 768));

            m_ButtonA = new fadingGraphic(Content.Load<Texture2D>("AButtonGame"), new Rectangle(100, -150, 45, 45));
            m_title = new fadingGraphic(Content.Load<Texture2D>("Select-Your-Character"), new Rectangle(50, -620, 870, 100));

            m_instA = new fadingString(Game1.debug, "Select", new Vector2(150, -130), Color.White);
            m_splash = new List<CharGraphic>();
            m_charSelect = new List<selection>();
            m_chardata = new List<fadingString>();
            m_splashTxr = new List<Texture2D>();


            m_splashTxr.Add(Content.Load<Texture2D>("Char1Splash"));
            m_splashTxr.Add(Content.Load<Texture2D>("Character2Splash"));
            m_splashTxr.Add(Content.Load<Texture2D>("Character3Splash"));
            m_splashTxr.Add(Content.Load<Texture2D>("Character4Splash"));


          
                m_charSelect.Add(new selection(Content.Load<Texture2D>("GameFrame"), new Rectangle(90, -500, 180, 300), Content));
                m_chardata.Add(new fadingString(Game1.debug, "Player 1", new Vector2(140, -180), new Color(0, 0, 0, 0)));
            
                m_charSelect.Add(new selection(Content.Load<Texture2D>("GameFrame"), new Rectangle(290, -500, 180, 300), Content));
                m_chardata.Add(new fadingString(Game1.debug, "Player 2", new Vector2(340, -180), new Color(255, 255, 255, 0)));
          
                m_charSelect.Add(new selection(Content.Load<Texture2D>("GameFrame"), new Rectangle(490, -500, 180, 300), Content));
                m_chardata.Add(new fadingString(Game1.debug, "Player 3", new Vector2(540, -180), new Color(255, 255, 255, 0)));
           
                m_charSelect.Add(new selection(Content.Load<Texture2D>("GameFrame"), new Rectangle(690, -500, 180, 300), Content));
                m_chardata.Add(new fadingString(Game1.debug, "Player 4", new Vector2(740, -180), new Color(255, 255, 255, 0)));

            for (int i = 0; i < m_charSelect.Count; i++)
            {
                m_splash.Add(new CharGraphic(m_splashTxr, new Rectangle(m_charSelect[i].rect.X + 15, m_charSelect[i].rect.Y + 30, 150, 250)));
            }
        }

        public void updateme(GameTime gt, ref Camera2d cam, ContentManager Content, List<PlayerInput> pads, ref List<PlayerClass> Players, ref Gamestate gamestate)
        {
            if (cam.Position.Y == -768)
            {
                for (int i = 0; i < m_splash.Count; i++)
                {
                    m_splash[i].updateme(pads[i]);       
                }

                
                for (int i = 0; i < pads.Count; i++)
                {
                    if (pads[i].IsDown(Buttons.A))
                    {
                        m_splash[i].tbool = true;
                        switch (m_splash[i].TC)
                        {
                            case 0:
                                Players[i].sTxr = Content.Load<Texture2D>("Character2Walking");
                                m_splash[i].alpha = 0.3f;
                                break;
                            case 1:
                                Players[i].sTxr = Content.Load<Texture2D>("Character1Walking");
                                m_splash[i].alpha = 0.3f;
                                break;
                            case 2:
                                Players[i].sTxr = Content.Load<Texture2D>("Character3Walking");
                                m_splash[i].alpha = 0.3f;
                                break;
                            case 3:
                                Players[i].sTxr = Content.Load<Texture2D>("Character4Walking");
                                m_splash[i].alpha = 0.3f;
                                break;
                        }
                        if (m_splash[0].tbool == true)
                        {
                            //cam.Position.Y = 0;
                            gamestate = Gamestate.ModeSelect;
                        }            
                    }
                }

                for (int i = 0; i < pads.Count; i++)
                {
                    if (pads[i].IsDown(Buttons.B))
                    {
                        m_splash[i].tbool = false;
                        m_splash[i].alpha = 1f;
                    }
                }
                #region updates
                m_title.updateme();
                m_instA.updateme();
                m_ButtonA.updateme();
                m_stick.updateme();
                m_stickstring.updateme();
                for (int i = 0; i < m_chardata.Count; i++)
                {
                    m_chardata[i].updateme();
                }
                for (int i = 0; i < m_charSelect.Count; i++)
                {
                    m_charSelect[i].updateme();
                }
                #endregion
            }
            else if (cam.Position.Y == 0)
            {
                for (int i = 0; i < m_chardata.Count; i++)
                {
                    m_chardata[i].alpha = 0;                  
                }
                for (int i = 0; i < m_charSelect.Count; i++)
                {
                    m_charSelect[i].alpha = 0;
                    m_splash[i].alpha = 0;
                }
            }     
        }

        public void drawme(SpriteBatch sb)
        { 
            m_background.drawme(sb);
            m_scroll.drawme(sb);
            m_ButtonA.drawme(sb);
            m_instA.drawme(sb);
            m_title.drawme(sb);
            m_stick.drawme(sb);
            m_stickstring.drawme(sb);
            for (int i = 0; i < m_charSelect.Count; i++)
            {
                m_charSelect[i].drawme(sb);
            }
            for (int i = 0; i < m_splash.Count; i++)
            {
                m_splash[i].drawme(sb);
            }
            for (int i = 0; i < m_chardata.Count; i++)
            {
                m_chardata[i].drawme(sb);
            }
            
        }
    }





    class ModeSelect
    {
        private BasicGraphic m_background;
        private BasicGraphic m_scroll;
        private BasicGraphic m_frame;
        private BasicGraphic m_ButtonA;
        private BasicGraphic m_stick;
        private StringVar m_stickstring;
        private StringVar m_instA;
        private StringVar m_title;
        private List<Texture2D> m_txrModeChoice;
        private BasicGraphic m_gameSwitcher;
        private int m_modeChoice;
        private List<StringVar> m_stringChoice;
        private List<StringVar> m_descriptions;

        public ModeSelect(ContentManager Content)
        {
            m_txrModeChoice = new List<Texture2D>();
            m_stringChoice = new List<StringVar>();
            m_descriptions = new List<StringVar>();


            m_stick = new BasicGraphic(Content.Load<Texture2D>("ChoicePic"), new Rectangle(720, -150, 45, 45));
            m_stickstring = new StringVar(Game1.debug, "Select", new Vector2(770, -130), Color.White);

            m_title = new StringVar(Game1.debug, "Choose your game type", new Vector2(370, -550), Color.White);
            m_frame = new BasicGraphic(Content.Load<Texture2D>("GameFrame"), new Rectangle(100, -500, 250,300));

            m_background = new BasicGraphic(Content.Load<Texture2D>("Background2"), new Rectangle(0, -768, 960, 768));
            m_scroll = new BasicGraphic(Content.Load<Texture2D>("CharSelect"), new Rectangle(0, -768, 960, 768));

            m_ButtonA = new BasicGraphic(Content.Load<Texture2D>("AButtonGame"), new Rectangle(100, -150, 45, 45));
            m_instA = new StringVar(Game1.debug, "Select", new Vector2(150, -130), Color.White);

            m_txrModeChoice.Add(Content.Load<Texture2D>("DeathmatchPic"));          
            m_txrModeChoice.Add(Content.Load<Texture2D>("OneLightPic"));
            m_txrModeChoice.Add(Content.Load<Texture2D>("StrobePic"));

            m_gameSwitcher = new BasicGraphic(m_txrModeChoice[m_modeChoice], new Rectangle(175, -350, 100, 100));

            m_stringChoice.Add(new StringVar(Game1.debug, "Deathmatch", new Vector2(170, -450), Color.White));
            m_descriptions.Add(new StringVar(Game1.debug, "Fight your enemies in a combat gaining kills \n \nThe player with the most kills wins! \n \n \n \n2-4 players required", new Vector2(400, -450), Color.White));


            m_stringChoice.Add(new StringVar(Game1.debug, "Few lights in the dark", new Vector2(130, -450), Color.White));
            m_descriptions.Add(new StringVar(Game1.debug, "Fight your enemies in the dark using only  \n \nthe light of you and your projectiles \n \nThe player with the most kills wins! \n \n \n \n2-4 players required", new Vector2(400, -450), Color.White));

            m_stringChoice.Add(new StringVar(Game1.debug, "Strobe", new Vector2(190, -450), Color.White));
            m_descriptions.Add(new StringVar(Game1.debug, "Don't be destracted by the colours! Fight your enemies \n \nin a combat zone with constant changing colours \n \n \n                  hammer time  \n \n \nThe player with the most kills wins! \n \n \n \n 2-4 players required", new Vector2(400, -450), Color.White));
        }

        public void updateme(GameTime gt, List <PlayerInput> pads, ref Gamestate gamestate, ref GameMode gamemode, ref Camera2d cam)
        {
            for (int i = 0; i < pads.Count; i++)
            {
                if (pads[i].WasPressedBack(Buttons.B))
                {
                    gamestate = Gamestate.Menu;
                }
                if (pads[i].StickLeft())
                {
                    m_modeChoice--;
                }
                if (pads[i].StickRight())
                {
                    m_modeChoice++;
                }
                if (pads[i].WasPressedBack(Buttons.Y))
                {
                    switch (m_modeChoice)
                    {
                        case 0:
                            gamemode = GameMode.normal;      
                            break;
                        case 1:
                            gamemode = GameMode.oneLightInTheDark;
                            break;
                        case 2:
                            gamemode = GameMode.strobe;
                            break;
                    }

                    cam.Position.Y = 0;
                    gamestate = Gamestate.Game;          
                }
            }
            if (m_modeChoice > 2)
            {
                m_modeChoice = 0;
            }
            if (m_modeChoice < 0)
            {
                m_modeChoice = 2;
            }
            m_gameSwitcher.sTxr = m_txrModeChoice[m_modeChoice];
        }

        public void drawme(SpriteBatch sb)
        {        
            m_background.drawme(sb);
            m_scroll.drawme(sb);
            m_frame.drawme(sb);
            m_title.drawme(sb);
            m_stick.drawme(sb);
            m_stickstring.drawme(sb);
            m_ButtonA.drawme(sb);
            m_instA.drawme(sb);
            m_stringChoice[m_modeChoice].drawme(sb);
            m_descriptions[m_modeChoice].drawme(sb);
            m_gameSwitcher.drawme(sb);
        }
    }

    class StringVar
    {
        protected SpriteFont m_font;
        protected Vector2 m_pos;
        protected Color m_color;

        public Color color
        {
            get
            {
                return m_color;
            }
            set
            {
                m_color = value;
            }
        }

        protected string m_text;


        public string text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
            }
        }



        public StringVar(SpriteFont font, string text, Vector2 pos, Color color)
        {
            m_font = font;
            m_text = text;
            m_pos = pos;
            m_color = color;
        }

        public virtual void drawme(SpriteBatch sb)
        {
            sb.DrawString(m_font, m_text, m_pos, m_color);
        }
    }

    class fadingString : StringVar
    {
        protected float m_alpha;
        public float alpha
        {
            get
            {
                return m_alpha;
            }
            set
            {
                m_alpha = value;
            }
        }


        public fadingString(SpriteFont font, string text, Vector2 pos, Color color)
            :base(font, text, pos, color)
        {
            
        }


        public void updateme()
        {
            if (m_alpha < 1)
            {
                m_alpha = m_alpha + 0.01f;
            }
        }


        public override void drawme(SpriteBatch sb)
        {
            sb.DrawString(m_font, m_text, m_pos, Color.White * m_alpha);
        }       
    }




    class selection : fadingGraphic
    {
        //BasicGraphic m_charpic;
        private List<fadingGraphic> m_arrows;
        
        
        public selection(Texture2D txr, Rectangle rect, ContentManager Content)
            :base(txr, rect)
        {
            m_arrows = new List<fadingGraphic>();

            m_arrows.Add(new fadingGraphic(Content.Load<Texture2D>("ArrowSelect"), new Rectangle(m_rect.X + (m_rect.Width/2) - 60,m_rect.Y + m_rect.Height - 100,120,50)));
            
        }

        public override void updateme()
        {
            for (int i = 0; i < m_arrows.Count; i++)
            {
                m_arrows[i].updateme();
            }

            base.updateme();
            
        }

        public override void drawme(SpriteBatch sb)
        {

            base.drawme(sb);
            //for (int i = 0; i < m_arrows.Count; i++)
            //{
            //    m_arrows[i].drawme(sb);
            //}
        }
    }

    class fadingGraphic : BasicGraphic
    {
        protected float m_alpha;
        public float alpha
        {
            get
            {
                return m_alpha;
            }

            set
            {
                m_alpha = value;
            }
        }



        public fadingGraphic(Texture2D txr, Rectangle rect)
            : base(txr, rect)
        {

        }

        public virtual void updateme()
        {
            if (m_alpha < 1)
            {
                m_alpha = m_alpha + 0.01f;
            }

        }


        public override void drawme(SpriteBatch sb)
        {
            sb.Draw(m_txr, m_rect, Color.White * m_alpha);
        }
    }

    class CharGraphic
    {
        protected int textureChoice;
        protected bool txrbool;


        public bool tbool
        {
            get
            {
                return txrbool;
            }
            set
            {
                txrbool = value;
            }
        }

        public int TC
        {
            get
            {
                return textureChoice;
            }
            set
            {
                textureChoice = value;
            }

        }

        private bool chosenOrNot;

        public bool chosenNot
        {
            get
            {
                return chosenOrNot;
            }
            set
            {
                chosenOrNot = value;
            }
        }


        private fadingString m_charText;

        private List<Texture2D> m_txr;
        private Rectangle m_rect;
        protected float m_alpha;

        public float alpha
        {
            get
            {
                return m_alpha;
            }
            set
            {
                m_alpha = value;
            }

        }


        public CharGraphic(List<Texture2D> txr, Rectangle rect)
        {
            m_charText = new fadingString(Game1.debug, "", new Vector2(m_rect.X + 10, m_rect.Y + 20), Color.White);

            chosenOrNot = false;

            txrbool = false;
            m_txr = new List<Texture2D>();
            m_txr = txr;
            m_rect = rect;
            textureChoice = 0;
        }

        public void updateme(PlayerInput Player)
        {
            if ((Player.StickLeft() && txrbool == false))
            {
                textureChoice = textureChoice-1;
            }
            if ((Player.StickRight() && txrbool == false))
            {
                textureChoice = textureChoice + 1;
            }
            if (textureChoice > 3)
            {
                textureChoice = 0;
            }
            if (textureChoice < 0)
            {
                textureChoice = 3;
            }

            switch (textureChoice)
            {
                case 0:
                    m_charText = new fadingString(Game1.debug, "Blue Wizard", new Vector2(m_rect.X + 10, m_rect.Y), Color.White);
                    //m_charText.text = "Blue Wizard";
                    break;
                case 1:
                    m_charText = new fadingString(Game1.debug, "Green Wizard", new Vector2(m_rect.X + 10, m_rect.Y), Color.White);
                    //m_charText.text = "Green Wizard";
                    break;
                case 2:
                    m_charText = new fadingString(Game1.debug, "Red Wizard", new Vector2(m_rect.X + 10, m_rect.Y), Color.White);
                    //m_charText.text = "Red Wizard";
                    break;
                case 3:
                    m_charText = new fadingString(Game1.debug, "Purple Wizard", new Vector2(m_rect.X + 10, m_rect.Y), Color.White);
                    //m_charText.text = "Purple Wizard";
                    break;
            }

            if (m_alpha < 1 && txrbool == false)
            {
                m_alpha = m_alpha + 0.01f;
            }
        }

        public void drawme(SpriteBatch sb)
        { 
             sb.Draw(m_txr[textureChoice], m_rect, Color.White * m_alpha);
             m_charText.drawme(sb);
        }
    }
}

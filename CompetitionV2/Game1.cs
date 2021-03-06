using CompetitionV2.Armes;
using CompetitionV2.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

namespace CompetitionV2
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static GameWindow Screen;
        public static PenumbraComponent Penumbra;


        private int FPSCounter;
        private double FPSTime;
        private int LastFPS;

        private static SoundEffectInstance seiTrameJeu;

        static IPartieDeJeu[] PartieDuJeu;
        private static int m_IndexPartieDeJeu;
        private static Game1 Instance;

        public static WeaponType[] Arma = new WeaponType[]
        {
            WeaponType.Pistol, WeaponType.AssaultRifle, WeaponType.Shotgun
        };


        public static void SetPartieDeJeu(int i)
        {
            if (m_IndexPartieDeJeu != i)
            {
                //Sortie de jeu
                if(m_IndexPartieDeJeu == 1)
                {
                    seiTrameJeu.Stop();
                    seiTrameJeu = SoundManager.TrameSonoreMenu.CreateInstance();
                    seiTrameJeu.Volume = (float)1;
                    seiTrameJeu.IsLooped = true;
                    seiTrameJeu.Play();
                }
                //Entr�e en jeu
                if(i == 1)
                {
                    seiTrameJeu.Stop();
                    seiTrameJeu = SoundManager.TrameSonoreJeu.CreateInstance();
                    seiTrameJeu.Volume = (float)0.5;
                    seiTrameJeu.IsLooped = true;
                    seiTrameJeu.Play();
                }
                
                switch (i)
                {
                    case (int)TypesDePartieDeJeu.Jeu:
                        PartieDuJeu[i] = new JeuMenu();
                        break;
                    case (int)TypesDePartieDeJeu.MenuDefaut:
                        PartieDuJeu[i] = new MenuDefaut();
                        break;
                    case (int)TypesDePartieDeJeu.Armurerie:
                        PartieDuJeu[i] = new Armurerie();
                        break;
                    case (int)TypesDePartieDeJeu.FermerJeu:
                        Instance.Exit();
                        break;
                    case (int)TypesDePartieDeJeu.Perdu:
                        PartieDuJeu[i] = new Perdu();
                        break;
                    case (int)TypesDePartieDeJeu.Gagne:
                        PartieDuJeu[i] = new Gagne();
                        break;
                }
                // CRASH REPORT
                // Instance.GraphicsDevice.Clear(Color.Gray);
                m_IndexPartieDeJeu = i;
            }
        }
    
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            _graphics.IsFullScreen = true;
         //   int Dimension = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;



            int Dimension = 768;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;

            _graphics.PreferredBackBufferHeight = Dimension;
            _graphics.PreferredBackBufferWidth = Dimension;
            _graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
            IndexPartieDeJeu = 0;
            Instance = this;
        }

        public int IndexPartieDeJeu
        {
            get { return m_IndexPartieDeJeu; }
            set { m_IndexPartieDeJeu = value; }
        }

        protected override void Initialize()
        {
          /*  int Dimension =GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            _graphics.PreferredBackBufferHeight = Dimension;
            _graphics.PreferredBackBufferWidth = Dimension;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();*/
            
            IsMouseVisible = true;
            //Window.AllowUserResizing = true;
            Screen = Window;
            Penumbra = new PenumbraComponent(this);
            Components.Add(Penumbra);
            Penumbra.AmbientColor = Color.Black;
            TextureManager.InitInstance(Content);
            
            PartieDuJeu = new IPartieDeJeu[6];
            PartieDuJeu[(int)TypesDePartieDeJeu.MenuDefaut] = new MenuDefaut();
            //PartieDuJeu[(int)TypesDePartieDeJeu.Jeu] = new JeuMenu();
            //PartieDuJeu[(int)TypesDePartieDeJeu.Armurerie] = new Armurerie();

            
            SoundManager.InitInstance(Content);

            seiTrameJeu = SoundManager.TrameSonoreMenu.CreateInstance();
            seiTrameJeu.Play();
            
            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            
            Penumbra.Initialize();
            Penumbra.Visible = true;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == 
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                    Keys.Escape))
                Exit();
            

            PartieDuJeu[IndexPartieDeJeu].Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            FPSCounter++;
            PartieDuJeu[IndexPartieDeJeu].DrawWithShadows(_spriteBatch, gameTime, GraphicsDevice);

            if (gameTime.TotalGameTime.TotalMilliseconds - 1000 > FPSTime)
            {
                FPSTime = gameTime.TotalGameTime.TotalMilliseconds;
                LastFPS = FPSCounter;
                FPSCounter = 0;
            }
            
            base.Draw(gameTime);

            PartieDuJeu[IndexPartieDeJeu].DrawWithoutShadows(_spriteBatch, gameTime, GraphicsDevice);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(TextureManager.Font, "FPS: " + LastFPS, new Vector2(10, 10), Color.Yellow);
            _spriteBatch.End();
            

        }
        
    }
}
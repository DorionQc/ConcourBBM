﻿using CompetitionV2.Armes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CompetitionV2.Menu
{
    class Armurerie : Menu
    {
        private static readonly Button[] btn = new Button[]
            {
                new Button("Jouer!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/3-120, Game1.Screen.ClientBounds.Height-100, 240, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Jeu),
                new Button("Retour!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/3*2-120, Game1.Screen.ClientBounds.Height-100, 240, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.MenuDefaut),


                new ArmurerieButton("Pistolet!", TextureManager.TextureTerre[0], new Rectangle(200, 10, 300, 100),
                    Game1.SetPartieDeJeu, (int) WeaponType.Pistol),
                new ArmurerieButton("Fusil d'assault!", TextureManager.TextureTerre[0], new Rectangle(200, 120, 300, 100),
                    Game1.SetPartieDeJeu, (int) WeaponType.AssaultRifle),
                new ArmurerieButton("Shotgun!", TextureManager.TextureTerre[0], new Rectangle(200, 230, 300, 100),
                    Game1.SetPartieDeJeu, (int) WeaponType.Shotgun),
                new ArmurerieButton("Sniper!", TextureManager.TextureTerre[0], new Rectangle(550, 120, 300, 100),
                    Game1.SetPartieDeJeu, (int) WeaponType.SemiAutoSniper),
                new ArmurerieButton("Bolt action!", TextureManager.TextureTerre[0], new Rectangle(550, 10, 300, 100),
                    Game1.SetPartieDeJeu, (int) WeaponType.BoltAction),



                new ButtonInfo("Vide", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/10*2-150, 500, 300, 100),
                    Game1.SetPartieDeJeu, 0), 
                new ButtonInfo("Vide", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/10*5-150, 500, 300, 100),
                    Game1.SetPartieDeJeu, 1),
                new ButtonInfo("Vide", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/10*8-150, 500, 300, 100),
                    Game1.SetPartieDeJeu, 2)


            };
        public Armurerie():base(btn)
        {

        }
    }



    class ButtonInfo : Button
    {
        public ButtonInfo(string nom, Texture2D texture, Rectangle ButtonBox, EventHandler job, int Arme)
            : base(nom, texture, ButtonBox, job, Arme)
        {

        }


        public override void Update(GameTime gameTime)
        {
            //do nothing
        }

        public override void Draw(SpriteBatch sb)
        {


            sb.Draw(BackgroundTexture, ButtonRect, Color.Green);


            
            if (PartieDeJeu < 3 && Game1.Arma[PartieDeJeu] != null)
            {
                Vector2 size = TextureManager.Font.MeasureString(Game1.Arma[PartieDeJeu].ToString());
                sb.DrawString(TextureManager.Font, Game1.Arma[PartieDeJeu].ToString(), ButtonRect.Center.ToVector2(), Color.White, 0, size * 0.5f, 2.0f,
                SpriteEffects.None, 0);
            }
            else
            {
                Vector2 size = TextureManager.Font.MeasureString(Nom);
                sb.DrawString(TextureManager.Font, Nom, ButtonRect.Center.ToVector2(), Color.White, 0, size * 0.5f, 2.0f,
                SpriteEffects.None, 0);
            }
            


        }
    }




    class ArmurerieButton : Button
    {
        private bool m_IsValid;
        public ArmurerieButton(string nom, Texture2D texture, Rectangle ButtonBox, EventHandler job, int Arme) :base( nom,  texture,  ButtonBox,  job, Arme)
        {
            m_IsValid = ProgressManager.ArmesAchete[Arme];
        }

        private bool IsValid
        {
            get
            {
                bool inthere = false;
                for (int i = 0; i < Game1.Arma.Length && !inthere; i++)
                {
                    inthere=((int)Game1.Arma[i] == PartieDeJeu);
                }
                return m_IsValid && !inthere;
            }
            set { m_IsValid = value; }
        }


        public override void Update(GameTime gameTime)
        {
            if (IsValid)
            {
                if (enterButton() && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    WasPressed = true;
                }
                if (WasPressed && enterButton() && Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    WasPressed = false;

                    for (int i = Game1.Arma.Length - 1; i >= 1; i--)
                    {
                        Game1.Arma[i] = Game1.Arma[i-1];
                    }
                    Game1.Arma[0] = (WeaponType)PartieDeJeu;
                   /* switch (PartieDeJeu)
                    {
                        case (int) WeaponType.Pistol:
                            EntityManager.Instance.Joueur.Weapon[0] = new Pistol(EntityManager.Instance.Joueur);
                            break;
                        case (int) WeaponType.AssaultRifle:
                            EntityManager.Instance.Joueur.Weapon[0] = new AssaultRifle(EntityManager.Instance.Joueur);
                            break;
                        case (int) WeaponType.Shotgun:
                            EntityManager.Instance.Joueur.Weapon[0] = new Shotgun(EntityManager.Instance.Joueur);
                            break;
                        case (int)WeaponType.SemiAutoSniper:
                            EntityManager.Instance.Joueur.Weapon[0] = new SemiAutomaticSniper(EntityManager.Instance.Joueur);
                            break;
                        case (int)WeaponType.BoltAction:
                            EntityManager.Instance.Joueur.Weapon[0] = new BoltActionSniper(EntityManager.Instance.Joueur);
                            break;
                    }*/
                }
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            if (IsValid)
            {
                if (enterButton())
                {
                    sb.Draw(BackgroundTexture, ButtonRect, Color.Gray);
                }
                else
                {
                    sb.Draw(BackgroundTexture, ButtonRect, Color.White);
                }
            }
            else
            {
                sb.Draw(BackgroundTexture, ButtonRect, Color.Black);
            }
            Vector2 size = TextureManager.Font.MeasureString(Nom);

            sb.DrawString(TextureManager.Font, Nom, ButtonRect.Center.ToVector2(), Color.White, 0, size * 0.5f, 2.0f, SpriteEffects.None, 0);


        }
    }

}

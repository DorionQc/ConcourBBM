﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Competition.Armes;
using CompetitionV2.Armes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownGridBasedEngine;
using TopDownGridBasedEngine.Armes;
using TopDownGridBasedEngine.Projectile;

namespace CompetitionV2.Menu
{
    class Armurerie : Menu
    {
        private static Button[] btn = new Button[]
            {
                new Button("Jouer!", TextureManager.TextureTerre[0], new Rectangle(10, 10, 120, 60),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Jeu),
                new Button("Retour!", TextureManager.TextureTerre[0], new Rectangle(10, 160, 120, 60),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.MenuDefaut),


                new ArmurerieButton("Pistolet!", TextureManager.TextureTerre[0], new Rectangle(200, 10, 250, 100),
                    Game1.SetPartieDeJeu, (int) WeaponType.Pistol),
                new ArmurerieButton("Fusil d'assault!", TextureManager.TextureTerre[0], new Rectangle(200, 120, 250, 100),
                    Game1.SetPartieDeJeu, (int) WeaponType.AssaultRifle),
                new ArmurerieButton("Shotgun!", TextureManager.TextureTerre[0], new Rectangle(200, 230, 240, 100),
                    Game1.SetPartieDeJeu, (int) WeaponType.Shotgun),



                new ButtonInfo("Vide", TextureManager.TextureTerre[0], new Rectangle(10, 500, 250, 100),
                    Game1.SetPartieDeJeu, 0), 
                new ButtonInfo("Vide", TextureManager.TextureTerre[0], new Rectangle(270, 500, 250, 100),
                    Game1.SetPartieDeJeu, 1),
                new ButtonInfo("Vide", TextureManager.TextureTerre[0], new Rectangle(530, 500, 250, 100),
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


            
            if (EntityManager.Instance.Joueur?.Weapon[PartieDeJeu]?.Nom != null)
            {
                Vector2 size = TextureManager.Font.MeasureString(EntityManager.Instance.Joueur.Weapon[PartieDeJeu].Nom);
                sb.DrawString(TextureManager.Font, EntityManager.Instance.Joueur.Weapon[PartieDeJeu].Nom, ButtonRect.Center.ToVector2(), Color.White, 0, size * 0.5f, 2.0f,
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
                for (int i = 0; i < EntityManager.Instance.Joueur.Weapon.Length && !inthere; i++)
                {
                    inthere=((int)EntityManager.Instance.Joueur.Weapon[i].WeaponType == PartieDeJeu);
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

                    for (int i = 0; i < EntityManager.Instance.Joueur.Weapon.Length - 1; i++)
                    {
                        EntityManager.Instance.Joueur.Weapon[i + 1] = EntityManager.Instance.Joueur.Weapon[i];
                    }
                    switch (PartieDeJeu)
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
                    }
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
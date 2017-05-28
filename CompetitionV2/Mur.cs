﻿using System;
using Microsoft.Xna.Framework;

namespace CompetitionV2
{
    /// <summary>
    /// Sert à la construction de murs lors de la génération de la map. En décentralise le code.
    /// </summary>
    public class Mur
    {
        // Variables de ressources.
        private readonly Map _Map;
        private readonly Random _r;

        // Variables du mur.
        private int _LeftLength;
        private int _Length;
        private int _Orientation;
        private Point _CurrPos;

        public Mur(Map map, Random r, int position)
        {
            _Map = map;
            _r = r;
            _CurrPos.X = position % map.Width;
            _CurrPos.Y = position / map.Width;

            // Balance la longueur des murs en fonction de la largeur de l'écran et du random.
            _LeftLength = r.Next(map.Width / 16, map.Width / 6);
            _Length = _LeftLength;

            _Orientation = (3 * r.Next(0, 4)); // Clockwise;
        }

        /// <summary>
        /// Pose une tile sur le mur, renvoit vrai si la construction du mur est finie.
        /// </summary>
        /// <returns></returns>
        public bool Build()
        {
            AbsCase tile;
            bool over;

            if (_Orientation == 0) // Haut
            {
                over = UpperStart();
            }
            else if (_Orientation == 3) // Droite
            {
                over = RightStart();
            }
            else if (_Orientation == 6) // Bas
            {
                over = DownStart();
            }
            else // Gauche
            {
                over = LeftStart();
            }


            if (!over)
            {
                // Trois dixièmes des murs sont destructibles.
                if (_r.Next(10) < 3)
                    tile = new CaseWall(_CurrPos.X, _CurrPos.Y, _Map);
                else
                    tile = new CaseSolidWall(_CurrPos.X, _CurrPos.Y, _Map);

                _LeftLength -= 1;
                _Map[_CurrPos.X, _CurrPos.Y] = tile;
				
                if (_LeftLength <= 0)
                    over = true;
            }
            return over;
        }

        /// <summary>
        /// Verify the possibility of building a wall up there.
        /// Returns false and set current position if the wall can be correctly built.
        /// Returns true if no wall could ever be built here.
        /// </summary>
        bool CheckUpperWallBuilding()
        {
            bool retour = false;
            bool nextPosIsInvalid = false; // Basically another return but with more explicit name.

            // Section dégueulasse, mais simple.
            if (!nextPosIsInvalid)
            {
                if (_CurrPos.Y < 2)
                {
                    nextPosIsInvalid = true;
                }
                else
                {
                    if (_CurrPos.X != 0)
                    {
                        if (_Map[_CurrPos.X - 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X - 1, _CurrPos.Y - 2].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.X != _Map.Width - 1)
                    {
                        if (_Map[_CurrPos.X + 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X + 1, _CurrPos.Y - 2].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.Y > 1 && _Map[_CurrPos.X, _CurrPos.Y].Type != CaseType.Vide)
                        nextPosIsInvalid = true;
                }
            }
            // Tentative d'autres directions
            if (nextPosIsInvalid)
                retour = true;
            else
            {
                _CurrPos = new Point(_CurrPos.X, _CurrPos.Y - 1);
                _Orientation = 0;
            }

            return retour;
        }

        /// <summary>
        /// Verify the possibility of building a wall right there.
        /// Returns false and set current position if the wall can be correctly built.
        /// Returns true if no wall could ever be built here.
        /// </summary>
        bool CheckRightWallBuilding()
        {
            bool retour = false;
            bool nextPosIsInvalid = false; // Basically another return but with more explicit name.

            // Section dégueulasse, mais simple.
            if (!nextPosIsInvalid)
            {
                if (_CurrPos.X >= _Map.Width - 2)
                {
                    nextPosIsInvalid = true;
                }
                else
                {
                    if (_CurrPos.Y != 0)
                    {
                        if (_Map[_CurrPos.X + 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X + 2, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.Y != _Map.Height - 1)
                    {
                        if (_Map[_CurrPos.X + 1, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X + 2, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.X < _Map.Width - 2 && _Map[_CurrPos.X, _CurrPos.Y].Type != CaseType.Vide)
                        nextPosIsInvalid = true;
                }
            }
            // Tentative d'autres directions
            if (nextPosIsInvalid)
                retour = true;
            else
            {
                _CurrPos = new Point(_CurrPos.X + 1, _CurrPos.Y);
                _Orientation = 3;
            }

            return retour;
        }

        /// <summary>
        /// Verify the possibility of building a wall down there.
        /// Returns false and set current position if the wall can be correctly built.
        /// Returns true if no wall could ever be built here.
        /// </summary>
        bool CheckDownWallBuilding()
        {
            bool retour = false;
            bool nextPosIsInvalid = false; // Basically another return but with more explicit name.

            // Section dégueulasse, mais simple.
            if (!nextPosIsInvalid)
            {
                if (_CurrPos.Y >= _Map.Height - 2)
                {
                    nextPosIsInvalid = true;
                }
                else
                {
                    if (_CurrPos.X != 0)
                    {
                        if (_Map[_CurrPos.X - 1, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X - 1, _CurrPos.Y + 2].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.X != _Map.Width - 1)
                    {
                        if (_Map[_CurrPos.X + 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X + 1, _CurrPos.Y + 2].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.Y > 1 && _Map[_CurrPos.X, _CurrPos.Y].Type != CaseType.Vide)
                        nextPosIsInvalid = true;

                }
            }
            // Tentative d'autres directions
            if (nextPosIsInvalid)
                retour = true;
            else
            {
                _CurrPos = new Point(_CurrPos.X, _CurrPos.Y + 1);
                _Orientation = 6;
            }

            return retour;
        }


        /// <summary>
        /// Verify if a wall can be left there.
        /// Returns false and set current position if the wall can be correctly built.
        /// Returns true if no wall could ever be built here.
        /// </summary>
        bool CheckLeftWallBuilding()
        {
            bool retour = false;
            bool nextPosIsInvalid = false; // Basically another return but with more explicit name.


            // Section dégueulasse, mais simple.
            if (!nextPosIsInvalid)
            {
                if (_CurrPos.X <  2)
                {
                    nextPosIsInvalid = true;
                }
                else
                {
                    if (_CurrPos.Y != 0)
                    {
                        if (_Map[_CurrPos.X - 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X - 2, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.Y != _Map.Height - 1)
                    {
                        if (_Map[_CurrPos.X - 1, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X - 2, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.X < _Map.Width - 2 && _Map[_CurrPos.X, _CurrPos.Y].Type != CaseType.Vide)
                        nextPosIsInvalid = true;
                }
            }
            // Tentative d'autres directions
            if (nextPosIsInvalid)
                retour = true;
            else
            {
                _CurrPos = new Point(_CurrPos.X - 1, _CurrPos.Y);
                _Orientation = 9;
            }

            return retour;
        }


        bool UpperStart()
        {
            bool over = CheckUpperWallBuilding() &&
                CheckRightWallBuilding() &&
                CheckLeftWallBuilding();

            return over;
        }

        bool RightStart()
        {
            bool over = CheckRightWallBuilding() &&
                CheckDownWallBuilding() &&
                CheckUpperWallBuilding();

            return over;
        }

        bool DownStart()
        {
            bool over = CheckDownWallBuilding() &&
                CheckRightWallBuilding() &&
                CheckLeftWallBuilding();

            return over;
        }

        bool LeftStart()
        {
            bool over = CheckLeftWallBuilding() &&
                        CheckUpperWallBuilding() &&
                        CheckDownWallBuilding();

            return over;
        }
    }
}

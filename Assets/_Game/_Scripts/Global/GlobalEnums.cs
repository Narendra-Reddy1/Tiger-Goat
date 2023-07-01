using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SovereignStudios.Enums
{
    public enum PlayerPrefKeys
    {
    }
    public enum ResourceID
    {

    }
    public enum ResourceType
    {
        Coin,
        Tiger,
        Goat,

    }
    public enum Owner
    {
        Goat = 0,
        Tiger = 1,
        None = 2
    }
    public enum PlayerTurn
    {
        Goat = 0,
        Tiger = 1,
    }

    public enum GameMode
    {
        AI,
        LocalPlayer,
        Multiplayer,
    }

    public enum AIDifficultyLevel
    {
        Easy,
        Medium,
        Hard,
    }
    public enum GameResult
    {
        None,
        TigerWon,
        GoatWon,
        Draw,
    }
    public enum GameState
    {
        Live,
        Ended,
    }
    public enum GameDifficultyLevel
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
    }
    //public enum SelectionMode
    //{
    //    TigerMove = 0,
    //    GoatMove = 1,
    //    GoatOnboarding = 2,
    //}
    public enum Who
    {
        /// <summary>
        /// Currently clicked or Selected to move.
        /// </summary>
        Selected,
        /// <summary>
        /// Destination spotpoint for the selected spotpoint.
        /// </summary>
        TargetSpotPoint,
    }
    public enum DirectionFace
    {
        Left,
        Right,
        Top,
        Bottom,
    }

}

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
        Tiger = 0,
        Goat = 1
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
        Selected,
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

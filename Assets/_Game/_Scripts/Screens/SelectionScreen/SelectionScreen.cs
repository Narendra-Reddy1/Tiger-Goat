using SovereignStudios;
using SovereignStudios.Enums;
using SovereignStudios.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScreen : ScreenBase
{
    #region Variables

    [SerializeField] private Owner playingAs;
    [SerializeField] private GameDifficultyLevel gameDifficulty;
    [SerializeField] private SelectableObject tigerObject;
    [SerializeField] private SelectableObject goatObject;

    #endregion Variables

    #region Unity Methods

    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_ON_DIFFICULTY_LEVEL_SELECTED, Callback_On_Difficulty_Level_Selected);
        tigerObject.OnObjectSelected.AddListener(OnTigerSelected);
        goatObject.OnObjectSelected.AddListener(OnGoatSelected);
    }

    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_DIFFICULTY_LEVEL_SELECTED, Callback_On_Difficulty_Level_Selected);
        tigerObject.OnObjectSelected.RemoveListener(OnTigerSelected);
        goatObject.OnObjectSelected.RemoveListener(OnGoatSelected);
    }

    private void Start()
    {
        playingAs = Owner.Goat;
    }

    #endregion Unity Methods

    #region Public Methods

    public void OnClickPlayButton()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.GameplayScreen, onComplete: () =>
        {
            //Update AI difficulty level 
            //And AI MODE (Tiger or goat)
        }));
    }

    #endregion Public Methods

    #region Private Methods
    private void OnTigerSelected()
    {
        playingAs = Owner.Tiger;
    }
    private void OnGoatSelected()
    {
        playingAs = Owner.Goat;
    }
    #endregion Private Methods

    #region Callbacks

    private void Callback_On_Difficulty_Level_Selected(object args)
    {
        gameDifficulty = (GameDifficultyLevel)args;
    }

    #endregion Callbacks
}

using SovereignStudios.Enums;
using SovereignStudios.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyLevelToolTip : GenericToolTip
{
    #region Variables
    [SerializeField] private TextMeshProUGUI difficultyLevelTxt;
    [SerializeField] private UnityEngine.UI.Slider difficultyLevelSlider;
    #endregion Variables

    #region Unity Methods
    public override void OnEnable()
    {
        difficultyLevelSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public override void OnDisable()
    {
        difficultyLevelSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void Start()
    {
        OnSliderValueChanged(0);
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods
    private void OnSliderValueChanged(float value)
    {
        GameDifficultyLevel difficultyLevel = (GameDifficultyLevel)value;
        difficultyLevelTxt.SetText(difficultyLevel.ToString().ToUpper());
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_DIFFICULTY_LEVEL_SELECTED, difficultyLevel);
        Debug.Log($"Value: {value} : {difficultyLevel}");
    }
    #endregion Private Methods

    #region Callbacks

    #endregion Callbacks
}

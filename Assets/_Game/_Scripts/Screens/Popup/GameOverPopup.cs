using SovereignStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopup : PopupBase
{
    [SerializeField] private UnityEngine.UI.Image animalHolder;
    [SerializeField] private TMPro.TextMeshProUGUI messageTxt;
    [SerializeField] private SpriteManager spriteManager;

    private void Awake()
    {
        SetupPopup();
    }
    private void SetupPopup()
    {
        switch (WinOrDefeatHandler.gameResult)
        {
            case GameResult.GoatWon:
                animalHolder.sprite = spriteManager.resourcesDictionary[ResourceType.Goat];
                messageTxt.text = "Goat Wins";
                break;
            case GameResult.TigerWon:
                animalHolder.sprite = spriteManager.resourcesDictionary[ResourceType.Tiger];
                messageTxt.text = "Tiger Wins";
                break;
            case GameResult.Draw:
                messageTxt.text = "Match Draw!!";
                break;
            default:
                SovereignUtils.Log($"Incorrect GameResult: {WinOrDefeatHandler.gameResult}", SovereignStudios.LogType.Error);
                break;
        }
    }

    public void OnClickHome()
    {
        SovereignUtils.Log($"HOME CLICKED");
        //Ads
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CLOSE_LAST_ADDITIVE_SCREEN);
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, new ScreenChangeProperties(Window.MainMenu));
    }
    public void OnClickRestart()
    {
        //Ads
        //OnCompleting Ad Successfully restart the Game
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_CLOSE_LAST_ADDITIVE_SCREEN, new System.Tuple<System.Action>(() =>
        {
            GlobalEventHandler.TriggerEvent(EventID.EVENT_RESTART_LEVEL_REQUESTED);
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_LEVEL_STARTED);
        }));
    }
    public override void OnCloseClick()
    {

    }
}

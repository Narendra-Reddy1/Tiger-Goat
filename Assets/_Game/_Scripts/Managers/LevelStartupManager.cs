using SovereignStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartupManager : MonoBehaviour, IInitializer
{
    [SerializeField] private List<SpotPointBase> spotPoints;
    [SerializeField] private Owner defaultOwner;
    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_RESTART_LEVEL_REQUESTED, Callback_On_Level_Restarted);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_RESTART_LEVEL_REQUESTED, Callback_On_Level_Restarted);

    }
    private void Start()
    {
        Init();
    }


    public void Init()
    {
        foreach (SpotPointBase sp in spotPoints)
        {
            //sp.isOccupied = true;
            sp.ownerOfTheSpotPoint = defaultOwner;
            if (defaultOwner.Equals(Owner.Tiger)) sp.ShowTigerGraphic();
            else if (defaultOwner.Equals(Owner.Goat)) sp.ShowGoatGraphic();
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ANIMAL_ONBOARDED, sp);
        }
    }
    private void Callback_On_Level_Restarted(object args)
    {
        Init();
    }

}

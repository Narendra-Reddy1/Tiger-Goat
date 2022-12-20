using SovereignStudios;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPoint : SpotPointBase
{
    [SerializeField] private SpotPointBase down1;
    [SerializeField] private SpotPointBase down2;
    [SerializeField] private SpotPointBase down3;

    private void OnEnable()
    {
        inputHandler.PointerClickCallback.AddListener(Callback_On_SpotPoint_Clicked);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_CLICKED, Callback_On_Spotpoint_Clicked);
    }

    private void OnDisable()
    {
        inputHandler.PointerClickCallback.RemoveListener(Callback_On_SpotPoint_Clicked);
    }
    public override void OnClickSpotPoint()
    {
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_CLICKED);
        CheckForPossibleMoves(down1);
        CheckForPossibleMoves(down2);
        CheckForPossibleMoves(down3);
    }
    private void CheckForPossibleMoves(SpotPointBase pointBase)
    {
        SovereignStudios.SovereignUtils.Log($"!! Headpoint Possible ways");
        SpotPoint point = (SpotPoint)pointBase;
        if (point == null) return;
        if (point.isOccupied)//occupied
        {
            if (ownerOfTheSpotPoint.Equals(Owner.Goat) && point.ownerOfTheSpotPoint.Equals(Owner.Tiger))
            {
                //Goat can't Move....
            }
            else if (ownerOfTheSpotPoint.Equals(Owner.Tiger) && point.ownerOfTheSpotPoint.Equals(Owner.Goat))
            {
                if (point.neighborsDictionary[DirectionFace.Bottom] == null) return;
                if (!point.neighborsDictionary[DirectionFace.Bottom].isOccupied)
                {
                    if (!pointsAvailableToOccupy.Contains(point.neighborsDictionary[DirectionFace.Bottom]))
                        pointsAvailableToOccupy.Add(point.neighborsDictionary[DirectionFace.Bottom]);
                    point.neighborsDictionary[DirectionFace.Bottom].ShowCanOccupyGraphic();
                }
            }
            else if (ownerOfTheSpotPoint.Equals(point.neighborsDictionary[DirectionFace.Bottom].ownerOfTheSpotPoint))
            {

            }
        }
        else
        {
            if (!pointsAvailableToOccupy.Contains(point))
                pointsAvailableToOccupy.Add(point);
            point.ShowCanOccupyGraphic();
        }

    }

    private void Callback_On_SpotPoint_Clicked()
    {
        OnClickSpotPoint();
    }
    private void Callback_On_Spotpoint_Clicked(object arg)
    {
        HideCanOccupyGraphic();
        pointsAvailableToOccupy.Clear();
    }

}

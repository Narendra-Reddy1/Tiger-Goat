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
        //GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_CLICKED, Callback_On_Spotpoint_Clicked);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC, Callback_On_Hide_Can_Occupy_Graphic_Requested);
    }

    private void OnDisable()
    {
        inputHandler.PointerClickCallback.RemoveListener(Callback_On_SpotPoint_Clicked);
        //GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_CLICKED, Callback_On_Spotpoint_Clicked);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC, Callback_On_Hide_Can_Occupy_Graphic_Requested);
    }
    public override void OnClickSpotPoint()
    {
        //if (!this.ownerOfTheSpotPoint.Equals(Owner.None))
        //{

        //    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC);
        //    CheckForPossibleMoves(down1);
        //    CheckForPossibleMoves(down2);
        //    CheckForPossibleMoves(down3);
        //    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_CLICKED, GetDetails());
        //}
        switch (GameplayManager.GetPlayerTurn())
        {
            case PlayerTurn.Goat:
                if (ownerOfTheSpotPoint.Equals(Owner.None))
                {
                    if (!GameplayManager.AreGoatsOnboarded())
                    {
                        GameplayManager.IncrementGoatCount();
                        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_GOAT_ONBOARDING_REQUESTED, new System.Tuple<Vector3, System.Action>(goatGraphic.position, () =>
                        {
                            ShowGoatGraphic();
                            ownerOfTheSpotPoint = Owner.Goat;
                            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED);
                            //Aduio cue here
                        }));
                        //show the goat anim.
                        //tigger selection done event.
                        return;
                    }
                    if (GameplayManager.isSpotPointClicked)
                    {
                        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_SELECTED, GetInfo());
                    }
                }
                else if (ownerOfTheSpotPoint.Equals(Owner.Goat) && GameplayManager.AreGoatsOnboarded())
                {
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC);
                    CheckForPossibleMoves(down1);
                    CheckForPossibleMoves(down2);
                    CheckForPossibleMoves(down3);
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_CLICKED, GetDetails());
                }
                break;
            case PlayerTurn.Tiger:
                if (ownerOfTheSpotPoint.Equals(Owner.Tiger))
                {
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC);
                    CheckForPossibleMoves(down1);
                    CheckForPossibleMoves(down2);
                    CheckForPossibleMoves(down3);
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_CLICKED, GetDetails());
                }
                else if (ownerOfTheSpotPoint.Equals(Owner.None) && GameplayManager.isSpotPointClicked && GameplayManager.GetPointsAvailableToMoveList().Contains(this))
                {
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_SELECTED, GetInfo());
                }
                break;
        }



    }
    private void CheckForPossibleMoves(SpotPointBase pointBase)
    {
        SovereignUtils.Log($"!! Headpoint Possible ways");
        SpotPoint point = (SpotPoint)pointBase;
        if (point == null) return;
        if (!point.ownerOfTheSpotPoint.Equals(Owner.None))//occupied
        {
            if (ownerOfTheSpotPoint.Equals(Owner.Goat) && point.ownerOfTheSpotPoint.Equals(Owner.Tiger))
            {
                //Goat can't Move....
            }
            else if (ownerOfTheSpotPoint.Equals(Owner.Tiger) && point.ownerOfTheSpotPoint.Equals(Owner.Goat))
            {
                if (point.neighborsDictionary[DirectionFace.Bottom] == null) return;
                if (point.neighborsDictionary[DirectionFace.Bottom].ownerOfTheSpotPoint.Equals(Owner.None))
                {
                    if (!pointsAvailableToOccupy.Contains(point.neighborsDictionary[DirectionFace.Bottom]))
                        pointsAvailableToOccupy.Add(point.neighborsDictionary[DirectionFace.Bottom]);
                    point.neighborsDictionary[DirectionFace.Bottom].ShowCanOccupyGraphic();
                }
            }
            else if (ownerOfTheSpotPoint.Equals(point.ownerOfTheSpotPoint))
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
    //private void Callback_On_Spotpoint_Clicked(object arg)
    //{
    //    if (pointsAvailableToOccupy.Contains(this)) return;
    //    HideCanOccupyGraphic();
    //}
    private void Callback_On_Hide_Can_Occupy_Graphic_Requested(object args)
    {
        HideCanOccupyGraphic();
        pointsAvailableToOccupy.Clear();
    }
}

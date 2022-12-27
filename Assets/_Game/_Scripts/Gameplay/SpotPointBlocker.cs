using SovereignStudios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//this class should run after the tiger or goat positions are changed only.
public class SpotPointBlocker : MonoBehaviour
{
    [SerializeField] private List<SpotPointBase> occupiedSpotPoints;
    [SerializeField] private GameObject screenBlocker;

    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_ON_GOAT_ONBOARDING_REQUESTED, Callback_On_GoatOnboardin_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_SELECTED, Callback_On_Spotpoint_Selected);
        GlobalEventHandler.AddListener(EventID.EVENT_REQUEST_SCREEN_BLOCK, Callback_On_Screen_Blocker_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_REQUEST_SCREEN_UNBLOCK, Callback_On_Screen_Unblocker_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED, Callback_On_Selection_Ended);
        GlobalEventHandler.AddListener(EventID.EVENT_RESTART_LEVEL_REQUESTED, Callback_On_Level_Restart_Requested);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_ANIMAL_MOVED, Callback_Animal_Moved);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_GOAT_KILLED, Callback_Goat_Killed);
        GlobalEventHandler.AddListener(EventID.EVENT_ANIMAL_ONBOARDED, Callback_Goat_Onboarded);
    }

    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_GOAT_ONBOARDING_REQUESTED, Callback_On_GoatOnboardin_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_SELECTED, Callback_On_Spotpoint_Selected);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED, Callback_On_Selection_Ended);
        GlobalEventHandler.RemoveListener(EventID.EVENT_REQUEST_SCREEN_BLOCK, Callback_On_Screen_Blocker_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_REQUEST_SCREEN_UNBLOCK, Callback_On_Screen_Unblocker_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_RESTART_LEVEL_REQUESTED, Callback_On_Level_Restart_Requested);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_ANIMAL_MOVED, Callback_Animal_Moved);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_GOAT_KILLED, Callback_Goat_Killed);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ANIMAL_ONBOARDED, Callback_Goat_Onboarded);
    }

    public virtual void BlockTheCellIfNoWayToMove()
    {
        SovereignUtils.Log($"!!!! THIS SpotBlocker method");
        foreach (SpotPointBase spotPoint in occupiedSpotPoints)
        {
            SovereignUtils.Log($"!!!! Running loop for: {spotPoint.name}");
            if (spotPoint.name == "Point_0")
            {
                SovereignUtils.Log($"!!!! Blocking HeadPoint: {spotPoint.name}");
                var sp = (HeadPoint)spotPoint;
                sp.CheckForBlocking();
                goto InnerLoopEnd;
            }
            if (spotPoint.ownerOfTheSpotPoint.Equals(Owner.None))
            {
                occupiedSpotPoints.Remove(spotPoint);
                SovereignUtils.Log($"!!!! Removed empty spotPoint From blocker!!", SovereignStudios.LogType.Error);
                continue;
            }
            List<DirectionFace> directions = spotPoint.GetKeysOfTheNeighborDictionary().ToList();
            int generalOccupancies = 0;

            //if any of the neighbor is not occupied then it doesn't make sense to run the logic completely for the particular spotpoint.
            foreach (DirectionFace direction in directions)
            {
                if (spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.None))
                {
                    if (spotPoint.isBlocked)
                    {
                        SovereignUtils.Log($"!!!! Unblockign spotpoint: {spotPoint.name}");
                        ShowOrHideBlockingEffect(spotPoint, false);
                    }
                    SovereignUtils.Log($"!!!! check occupancies: {spotPoint.name} {direction}");
                    goto InnerLoopEnd;
                }
            }
            //if (spotPoint.ownerOfTheSpotPoint.Equals(Owner.Goat))
            //{
            //    foreach (DirectionFace direction in directions)
            //    {
            //        //spot point neighbor in top direction and neighbor the top direction is not occupied then skip the iteration.
            //        //if (!spotPoint.neighborsDictionary[direction].neighborsDictionary[direction].isOccupied) continue;
            //        if (spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Goat) || spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Tiger))
            //        {
            //            generalOccupancies++;
            //        }
            //    }
            //}
            //else 
            if (spotPoint.ownerOfTheSpotPoint.Equals(Owner.Tiger))
            {
                foreach (DirectionFace direction in directions)
                {
                    if (spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Goat))
                    {
                        if (!spotPoint.neighborsDictionary[direction].neighborsDictionary.Contains(direction))
                        {
                            generalOccupancies++;
                        }
                        else if (spotPoint.neighborsDictionary[direction].neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Goat) ||
                            spotPoint.neighborsDictionary[direction].neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Tiger))
                        {
                            generalOccupancies++;
                        }
                    }
                    else if (spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Tiger))
                    {
                        generalOccupancies++;
                    }
                }
                if (generalOccupancies == directions.Count && !spotPoint.isBlocked)
                {
                    SovereignUtils.Log($"!!!! blocking spotpoint: {spotPoint.name}");
                    ShowOrHideBlockingEffect(spotPoint, true);
                }

            }
            SovereignUtils.Log($"!!!! {spotPoint.name} Occupancies: {generalOccupancies}. Dir. count: {directions.Count}. Can it be blocked?: {directions.Count == generalOccupancies}");
        InnerLoopEnd:
            SovereignUtils.Log($"!!!! InnerLoop end {spotPoint.name}");
        }
    }
    private void ShowOrHideBlockingEffect(SpotPointBase spotPoint, bool status)
    {
        spotPoint.isBlocked = status;
        if (status) spotPoint.ShowTigerGrayscaleEffect();
        else spotPoint.HideTigerGrayscaleEffect();
    }
    private void BlockScreen()
    {
        screenBlocker.SetActive(true);
    }
    private void UnblockScreen()
    {
        screenBlocker.SetActive(false);
    }

    #region Callbacks

    private void Callback_On_Screen_Unblocker_Requested(object arg)
    {
        UnblockScreen();
    }
    private void Callback_On_Screen_Blocker_Requested(object arg)
    {
        BlockScreen();
    }
    private void Callback_On_GoatOnboardin_Requested(object arg)
    {
        BlockScreen();
    }
    private void Callback_On_Spotpoint_Selected(object arg)
    {
        BlockScreen();
    }
    private void Callback_On_Selection_Ended(object args)
    {
        UnblockScreen();
        BlockTheCellIfNoWayToMove();
    }
    private void Callback_On_Level_Restart_Requested(object arg)
    {
        foreach (SpotPointBase spotPoint in occupiedSpotPoints)
        {
            spotPoint.UpdateOwner(Owner.None);
            spotPoint.HideAnimalGraphic();
        }
        occupiedSpotPoints.Clear();
    }

    private void Callback_Goat_Onboarded(object args)
    {
        occupiedSpotPoints.Add((SpotPointBase)args);
    }

    private void Callback_Goat_Killed(object args)
    {
        occupiedSpotPoints.Remove((SpotPointBase)args);
    }

    private void Callback_Animal_Moved(object args)
    {
        var table = args as Hashtable;
        occupiedSpotPoints.Add((SpotPointBase)table[Who.TargetSpotPoint]);
        occupiedSpotPoints.Remove((SpotPointBase)table[Who.Selected]);

    }

    #endregion Callbacks
}

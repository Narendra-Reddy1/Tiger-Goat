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
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_SELECTED, Callback_On_Spotpoint_Selected);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED, Callback_On_Selection_Ended);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_SELECTED, Callback_On_Spotpoint_Selected);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED, Callback_On_Selection_Ended);
    }

    public virtual void BlockTheCellIfNoWayToMove()
    {
        SovereignUtils.Log($"++ THIS SpotBlocker method");
        foreach (SpotPointBase spotPoint in occupiedSpotPoints)
        {
            if (spotPoint.ownerOfTheSpotPoint.Equals(Owner.None)) continue;
            List<DirectionFace> directions = spotPoint.GetKeysOfTheNeighborDictionary().ToList();
            int generalOccupancies = 0;

            //if any of the neighbor is not occupied then it doesn't make sense to run the logic completely for the particular spotpoint.
            foreach (DirectionFace direction in directions)
            {
                if (spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.None)) 
                {
                    SovereignUtils.Log($"check occupancies: {directions.IndexOf(direction)}");
                    goto InnerLoopEnd;
                }
            }
            if (spotPoint.ownerOfTheSpotPoint.Equals(Owner.Goat))
            {
                foreach (DirectionFace direction in directions)
                {
                    //spot point neighbor in top direction and neighbor the top direction is not occupied then skip the iteration.
                    //if (!spotPoint.neighborsDictionary[direction].neighborsDictionary[direction].isOccupied) continue;
                    if (spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Goat) || spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Tiger))
                    {
                        generalOccupancies++;
                    }
                }
            }
            else if (spotPoint.ownerOfTheSpotPoint.Equals(Owner.Tiger))
            {
                foreach (DirectionFace direction in directions)
                {
                    if (spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Goat))
                    {
                        if (spotPoint.neighborsDictionary[direction].neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Goat) ||
                            spotPoint.neighborsDictionary[direction].neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Tiger))
                        {
                            generalOccupancies++;
                        }
                    }
                    if (spotPoint.neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Tiger))
                    {
                        generalOccupancies++;
                    }
                }
            }
            SovereignUtils.Log($"{spotPoint.name} Occupancies: {generalOccupancies}. Dir. count: {directions.Count}. Can it be blocked?: {directions.Count == generalOccupancies}");
        InnerLoopEnd:
            SovereignUtils.Log($"InnerLoop end {spotPoint.name}");
        }
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
    private void Callback_On_Spotpoint_Selected(object arg)
    {
        BlockScreen();
    }
    private void Callback_On_Selection_Ended(object args)
    {
        UnblockScreen();
        BlockTheCellIfNoWayToMove();
    }
    #endregion Callbacks
}

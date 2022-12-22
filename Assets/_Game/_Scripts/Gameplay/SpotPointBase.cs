using SovereignStudios;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpotPointBase : MonoBehaviour
{

    #region Variables 

    //public bool isOccupied = false;
    public bool isBlocked = false;
    public List<SpotPointBase> pointsAvailableToOccupy;
    public UnityEngine.UI.Image canOccupyOverlayImage;
    public RectTransform tigerGraphic;
    public RectTransform goatGraphic;
    public Owner ownerOfTheSpotPoint = Owner.None;
    //public Neighbors neighbors;
    [Space(10)]
    public NeighborsDictionary neighborsDictionary;
    [Space(10)]
    public InputPointerHandler inputHandler;
    #endregion Variables
    //private void OnValidate()
    //{
    //    if (this.name == "HeadPoint")
    //    {
    //        return;
    //    }
    //    if (neighbors.left != null)
    //        neighborsDictionary.Add(DirectionFace.Left, neighbors.left);
    //    if (neighbors.right != null)
    //        neighborsDictionary.Add(DirectionFace.Right, neighbors.right);
    //    if (neighbors.up != null)
    //        neighborsDictionary.Add(DirectionFace.Top, neighbors.up);
    //    if (neighbors.down != null)
    //        neighborsDictionary.Add(DirectionFace.Bottom, neighbors.down);
    //}
    #region Methods

    public virtual void CheckForVacancy(DirectionFace direction)
    {
        if (!neighborsDictionary.Contains(direction)) return;
        if (!neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.None))
        {
            if (ownerOfTheSpotPoint.Equals(Owner.Goat) && neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Tiger))
            {
                //Goat can't Move....
            }
            else if (ownerOfTheSpotPoint.Equals(Owner.Tiger) && neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Goat))
            {
                if (!neighborsDictionary[direction].neighborsDictionary.Contains(direction)) return;
                if (neighborsDictionary[direction].neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.None))//can kill goat
                {
                    if (!pointsAvailableToOccupy.Contains(neighborsDictionary[direction].neighborsDictionary[direction]))
                        pointsAvailableToOccupy.Add(neighborsDictionary[direction].neighborsDictionary[direction]);
                    neighborsDictionary[direction].neighborsDictionary[direction].ShowCanOccupyGraphic();
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_GOAT_DEAD_POINT_DETECTED, new System.Tuple<SpotPointBase, SpotPointBase>(neighborsDictionary[direction], neighborsDictionary[direction].neighborsDictionary[direction]));
                }
            }
            else if (ownerOfTheSpotPoint.Equals(neighborsDictionary[direction].ownerOfTheSpotPoint))
            {

            }
        }
        else
        {
            if (!pointsAvailableToOccupy.Contains(neighborsDictionary[direction]))
                pointsAvailableToOccupy.Add(neighborsDictionary[direction]);
            neighborsDictionary[direction].ShowCanOccupyGraphic();
        }
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOT_POINTS_AVAILABLE_TO_OCCUPY, pointsAvailableToOccupy);
        SovereignUtils.Log($"++ Check for vacancy from Base class");
    }
    public void UnblockBlockedCellsInNeighborsIfAny()
    {
        List<DirectionFace> directions = GetKeysOfTheNeighborDictionary().ToList();
        foreach (DirectionFace direction in directions)
        {
            if (neighborsDictionary[direction].isBlocked)
            {
                neighborsDictionary[direction].isBlocked = false;
            }
            if (!neighborsDictionary[direction].neighborsDictionary.Contains(direction)) return;
            else
            {
                neighborsDictionary[direction].neighborsDictionary[direction].isBlocked = false;
            }
        }
    }
    public void ShowTigerGraphic()
    {
        tigerGraphic.gameObject.SetActive(true);
    }
    public void HideTigerGraphic()
    {
        tigerGraphic.gameObject.SetActive(false);
    }

    public void ShowGoatGraphic()
    {
        goatGraphic.gameObject.SetActive(true);
    }
    public void HideGoatGraphic()
    {
        goatGraphic.gameObject.SetActive(false);
    }
    public void UpdateOwner(Owner owner)
    {
        ownerOfTheSpotPoint = owner;
    }
    public ICollection<DirectionFace> GetKeysOfTheNeighborDictionary()
    {
        return neighborsDictionary.Keys;
    }
    public virtual void OnClickSpotPoint()
    {

    }
    public Hashtable GetInfo()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add(GameplayManager.Who.TargetSpotPoint, this);
        return hashtable;
    }
    public Hashtable GetDetails()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add(GameplayManager.Who.Selected, this);
        return hashtable;
    }
    public virtual void ShowCanOccupyGraphic()
    {
        canOccupyOverlayImage.enabled = true;
        SovereignUtils.Log($"++ Showing CanOccupyGraphic {transform.name}");
    }
    public virtual void HideCanOccupyGraphic()
    {
        canOccupyOverlayImage.enabled = false;
    }
    #endregion
}
[System.Serializable]
public class NeighborsDictionary : SerializableDictionary<DirectionFace, SpotPointBase> { }
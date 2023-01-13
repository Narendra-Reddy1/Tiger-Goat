using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SovereignStudios;
using SovereignStudios.Enums;
using SovereignStudios.EventSystem;
using SovereignStudios.Utils;


public class SpotPointBase : MonoBehaviour
{

    #region Variables 

    //public bool isOccupied = false;
    public SpriteManager spriteManager;
    public bool isBlocked = false;
    public List<SpotPointBase> pointsAvailableToOccupy;
    public UnityEngine.UI.Image canOccupyOverlayImage;
    public RectTransform animalGraphicTransform;
    public UIEffect tigerUiEffect;
    public Owner ownerOfTheSpotPoint = Owner.None;
    //public Neighbors neighbors;
    [Space(10)]
    public NeighborsDictionary neighborsDictionary;
    [Space(10)]
    public InputPointerHandler inputHandler;
    [HideInInspector]
    public UnityEngine.UI.Image animalGraphic;

    public readonly Vector3 tigerScale = Vector3.one * 0.7f;
    #endregion Variables

    #region Unity Methods

    public virtual void OnEnable()
    {
        animalGraphicTransform.TryGetComponent(out animalGraphic);
    }
    public virtual void OnDisable()
    {

    }
    #endregion Unity Methods


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
            //if (ownerOfTheSpotPoint.Equals(Owner.Goat) && neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Tiger))
            //{
            //    //Goat can't Move....
            //}
            if (ownerOfTheSpotPoint.Equals(Owner.Tiger) && neighborsDictionary[direction].ownerOfTheSpotPoint.Equals(Owner.Goat))
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
            //else if (ownerOfTheSpotPoint.Equals(neighborsDictionary[direction].ownerOfTheSpotPoint))
            //{

            //}
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


    public void ShowAnimalGraphic(bool isTiger)
    {
        animalGraphic.gameObject.SetActive(true);
        if (isTiger)
            animalGraphicTransform.localScale = tigerScale;
        else
            animalGraphicTransform.localScale = Vector3.one;
        animalGraphic.sprite = spriteManager.resourcesDictionary[isTiger ? ResourceType.Tiger : ResourceType.Goat];
    }
    public void HideAnimalGraphic()
    {
        animalGraphic.gameObject.SetActive(false);
    }

    //public void HideAnimalGraphic()
    //{
    //    HideGoatGraphic();
    //    HideTigerGraphic();
    //}
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
    public Hashtable GetInfo()//selected point to move
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add(Who.TargetSpotPoint, this);
        return hashtable;
    }
    public Hashtable GetDetails()//clicked point
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add(Who.Selected, this);
        return hashtable;
    }
    public virtual void ShowCanOccupyGraphic()
    {
        if (canOccupyOverlayImage)
            canOccupyOverlayImage.enabled = true;
        SovereignUtils.Log($"++ Showing CanOccupyGraphic {transform.name}");
    }
    public virtual void HideCanOccupyGraphic()
    {
        if (canOccupyOverlayImage)
            canOccupyOverlayImage.enabled = false;
    }
    public virtual void ShowTigerGrayscaleEffect()
    {
        if (Owner.Tiger == ownerOfTheSpotPoint)
        {
            tigerUiEffect.effectMode = EffectMode.Grayscale;
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_TIGER_BLOCKED);
        }
    }
    public virtual void HideTigerGrayscaleEffect()
    {
        if (Owner.Tiger == ownerOfTheSpotPoint)
        {
            tigerUiEffect.effectMode = EffectMode.None;
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_TIGER_UNBLOCKED);
        }

    }
    #endregion
}

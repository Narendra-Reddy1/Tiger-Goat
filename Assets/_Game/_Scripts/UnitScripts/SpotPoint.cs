using UnityEngine;
using SovereignStudios;
//Prototype Class

public class SpotPoint : SpotPointBase, IInitializer
{

    /*
 * Neighbour spot points
 * To show an overlay image when any neighbour point is selected and if the player can move to this spot point.(to show the possible ways to move)
 * Boolean to check whether the spotpoint is occupied or not.
 * Spotpoint Owner.
 * For this spotpoint it should switch between two states
 * i)When this point is not occupied then this point should act like a button to select .
 *** Even in this selection mode also if select this point in tiger selection or goat selection mode then we should check wether this point is the neighbour or not.
 *** If it is goat selection mode then we should check wether the goats are in onboarding state or not. if goats are in onboarding state we can directly place the goat in this spotpoint(only if it not occupied.)
 * 
 * 
 */

    #region Variables
    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        inputHandler.PointerClickCallback.AddListener(Callback_On_SpotPoint_Clicked);
        GlobalEventHandler.AddListener(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC, Callback_On_Hide_Can_Occupy_Graphic_Requested);
    }
    private void OnDisable()
    {
        inputHandler.PointerClickCallback.RemoveListener(Callback_On_SpotPoint_Clicked);
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC, Callback_On_Hide_Can_Occupy_Graphic_Requested);
    }
    #endregion Unity Methods

    #region Public Methods
    public void Init()
    {
        SovereignUtils.Log($"!! Init from Spotpoint");
    }
    public override void OnClickSpotPoint()
    {
        switch (GameplayManager.GetPlayerTurn())
        {
            case PlayerTurn.Goat:
                if (ownerOfTheSpotPoint.Equals(Owner.None))
                {
                    if (!GameplayManager.AreGoatsOnboarded())
                    {
                        GameplayManager.IncrementGoatCount();
                        SovereignUtils.Log($"Goat onboarding...");
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
                    CheckForPossibleMoves(DirectionFace.Left);
                    CheckForPossibleMoves(DirectionFace.Right);
                    CheckForPossibleMoves(DirectionFace.Top);
                    CheckForPossibleMoves(DirectionFace.Bottom);
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_CLICKED, GetDetails());
                }
                break;
            case PlayerTurn.Tiger:
                if (ownerOfTheSpotPoint.Equals(Owner.Tiger))
                {
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC);
                    CheckForPossibleMoves(DirectionFace.Left);
                    CheckForPossibleMoves(DirectionFace.Right);
                    CheckForPossibleMoves(DirectionFace.Top);
                    CheckForPossibleMoves(DirectionFace.Bottom);
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_CLICKED, GetDetails());
                }
                else if (ownerOfTheSpotPoint.Equals(Owner.None) && GameplayManager.isSpotPointClicked && GameplayManager.GetPointsAvailableToMoveList().Contains(this))
                {
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_SELECTED, GetInfo());
                }
                break;
        }
    }

    #endregion Public Methods

    #region Private Methods
    //private void CheckForVacancy(SpotPointBase currentPoint, SpotPointBase affiliatePoint)
    //{
    //    if (affiliatePoint == null) return;
    //}
    private void CheckForPossibleMoves(DirectionFace direction)
    {
        base.CheckForVacancy(direction);
    }
    #endregion Private Methods

    #region Callbacks
    private void Callback_On_SpotPoint_Clicked()
    {
        OnClickSpotPoint();
    }
    private void Callback_On_Hide_Can_Occupy_Graphic_Requested(object args)
    {
        HideCanOccupyGraphic();
        pointsAvailableToOccupy.Clear();
    }
    #endregion
}

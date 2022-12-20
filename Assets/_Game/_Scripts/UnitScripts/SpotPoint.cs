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
    private PlayerTurn playerTurn;
    private SelectionMode selectionMode;
    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_CLICKED, Callback_On_SpotPoint_Clicked);
        inputHandler.PointerClickCallback.AddListener(Callback_On_SpotPoint_Clicked);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_CLICKED, Callback_On_SpotPoint_Clicked);
        inputHandler.PointerClickCallback.RemoveListener(Callback_On_SpotPoint_Clicked);
    }
    #endregion Unity Methods

    #region Public Methods
    public void Init()
    {
        SovereignUtils.Log($"!! Init from Spotpoint");
    }
    public override void OnClickSpotPoint()
    {
        if (!isOccupied && !selectionMode.Equals(SelectionMode.GoatOnboarding)) return;
        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_CLICKED);
        CheckForPossibleMoves(DirectionFace.Left);
        CheckForPossibleMoves(DirectionFace.Right);
        CheckForPossibleMoves(DirectionFace.Top);
        CheckForPossibleMoves(DirectionFace.Bottom);
        switch (playerTurn)
        {
            case PlayerTurn.Goat:

                break;
            case PlayerTurn.Tiger:
                if (isOccupied && ownerOfTheSpotPoint.Equals(Owner.Tiger))
                {

                }
                break;
        }
    }

    #endregion Public Methods

    #region Private Methods
    private void CheckForVacancy(SpotPointBase currentPoint, SpotPointBase affiliatePoint)
    {
        if (affiliatePoint == null) return;
    }
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

    private void Callback_On_SpotPoint_Clicked(object arg)
    {
        HideCanOccupyGraphic();
        pointsAvailableToOccupy.Clear();
    }

    #endregion
}

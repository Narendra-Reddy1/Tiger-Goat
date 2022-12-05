using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Prototype Class
public class SpotPoint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    public enum Owner
    {
        Goat = 0,
        Tiger = 1,
        None = 2
    }
    public enum SpotPointState
    {

    }
    [SerializeField] private List<SpotPoint> myNeighbourSpotPoints;
    [SerializeField] private GameObject canOccupyOverlayImage;
    [SerializeField] private bool isOccupied = false;
    [SerializeField] private Owner ownerOfTheSpotPoint = Owner.None;




    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}

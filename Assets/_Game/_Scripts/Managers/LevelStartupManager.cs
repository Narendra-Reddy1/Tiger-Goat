using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartupManager : MonoBehaviour
{
    [SerializeField] private List<SpotPointBase> spotPoints;
    [SerializeField] private SovereignStudios.Owner defaultOwner;
    private void Start()
    {
        foreach (SpotPointBase sp in spotPoints)
        {
            //sp.isOccupied = true;
            sp.ownerOfTheSpotPoint = defaultOwner;
            if (defaultOwner.Equals(SovereignStudios.Owner.Tiger)) sp.ShowTigerGraphic();
            else if (defaultOwner.Equals(SovereignStudios.Owner.Goat)) sp.ShowGoatGraphic();
        }
    }
}

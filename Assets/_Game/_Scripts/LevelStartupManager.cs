using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartupManager : MonoBehaviour
{
    [SerializeField] private List<SpotPointBase> spotPoints;
    private void Start()
    {
        foreach (SpotPointBase sp in spotPoints)
        {
            sp.ShowTigerGraphic();
            //sp.isOccupied = true;
            sp.ownerOfTheSpotPoint = SovereignStudios.Owner.Tiger;
        }
    }
}

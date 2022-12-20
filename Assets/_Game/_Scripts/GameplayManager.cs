using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SovereignStudios
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private PlayerTurn playerTurn;
        [SerializeField] private List<SpotPointBase> spotPointsAvailableToMove;

        private List<SpotPointBase> GetPointsAvailableToMoveList()
        {
            return spotPointsAvailableToMove;
        }
    }
}
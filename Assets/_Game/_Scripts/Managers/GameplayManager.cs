using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SovereignStudios
{
    public class GameplayManager : MonoBehaviour
    {
        private static PlayerTurn playerTurn = PlayerTurn.Goat;
        // private static SelectionMode selectionMode;
        private static List<SpotPointBase> spotPointsAvailableToMove = new List<SpotPointBase>();
        [SerializeField] private RectTransform goatAnim;

        private RectTransform currentAnimal;
        private List<SpotPointBase> avilablePos;
        private RectTransform targetGraphic;
        private Vector3 targetPos;
        private Vector3 myPrevPos;
        private bool isTiger;
        private bool isGoat;
        private SpotPointBase selectedPoint;
        private SpotPointBase targetPoint;
        private static byte noOfGoatsPlacedOnBoard = 0;
        private static byte noOfGoatsDied = 0;
        private SpotPointBase goatDeadPoint;
        private SpotPointBase goatPoint;
        public enum Who
        {
            Selected,
            TargetSpotPoint,
        }

        public static bool isSpotPointClicked = false;

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_CLICKED, Callback_On_SpotPoint_Clicked);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_SELECTED, MoveTheAnimal);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED, Callback_On_Selection_Ended);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_GOAT_ONBOARDING_REQUESTED, Callback_On_Goat_Onboarding_Anim_Requested);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_GOAT_DEAD_POINT_DETECTED, Callback_On_Goat_Dead_Point_Detected);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOT_POINTS_AVAILABLE_TO_OCCUPY, Callback_On_Spotpoints_Avail_To_Occupy);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_CLICKED, Callback_On_SpotPoint_Clicked);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_SELECTED, MoveTheAnimal);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED, Callback_On_Selection_Ended);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_GOAT_ONBOARDING_REQUESTED, Callback_On_Goat_Onboarding_Anim_Requested);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_GOAT_DEAD_POINT_DETECTED, Callback_On_Goat_Dead_Point_Detected);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOT_POINTS_AVAILABLE_TO_OCCUPY, Callback_On_Spotpoints_Avail_To_Occupy);
        }
        private void MoveTheAnimal(object args)
        {
            Hashtable hashtable = (Hashtable)args;
            if (hashtable.ContainsKey(Who.Selected))
            {
                selectedPoint = (SpotPointBase)hashtable[Who.Selected];
                //need check what if owneris none. I dont think it is need cause if owner is none the logic won't even execute to this point
                isTiger = selectedPoint.ownerOfTheSpotPoint == Owner.Tiger;
                isGoat = selectedPoint.ownerOfTheSpotPoint == Owner.Goat;
                currentAnimal = isTiger ? selectedPoint.tigerGraphic : selectedPoint.goatGraphic;
                avilablePos = selectedPoint.pointsAvailableToOccupy;
                myPrevPos = currentAnimal.position;
                SovereignUtils.Log($"Move TheAnimal CurrentGraphic event: {currentAnimal}, {avilablePos} ,{avilablePos.Count}, {myPrevPos}");
            }
            else if (hashtable.ContainsKey(Who.TargetSpotPoint))
            {
                targetPoint = (SpotPointBase)hashtable[Who.TargetSpotPoint];
                //targetGraphic = 
                targetPos = isTiger ? targetPoint.tigerGraphic.position : targetPoint.goatGraphic.position;
                if (!avilablePos.Contains(targetPoint)) return;
                if (targetPoint.Equals(goatDeadPoint))//Killing the goat 
                {
                    goatPoint.HideGoatGraphic();
                    goatPoint.ownerOfTheSpotPoint = Owner.None;
                    UpdateDeadGoatCount();
                }
                if (currentAnimal != null)
                {
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC);
                    currentAnimal.DOMove(targetPos, 1f, true).onComplete += () =>
                    {
                        if (isTiger) targetPoint.ShowTigerGraphic();
                        else if (isGoat) targetPoint.ShowGoatGraphic();
                        currentAnimal.DOMove(myPrevPos, 0);
                        currentAnimal.gameObject.SetActive(false);
                        targetPoint.UpdateOwner(selectedPoint.ownerOfTheSpotPoint); ;
                        //targetPoint.isOccupied = true;
                        selectedPoint.UpdateOwner(Owner.None);
                        // selectedPoint.isOccupied = false;
                        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED);
                        avilablePos.Clear();

                    };
                }
                SovereignUtils.Log($"Move TheAnimal CurrentGraphic event: {currentAnimal}, {avilablePos}, {myPrevPos}");
            }
        }
        private void ShowGoatOnboardingAnim(Vector3 targetPos, System.Action onComplete)
        {
            Vector3 myPrevPos = goatAnim.position;
            goatAnim.gameObject.SetActive(true);
            goatAnim.DOMove(targetPos, 1.1f, true).onComplete += () =>
            {
                goatAnim.DOMove(myPrevPos, 0f).onComplete += () =>
                {
                    goatAnim.gameObject.SetActive(false);
                };
                onComplete?.Invoke();

            };
        }
        #region Public Methods
        public static List<SpotPointBase> GetPointsAvailableToMoveList()
        {
            return spotPointsAvailableToMove;
        }
        public static byte GetGoatsOnTheBoard() => noOfGoatsPlacedOnBoard;
        public static PlayerTurn GetPlayerTurn() => playerTurn;
        public static void SwitchPlayerTurn()
        {
            if (playerTurn == PlayerTurn.Goat)
            {
                playerTurn = PlayerTurn.Tiger;
                GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_TIGER_TURN);
            }
            else
            {
                playerTurn = PlayerTurn.Goat;
                GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_GOAT_TURN);
            }
            SovereignUtils.Log($"Player Switch: {playerTurn}");
        }
        public static bool AreGoatsOnboarded() => noOfGoatsPlacedOnBoard >= Constants.NUMBER_OF_GOATS_IN_THE_GAME;
        public static void IncrementPlacedGoatCount()
        {
            Mathf.Clamp(++noOfGoatsPlacedOnBoard, 0, Constants.NUMBER_OF_GOATS_IN_THE_GAME);
            SovereignUtils.Log($"{noOfGoatsPlacedOnBoard} goats placed on board");
        }
        public static void UpdateDeadGoatCount()
        {
            noOfGoatsDied++;
        }
        public static byte GetDeadGoatsCount() => noOfGoatsDied;

        #endregion Public Methods
        #region Callbacks
        private void Callback_On_Goat_Onboarding_Anim_Requested(object args)
        {
            System.Tuple<Vector3, System.Action> tuple = (System.Tuple<Vector3, System.Action>)args;
            ShowGoatOnboardingAnim(tuple.Item1, tuple.Item2);
        }
        private void Callback_On_SpotPoint_Clicked(object args)
        {
            MoveTheAnimal(args);
            isSpotPointClicked = true;
        }
        private void Callback_On_Goat_Dead_Point_Detected(object args)
        {
            System.Tuple<SpotPointBase, SpotPointBase> tuple = (System.Tuple<SpotPointBase, SpotPointBase>)args;
            goatPoint = tuple.Item1;
            goatDeadPoint = tuple.Item2;
        }
        private void Callback_On_Selection_Ended(object args)
        {
            SwitchPlayerTurn();
            isSpotPointClicked = false;
        }
        private void Callback_On_Spotpoints_Avail_To_Occupy(object args)
        {
            List<SpotPointBase> list = (List<SpotPointBase>)args;
            spotPointsAvailableToMove.Clear();
            spotPointsAvailableToMove.AddRange(list);
        }
        #endregion Callbacks
    }
}
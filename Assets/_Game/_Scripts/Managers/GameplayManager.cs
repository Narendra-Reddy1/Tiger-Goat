using DG.Tweening;
using SovereignStudios.Enums;
using SovereignStudios.EventSystem;
using SovereignStudios.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SovereignStudios
{
    /// <summary>
    /// This class is resoponsible for handling player turn. killing the goat.
    /// showing goat onboarding animation. It also keep tracks of no.og goats on the board,
    /// no of goats killed and no.of tigers tied by goats.
    /// </summary>
    public class GameplayManager : MonoBehaviour, IInitializer
    {
        private static PlayerTurn playerTurn;
        private static GameState gameState;
        // private static SelectionMode selectionMode;
        private static List<SpotPointBase> spotPointsAvailableToMove = new List<SpotPointBase>();
        [SerializeField] private RectTransform goatAnim;


        private RectTransform currentAnimal;
        private List<SpotPointBase> avilablePos;
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

        public static bool isSpotPointClicked = false;
        public static byte noOfTigersBlocked = 0;

        private void Start()
        {
            Init();
        }
        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_CLICKED, Callback_On_SpotPoint_Clicked);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_SELECTED, MoveTheAnimal);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED, Callback_On_Selection_Ended);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_GOAT_ONBOARDING_REQUESTED, Callback_On_Goat_Onboarding_Anim_Requested);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_GOAT_DEAD_POINT_DETECTED, Callback_On_Goat_Dead_Point_Detected);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_SPOT_POINTS_AVAILABLE_TO_OCCUPY, Callback_On_Spotpoints_Avail_To_Occupy);
            GlobalEventHandler.AddListener(EventID.EVENT_REQUEST_TO_CHANGE_PLAYER_TURN, Callback_On_Player_Turn_Change_Requested);
            GlobalEventHandler.AddListener(EventID.EVENT_RESTART_LEVEL_REQUESTED, Callback_On_Level_Restart_Requested);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_TIGER_BLOCKED, Callback_On_Tiger_Blocked);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_TIGER_UNBLOCKED, Callback_On_Tiger_Unblocked);

        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_CLICKED, Callback_On_SpotPoint_Clicked);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_SELECTED, MoveTheAnimal);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED, Callback_On_Selection_Ended);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_GOAT_ONBOARDING_REQUESTED, Callback_On_Goat_Onboarding_Anim_Requested);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_GOAT_DEAD_POINT_DETECTED, Callback_On_Goat_Dead_Point_Detected);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SPOT_POINTS_AVAILABLE_TO_OCCUPY, Callback_On_Spotpoints_Avail_To_Occupy);
            GlobalEventHandler.RemoveListener(EventID.EVENT_REQUEST_TO_CHANGE_PLAYER_TURN, Callback_On_Player_Turn_Change_Requested);
            GlobalEventHandler.RemoveListener(EventID.EVENT_RESTART_LEVEL_REQUESTED, Callback_On_Level_Restart_Requested);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_TIGER_BLOCKED, Callback_On_Tiger_Blocked);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_TIGER_UNBLOCKED, Callback_On_Tiger_Unblocked);
        }

        /// <summary>
        /// This method is responsible to move the selected animal to the given target point.
        /// </summary>
        /// <param name="args"></param>
        private void MoveTheAnimal(object args)
        {
            Hashtable hashtable = (Hashtable)args;
            try
            {
                if (hashtable.ContainsKey(Who.Selected))
                {
                    selectedPoint = (SpotPointBase)hashtable[Who.Selected];
                    //need check what if owneris none. I dont think it is need cause if owner is none the logic won't even execute to this point
                    isTiger = selectedPoint.ownerOfTheSpotPoint == Owner.Tiger;
                    isGoat = selectedPoint.ownerOfTheSpotPoint == Owner.Goat;
                    currentAnimal = isTiger ? selectedPoint.animalGraphicTransform : selectedPoint.animalGraphicTransform;
                    avilablePos = selectedPoint.pointsAvailableToOccupy;
                    myPrevPos = currentAnimal.position;
                    SovereignUtils.Log($"Move TheAnimal CurrentGraphic event: {currentAnimal}, {avilablePos} ,{avilablePos.Count}, {myPrevPos}");
                }
                else if (hashtable.ContainsKey(Who.TargetSpotPoint))
                {
                    targetPoint = (SpotPointBase)hashtable[Who.TargetSpotPoint];
                    //targetGraphic = 
                    targetPos = isTiger ? targetPoint.animalGraphicTransform.position : targetPoint.animalGraphicTransform.position;
                    if (!avilablePos.Contains(targetPoint)) return;
                    if (targetPoint.Equals(goatDeadPoint))//Killing the goat 
                    {
                        goatPoint.HideAnimalGraphic();
                        goatPoint.ownerOfTheSpotPoint = Owner.None;
                        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_GOAT_KILLED, goatPoint);
                        UpdateDeadGoatCount();
                        goatPoint = null;
                        goatDeadPoint = null;
                    }
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC);
                    hashtable.Add(Who.Selected, selectedPoint);//just for the sake of below event.
                    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_ANIMAL_MOVED, hashtable);
                    var c = (Canvas)currentAnimal.gameObject.AddComponent(typeof(Canvas));
                    c.overrideSorting = true;
                    c.sortingOrder = 40;
                    currentAnimal.DOMove(targetPos, 1f, true).onComplete += () =>
                    {
                        if (isTiger) targetPoint.ShowAnimalGraphic(true);
                        else if (isGoat) targetPoint.ShowAnimalGraphic(false);
                        currentAnimal.DOMove(myPrevPos, 0);
                        Destroy(currentAnimal.GetComponent<Canvas>());
                        currentAnimal.gameObject.SetActive(false);
                        targetPoint.UpdateOwner(selectedPoint.ownerOfTheSpotPoint); ;
                        //targetPoint.isOccupied = true;
                        selectedPoint.UpdateOwner(Owner.None);
                        // selectedPoint.isOccupied = false;
                        GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_SPOTPOINT_SELECTION_ENDED);
                        avilablePos.Clear();

                    };
                    SovereignUtils.Log($"Move TheAnimal CurrentGraphic event: {currentAnimal}, {avilablePos}, {myPrevPos}");
                }
            }
            catch (System.Exception e)
            {
                SovereignUtils.Log($"{e.Message} stackTrace: {e.StackTrace}", LogType.Error);
            }
        }
        private void ShowGoatOnboardingAnim(Vector3 targetPos, System.Action onComplete)
        {
            Vector3 myPrevPos = goatAnim.position;
            goatAnim.gameObject.SetActive(true);
            goatAnim.DOMove(targetPos, 1f, true).onComplete += () =>
            {
                goatAnim.DOMove(myPrevPos, 0f).onComplete += () =>
                {
                    goatAnim.gameObject.SetActive(false);
                };
                onComplete?.Invoke();

            };
        }

        #region Public Methods

        public void Init()
        {
            gameState = GameState.Live;
            playerTurn = PlayerTurn.Tiger;
            GlobalEventHandler.TriggerEvent(EventID.EVENT_REQUEST_TO_CHANGE_PLAYER_TURN);
            noOfGoatsPlacedOnBoard = 0;
            noOfGoatsDied = 0;
            noOfTigersBlocked = 0;
            SovereignUtils.Log($"GameplayManager Init: {gameState} {playerTurn} {noOfGoatsPlacedOnBoard} {noOfGoatsDied} {noOfTigersBlocked}");
        }
        public static List<SpotPointBase> GetPointsAvailableToMoveList()
        {
            return spotPointsAvailableToMove;
        }
        public static byte GetGoatsOnTheBoard() => noOfGoatsPlacedOnBoard;
        public static PlayerTurn GetPlayerTurn() => playerTurn;
        public static void SwitchPlayerTurn()
        {
            GlobalEventHandler.TriggerEvent(EventID.EVENT_REQUEST_TO_KILL_TURN_TIMER_TWEENING);
            //if (playerTurn == PlayerTurn.Goat)
            //{
            //    playerTurn = PlayerTurn.Tiger;
            //    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_TIGER_TURN);
            //}
            //else
            //{
            //    playerTurn = PlayerTurn.Goat;
            //    GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_GOAT_TURN);
            //}
            playerTurn = playerTurn.Equals(PlayerTurn.Goat) ? PlayerTurn.Tiger : PlayerTurn.Goat;
            GlobalEventHandler.TriggerEvent(EventID.EVENT_ON_PLAYER_TURN_CHANGED, playerTurn);
            SovereignUtils.Log($"Player Switch: {playerTurn}");
        }
        public static bool AreGoatsOnboarded() => noOfGoatsPlacedOnBoard >= Constants.NUMBER_OF_GOATS_IN_THE_GAME;
        public static void IncrementPlacedGoatCount()
        {
            Mathf.Clamp(++noOfGoatsPlacedOnBoard, 0, Constants.NUMBER_OF_GOATS_IN_THE_GAME);
            SovereignUtils.Log($"{noOfGoatsPlacedOnBoard} goats placed on board");
        }
        /// <summary>
        /// This method updates the dead goat count. <br></br>If the minimum threshold is reached for the tigers win
        /// it will trigger the LEVEL_FINISH_EVENT.
        /// </summary>
        public static void UpdateDeadGoatCount()
        {
            noOfGoatsDied++;
            if (noOfGoatsDied >= Constants.MINIMUM_NUMBER_OF_GOATS_SHOULD_KILL_FOR_TIGERS_WIN)
                GlobalEventHandler.TriggerEvent(EventID.EVEN_ON_LEVEL_FINISHED, GameResult.TigerWon);
        }
        public static void SetGameState(GameState state)
        {
            gameState = state;
        }
        public static GameState GetCurrentGameState() => gameState;
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
            goatPoint = null;
            goatDeadPoint = null;
            SwitchPlayerTurn();
            isSpotPointClicked = false;
        }
        private void Callback_On_Spotpoints_Avail_To_Occupy(object args)
        {
            List<SpotPointBase> list = (List<SpotPointBase>)args;
            spotPointsAvailableToMove.Clear();
            spotPointsAvailableToMove.AddRange(list);
        }
        private void Callback_On_Player_Turn_Change_Requested(object args)
        {
            SwitchPlayerTurn();
            foreach (SpotPointBase item in spotPointsAvailableToMove)
            {
                item.HideCanOccupyGraphic();
            };
            goatPoint = null;
            goatDeadPoint = null;
            spotPointsAvailableToMove.Clear();
        }
        private void Callback_On_Level_Restart_Requested(object arg)
        {
            foreach (SpotPointBase item in spotPointsAvailableToMove)
            {
                item.HideCanOccupyGraphic();
            };
            goatDeadPoint = null;
            goatPoint = null;
            spotPointsAvailableToMove.Clear();
            Init();
        }

        private void Callback_On_Tiger_Unblocked(object args)
        {
            noOfTigersBlocked = (byte)Mathf.Clamp(--noOfTigersBlocked, 0, Constants.NUMBER_OF_TIGERS_IN_THE_GAME);
            SovereignUtils.Log($"*** NoOf Tigers un blocked: {noOfTigersBlocked} incre..");
        }

        private void Callback_On_Tiger_Blocked(object args)
        {

            noOfTigersBlocked = (byte)Mathf.Clamp(++noOfTigersBlocked, 0, Constants.NUMBER_OF_TIGERS_IN_THE_GAME);
            SovereignUtils.Log($"*** NoOf Tigers blocked: {noOfTigersBlocked} incre..");
            if (noOfTigersBlocked >= Constants.NUMBER_OF_TIGERS_IN_THE_GAME)
                GlobalEventHandler.TriggerEvent(EventID.EVEN_ON_LEVEL_FINISHED, GameResult.GoatWon);
        }


        #endregion Callbacks

    }
}
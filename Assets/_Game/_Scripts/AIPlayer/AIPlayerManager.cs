using SovereignStudios.Enums;
using SovereignStudios.EventSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Should handle the tiger and goat playing logic.
 * Should track of all the movements of the real player which means all the animals of the real player.
 * Should track of all it's animals (positions of the spotpoints)
 * Should handle the logic to make next move based on the difficulty level.
 * 
 
 */
public class AIPlayerManager : MonoBehaviour, IInitializer
{
    #region Variables

    public List<SpotPointBase> spotPoints;
    public List<SpotPointBase> mySpotpoints;
    public List<SpotPointBase> opponentSpotPoints;
    public PlayerTurn playingAs; //value will be either tiger or goat.
    //public Owner AiPlayer;
    public AIDifficultyLevel AiDifficultyLevel;
    #endregion Variables

    #region Unity Methods

    private void OnEnable()
    {
        GlobalEventHandler.AddListener(EventID.EVENT_ON_PLAYER_TURN_CHANGED, Callback_On_AI_Turn);
    }
    private void OnDisable()
    {
        GlobalEventHandler.RemoveListener(EventID.EVENT_ON_PLAYER_TURN_CHANGED, Callback_On_AI_Turn);
    }   
    #endregion Unity Methods

    #region Public Methods
    public void Init()
    {
        spotPoints = FindObjectsOfType<SpotPointBase>().ToList();

    }
    #endregion Public Methods

    #region Private Methods
    private void MakeCalculationsForMove()
    {
        switch (AiDifficultyLevel)
        {
            case AIDifficultyLevel.Easy:
                break;
            case AIDifficultyLevel.Medium:
                break;
            case AIDifficultyLevel.Hard:
                break;
            default:
                SpotPointBase mySpotPoint = default;
                List<SpotPointBase> tigerPoints = spotPoints.Where(x => x.ownerOfTheSpotPoint.Equals(Owner.Tiger)).ToList();
                List<SpotPointBase> goatPoints = spotPoints.Where(x => x.ownerOfTheSpotPoint.Equals(Owner.Goat)).ToList();
                foreach (SpotPointBase tigerPoint in tigerPoints)
                {
                    //tigerPoint.neighborsDictionary.
                }
                if (playingAs.Equals(PlayerTurn.Tiger))
                {
                }

                foreach (SpotPointBase spotPoint in spotPoints)
                {
                    spotPoint.ownerOfTheSpotPoint.Equals(playingAs);
                }
                break;
        }
    }
    private void ExecuteMove()
    {

    }



    #endregion Private Methods

    #region Callbacks
    private void Callback_On_AI_Turn(object args)
    {
        PlayerTurn currentTurn = (PlayerTurn)args;
        if (currentTurn == playingAs)
        {
            MakeCalculationsForMove();
            ExecuteMove();
        }
    }
    #endregion Callbacks
}
/*
1)  Define the AI's objectives: Determine what the AI's primary objectives are in the game. For example, the AI may want to protect its tigers, capture as many goats as possible, or strategically position its pieces.

2) Evaluate the game state: Assess the current state of the game, including the positions of the tigers and goats, the number of captured goats, and any potential threats or opportunities. This information will help the AI make informed decisions.

3) Implement a scoring system: Assign scores to different game states based on how favorable they are for the AI. For example, a high score can be given to game states where the AI has more tigers or goats, or where the goats are closer to being captured.

4) Implement a search algorithm: Use a search algorithm, such as Minimax or Alpha-Beta Pruning, to explore the possible moves and predict future game states. These algorithms help the AI consider the consequences of its decisions and choose the move that leads to the most advantageous outcome.

5) Consider heuristics: Introduce heuristics to guide the AI's decision-making process. Heuristics are rules or strategies that simplify complex problems. For example, you can prioritize moves that bring tigers closer to goats, or moves that create potential traps for goats.

6) Vary the AI's difficulty: Adjust the AI's difficulty level by modifying parameters such as search depth or the weightings of different heuristics. This allows you to create AI opponents with varying levels of skill.

7) Test and iterate: Implementing AI is an iterative process. Test your AI player against human players or other AI players to evaluate its performance. Make adjustments and fine-tune the decision-making logic based on the results.

8) Add randomness: To make the AI's behavior less predictable, you can introduce a level of randomness in its decision-making. For example, you can occasionally make the AI choose a suboptimal move to simulate a more human-like player.
 


AI Decision making as TIGER:
1) Initially, set the AI's objective to be capturing as many goats as possible and protecting its tigers.
2) Evaluate the game state and determine the current status of the tigers and goats.
3) If there are goats within reach of the tigers, prioritize capturing them.
4) If there are no immediate capture opportunities, focus on moving the tigers closer to the goats to create potential capturing chances.
5) Additionally, consider the safety of the tigers. If any of the tigers are under threat from the goats, prioritize moving them to safer positions.
6) Continuously evaluate the game state and adjust the AI's objective accordingly. For example, if the AI has captured a significant number of goats, it can shift its objective to focus more on protecting its tigers rather than capturing more goats.
 
 */
/// <summary>
/// All events to be used need to be added to this script.
/// </summary>
#region Event Types enum
public enum EventID
{

    //Level
    EVENT_ON_LEVEL_STARTED,
    EVEN_ON_LEVEL_FINISHED,
    EVENT_ON_LEVEL_EXITED,


    //Player
    EVENT_ON_PLAYER_STATE_CHANGED,
    EVENT_ON_PLAYER_DEAD,
    EVENT_ON_PLAYER_WIN,

    //Spot point
    EVENT_ON_SPOTPOINT_CLICKED
}
#endregion
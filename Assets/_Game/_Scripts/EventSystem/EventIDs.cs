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
    EVENT_ON_SPOTPOINT_CLICKED,
    EVENT_ON_SPOTPOINT_SELECTED,
    EVENT_ON_SPOTPOINT_SELECTION_ENDED,
    EVENT_ON_HIDE_CAN_OCCUPY_GRAPHIC,

    //Goat
    EVENT_ON_GOAT_ONBOARDING_REQUESTED,
    EVENT_ON_GOAT_TURN,
    EVENT_ON_GOAT_DEAD_POINT_DETECTED,

    //Tiger
    EVENT_ON_TIGER_TURN,

    EVENT_ON_SPOT_POINTS_AVAILABLE_TO_OCCUPY,
}
#endregion
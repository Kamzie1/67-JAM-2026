namespace YANG
{
    public enum ActionOnMap
    { 
        Move
    }

    public enum ActionOnPlayer 
    { 
        IncreaseRegen
    }

    public enum TurnPhase
    {
        NotStarted,
        PlayerAction,
        Resolving,
    }
}
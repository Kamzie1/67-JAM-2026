namespace YANG
{
    public enum ActionOnMap
    { 
        Move,
        Attack
    }

    public enum ActionOnPlayer 
    { 
        IncreaseRegen
    }

    public enum TurnPhase
    {
        StartTurn,
        EndTurn,
        NotStarted,
        PlayerAction,
        Resolving,
    }
}
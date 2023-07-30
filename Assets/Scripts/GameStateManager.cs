using System;

public static class GameStateManager
{
    public enum GameState 
    {
        None = 0,
        Paused = 1,
        GamePlay = 2,
        DeathScreen = 3,
    }

    public static GameState CurrentGameState { get; private set; }
    public static Action<GameState> GameStateChangedEvent;

    public static void SetState(GameState newState) 
    {
        if (newState == CurrentGameState) return;

        CurrentGameState = newState;
        GameStateChangedEvent?.Invoke(newState);
    }
}

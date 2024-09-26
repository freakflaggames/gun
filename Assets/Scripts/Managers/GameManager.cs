using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Game,
    LevelComplete,
    Dead
}

//This singleton class handles high-level gameplay states and loops
//It should be housed on the GameManager prefab object

public class GameManager : MonoBehaviour
{
    //Other classes reference the gamemanger static instance
    public static GameManager Instance;
    public GameState currentState;

    private void Awake()
    {
        //The gamemanager static instance is created in awake, so reference it in
        //Start() after it has been created
        Instance = this;
        //Set the game state to main game
        ChangeState(GameState.Game);
    }

    public void ChangeState(GameState state)
    {
        //When the state is changed, execute state-specific logic
        switch (state)
        {
            //When the main game state starts
            case GameState.Game:
                break;
            //When the level is completed
            case GameState.LevelComplete:
                break;
            //When the player died
            case GameState.Dead:
                break;
        }

        //Set the current state to the next state
        currentState = state;
    }
}

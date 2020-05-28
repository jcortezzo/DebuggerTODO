using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalValues : MonoBehaviour
{
    public static GlobalValues Instance;
    public bool mouseHide;
    public float playerHealth;
    public float money;
    public float highScore;
    public int primary;
    public int secondary;
    public int level;
    public double runStartTime;
    public int run;
    public int gameversion;
    public float healthCost;

    public GameState gameState;

    public enum GameState
    {
        MENU,
        GAMEPLAY,
        LEVELMENU
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            gameState = GameState.MENU;
        }
        else
        {
            Destroy(gameObject);
        }
        runStartTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
    }

    private void Update()
    {
        Cursor.visible = gameState != GameState.GAMEPLAY;
    }
}
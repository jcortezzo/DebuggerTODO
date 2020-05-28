using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Player player;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Instance.player = GameObject.Find("Player").GetComponent<Player>();
            Destroy(gameObject);
        }
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (player != null && player.health <= 0)
        {
            player.health = 15;
            player.weaponHolder.primary = null;
            player.weaponHolder.secondary = null;
            SceneManager.LoadScene("GameOverScene");
        }

        if (GlobalValues.Instance.gameState == GlobalValues.GameState.GAMEPLAY)
        {

        }

    }

}
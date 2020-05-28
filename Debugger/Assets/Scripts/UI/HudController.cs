using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class HudController : MonoBehaviour
{
    public LevelManager levelManager;
    public RectTransform PanelItemQ;
    public RectTransform PanelItemE;
    public RectTransform PanelLevel;
    public RectTransform PanelHealth;
    public RectTransform PanelScore;

    private TextMeshProUGUI healthText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI itemEText;
    private TextMeshProUGUI itemQText;
    private TextMeshProUGUI levelText;
    
    private Image itemQImage;
    private Image itemEImage;
    private Image healthBar;

    public RawImage minimap;
    public GameObject pauseMenuUI;
    public static bool paused = false;

    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        player = levelManager.player;
        Debug.Log(player);
        healthText = PanelHealth.GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreText = PanelScore.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemEText = PanelItemE.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemQText = PanelItemQ.GetChild(0).GetComponent<TextMeshProUGUI>();
        levelText = PanelLevel.GetChild(0).GetComponent<TextMeshProUGUI>();

        itemQImage = PanelItemQ.GetChild(1).GetComponent<Image>();
        itemEImage = PanelItemE.GetChild(1).GetComponent<Image>();
        healthBar = PanelHealth.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        //CheckPause();
    }

    public void UpdateUI()
    {
        /* 
        healthText.text = "" + Mathf.Round(player.health);
        if (player.health < 5 && player.health > 3)
        {
            healthText.color = Color.yellow;
        } else if(player.health < 3)
        {
            healthText.color = Color.red;
        } else
        {
            healthText.color = Color.black;
        }
        */
        scoreText.text = "" + Mathf.Round(GlobalValues.Instance.money);
        levelText.text = "" + GlobalValues.Instance.level;
        if (player.weaponHolder.secondary == null)
        {
            itemEImage.sprite = null;
            itemEText.text = "Item E";
        }
        else
        {
            itemEImage.sprite = player.weaponHolder.secondary.GetComponent<SpriteRenderer>().sprite;
            itemEText.text = player.weaponHolder.secondary.ToString();
        }
        if (player.weaponHolder.primary == null)
        {
            itemQImage.sprite = null;
            itemQText.text = "Item Q";

        }
        else
        {
            itemQImage.sprite = player.weaponHolder.primary.GetComponent<SpriteRenderer>().sprite;
            itemQText.text = player.weaponHolder.primary.ToString();
        }
        SetHealthBar();
    }

    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        paused = false;
        GlobalValues.Instance.mouseHide = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        paused = true;
        GlobalValues.Instance.mouseHide = true;
    }

    public void PauseExit()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void PauseResume()
    {
        Resume();
    }

    public void SetHealthBar()
    {
        int maxHealth = 15;
        float percentage = player.health / maxHealth;
        percentage = Mathf.Min(percentage, 1f);
        healthBar.transform.localScale = new Vector3(percentage, 1);
    }
}

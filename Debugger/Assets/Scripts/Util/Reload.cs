using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    Image img;
    Player player;
    public float heightScale;

    public void Start()
    {
        img = GetComponent<Image>();
        player = LevelManager.Instance.player;
    }

    public void Update()
    {
        float viewportHeight = Camera.main.pixelRect.height / heightScale;
        gameObject.transform.position = Camera.main.WorldToScreenPoint(player.transform.position) + new Vector3(0, viewportHeight, 0);
        if (player.HasWeaponEquipped() && player.weaponHolder.primary.cdTimer > 0)
        {
            Weapon primary = player.weaponHolder.primary;
            img.fillAmount = 1 - (primary.cdTimer / Mathf.Max(0.001f, primary.cd));
        }
        else
        {
            img.fillAmount = 0;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCrossHair : MonoBehaviour
{
    public GameObject crossHairPrefab;
    private GameObject crossHair;
    void Start()
    {
        crossHair = Instantiate(crossHairPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        crossHair.transform.position = mousePos;
    }
}
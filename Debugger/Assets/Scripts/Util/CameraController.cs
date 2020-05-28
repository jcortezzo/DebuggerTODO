using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject playerGo;
    public Vector2 mousePos;
    public float dampening;


    public bool shaking = false;
    float shakeMag, shakeTimeEnd;
    Vector3 shakeVector;

    private Player player;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        player = playerGo.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //mousePos = CaptureMousePos();
    }

    private void FixedUpdate()
    {
        mousePos = CaptureMousePos();
        Vector3 target = UpdateTargetPos();
        UpdateCameraPosition(target, new Vector3());
    }

    Vector3 CaptureMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition); //raw mouse pos
        ret *= 2;
        ret -= Vector2.one; //set (0,0) of mouse to middle of screen
        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized; //helps smooth near edges of screen
        }
        return ret;
    }
    Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * 2f;//3.5f; //mult mouse vector by distance scalar 
        Vector3 ret = player.transform.position + mouseOffset; //find position as it relates to the player
        ret += UpdateShake(); //add the screen shake vector to the target
        ret.z = -10;//zStart; //make sure camera stays at same Z coord
        return ret;
    }

    void UpdateCameraPosition(Vector3 target, Vector3 refVel)
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, dampening); //smoothly move towards the target
        rb.MovePosition(target);// * Time.fixedDeltaTime);
        //transform.position = tempPos; //update the position
    }

    // shaking

    public void Shake(Vector3 direction, float magnitude, float length)
    {
        shaking = true;
        shakeVector = direction;
        shakeMag = magnitude;
        shakeTimeEnd = Time.time + length;
    }

    Vector3 UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd)
        {
            shaking = false;
            return Vector3.zero;
        }
        Vector3 tempOffset = shakeVector;
        tempOffset *= shakeMag;
        return tempOffset;
    }


}
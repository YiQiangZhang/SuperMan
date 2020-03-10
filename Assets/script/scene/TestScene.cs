using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TestScene : MonoBehaviour
{
    public Joystick joystick;
    float speed = 600;
    // Use this for initialization
    void Start()
    {
        SpriteHelper.Init();
        joystick.sickPos = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(joystick.sickPos.x);
        Debug.Log(joystick.sickPos.y);
        GameObject.Find("Role").transform.Translate(joystick.sickPos * Time.deltaTime * 400);
        //SpriteHelper.Update(GameObject.Find("Role"), joystick.sickPos);
    }
}

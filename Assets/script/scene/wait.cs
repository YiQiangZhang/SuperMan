using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using Newtonsoft.Json;

public class wait : MonoBehaviour
{
    public Joystick joystick;


    // Use this for initialization
    void Start()
    {
        SpriteHelper.Init();
    }

    // Update is called once per frame public SpriteRenderer sr;
    void Update()
    {
        GameObject role = GameObject.Find("GameObject");
        SpriteHelper.Update(role, joystick.sickPos);
    }
}

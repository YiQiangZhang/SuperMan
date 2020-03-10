using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimeScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        var t = DateTime.Parse(gameObject.GetComponent<Text>().text);
        int timer = t.Hour * 3600 + t.Minute * 60 + t.Second;
        StartCoroutine("OpenTimer", timer);

    }
    IEnumerator OpenTimer(int timer)
    {
        gameObject.GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}:{2:D2}", timer / 3600, timer / 60 % 60, timer % 60);
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer++;
            gameObject.GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}:{2:D2}", timer / 3600, timer / 60 % 60, timer % 60);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : ScrollRect
{
    float radius = 0f;//半径
    public Vector3 sickPos;
    // Start is called before the first frame update

    void Start()
    {
        radius = viewport.rect.width / 2;   
    }

    public void back_to_begain()
    {
        Debug.Log(111);
        Vector3 vz = new Vector3(0, 0, 0);
        content.GetComponent<RectTransform>().anchoredPosition = vz;
    }

    // Update is called once per frame
    void Update()
    {
        if (content.localPosition.magnitude > radius)
            content.localPosition = content.localPosition.normalized * radius;

        sickPos = content.localPosition.normalized;
    }
}

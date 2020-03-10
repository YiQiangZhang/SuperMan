using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.UI;

public class SpriteHelper
{
    public static Dictionary<string, ArrayList> actionMap = new Dictionary<string, ArrayList>();

    public static void Init()
    {
        Debug.Log("Init");
        Dictionary<string, string[]> spriteMap = new Dictionary<string, string[]>()
        {
            { "wait", new string[] { "00001", "00002", "00003", "00004" } },
            { "run", new string[] { "00006", "00007", "00008", "00009" }}
        };
        Sprite[] sprites = Resources.LoadAll<Sprite>("player/9");
        var spiteNames = sprites.ToDictionary(r => r.name.Split(' ')[1], r => r);
        Debug.Log(JsonConvert.SerializeObject(spiteNames.Keys));
        foreach (var item in spriteMap)
        {
            Debug.Log(item.Key);

            if (!actionMap.ContainsKey(item.Key))
            {
                actionMap[item.Key] = new ArrayList();
            }

            foreach (var ele in item.Value)
            {
                Debug.Log(ele);
                if (spiteNames.ContainsKey(ele))
                {
                    Debug.Log(1);
                    actionMap[item.Key].Add(spiteNames[ele]);
                }
            }
        }
    }

    public static int index = 0;
    public static float lastTime = 0.05f;
    
    public static void Update(GameObject role,Vector3 sickPos)
    {
        lastTime -= Time.deltaTime;
        if (lastTime <= 0)
        {
            lastTime = 0.3f;
            index++;
        }
        Image image = role.GetComponentInChildren<Image>();
        if (sickPos.magnitude > 0)
        {
            image.sprite = actionMap["run"][index % 4] as Sprite;
            Debug.Log(image.transform.position);
            image.transform.SetPositionAndRotation(image.transform.position, new Quaternion(0, sickPos.x > 0 ? 0 : 180, 0, 0));
        }
        else
        {
            image.sprite = actionMap["wait"][index % 4] as Sprite;
        }
    }
}
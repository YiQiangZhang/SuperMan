using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class npcScript : MonoBehaviour
{
    Dictionary<string, ArrayList> actionMap = new Dictionary<string, ArrayList>();
    public int index = 0;
    public float lastTime = 0.05f;
    Value config;
    // Use this for initialization
    void Start()
    {
        if (config == null)
        {
            config = User.GetConfigByName(gameObject.name);
        }
        Dictionary<string, string[]> spriteMap = new Dictionary<string, string[]>()
        {
            { "wait", new string[] { "00001", "00002", "00003", "00004" } },
         //   { "run", new string[] { "00006", "00007", "00008", "00009" }}
        };
        Sprite[] sprites = Resources.LoadAll<Sprite>("player/"+ config.TryGet<string>("形象"));
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
                if (spiteNames.ContainsKey(ele))
                {
                    actionMap[item.Key].Add(spiteNames[ele]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (config != null)
        {
            lastTime -= Time.deltaTime;
            if (lastTime <= 0)
            {
                lastTime = 0.3f;
                index++;
            }
            Image image = gameObject.GetComponentInChildren<Image>();
            image.sprite = actionMap["wait"][index % 4] as Sprite;
        }

    }
  

}

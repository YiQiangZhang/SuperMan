using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectItem
{
    public int type { get; set; }

    public int x { get; set; }

    public int y { get; set; }

    public int width { get; set; }

    public int height { get; set; }

    public string prefab { get; set; }
    public string text { get; set; }

    public string color { get; set; }
    public string name { get; set; }

    public int fontSize { get; set; }

    public string sprite { get; set; }
}

public class GameObjectI : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject loadPrefab(string path)
    {
        GameObject go = (GameObject)Resources.Load(path);
        if (go)
        {
            return GameObject.Instantiate(go) as GameObject;
        }
        return null;
    }

    void Start()
    {
        //构建3个按钮
        var buttons = new List<GameObjectItem>()
        {
            new GameObjectItem()
            {
                type = 1,
                x = -100,
                y = -50,
                prefab ="Prefabs/messageButton",
                text ="取消",
                width = 60,
                height = 30,
            },
            new GameObjectItem()
            {
                type = 1,
                x = 100,
                y = -50,
                prefab ="Prefabs/messageButton",
                text ="确定",
                width = 60,
                height = 30,
            },
             new GameObjectItem()
            {
                type = 1,
                x = 180,
                y = 80,
                prefab ="Prefabs/messageButton",
                text ="X",
                width = 30,
                height = 30,
            },
             new GameObjectItem()
             {
                type = 2,
                x = 0,
                y = 0,
                prefab ="Prefabs/messageText",
                text ="花费30元宝购买",
             }  
        };

        GameObject message = GameObject.Find("Canvas/Message/MessageBox");
        foreach(var item in buttons)
        {
            GameObject go = loadPrefab(item.prefab);
            go.name = item.text;
            go.GetComponentInChildren<Text>().text = item.text;
            go.transform.position = new Vector3(item.x, item.y);


            if(item.width > 0)
            {
                go.GetComponent<RectTransform>().sizeDelta = new Vector2(item.width, item.height);
            }
            go.transform.SetParent(message.transform, false);
        }
    }



    T Find<T>(string path)
    {
        //获取按钮游戏对象
        GameObject obj = GameObject.Find(path);
        //获取按钮脚本组件
        var comm = (T)obj.GetComponent<T>();
        Debug.Log(comm);
        return comm;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("1");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(string.Format("{0},{1}", Input.mousePosition.x, Input.mousePosition.y));
            //如果点击到pannel之外，则取消
            
        }
    }
}

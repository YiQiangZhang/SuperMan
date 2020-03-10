using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;

public class Game
{
    public static void ChangeText(string path, string text, string color = "", int fontSize = 0)
    {
        GameObject obj = GameObject.Find(path);
        obj.GetComponent<Text>().text = text;
        if (!string.IsNullOrEmpty(color))
        {
            Color fontColor = new Color(255, 0, 0);
            if (color == "red")
            {
                fontColor = new Color(255, 0, 0);
            }
            obj.GetComponentInChildren<Text>().color = fontColor;
        }

        if (fontSize != 0)
        {
            obj.GetComponentInChildren<Text>().fontSize = fontSize;
        }
    }

    public static void ChangeImage(string path,string imagePath)
    {
        var sprite = Resources.Load(imagePath, typeof(Sprite)) as Sprite;
        GameObject.Find(path).GetComponent<Image>().sprite = sprite;
    }

    public static IEnumerator yieldAll(Action<CallBack>[] action)
    {
        int i = 0;
        bool next = true;
        Debug.Log("Enter");
        CallBack cb = new CallBack();
        while (i < action.Length)
        {
            if (next)
            {
                Debug.Log(" yield ALL" + i);
                next = false;
                cb.next = () =>
                {
                    i++;
                    next = true;

                };
                action[i](cb);
            }

            yield return null;
        }
        Debug.Log("结束循环");
    }

    public static void ShowMessage(string text,int messageType =0, UnityAction<int> clickAction = null)
    {
        GameObject message = GameObject.Find("Canvas");
        var go = GameObject.Find("Message");
        if (go == null)
        {
            go = loadPrefab("Prefabs/Message");
            go.name = "Message";
            var messageText = Find<Text>("MessageText");
            messageText.text = text;
            messageText.color = new Color(255, 0, 0);
            go.transform.SetParent(message.transform, false);


            var messageConfirm = Find<Button>("MessageConfirm");
            messageConfirm.onClick.AddListener(() =>
            {
                GameObject m = GameObject.Find("Message");
                GameObject.Destroy(m);
                if (clickAction != null)
                    clickAction(1);
            });
            var MessageCancel = Find<Button>("MessageCancel");
            if (messageType == 0)
            {
                MessageCancel.onClick.AddListener(() =>
                {
                    GameObject m = GameObject.Find("Message");
                    GameObject.Destroy(m);
                    if (clickAction != null)
                        clickAction(0);
                });
            }
            else
            {
                MessageCancel.gameObject.SetActive(false);
            }
            var MessageX = Find<Button>("X");
            MessageX.onClick.AddListener(() =>
            {
                GameObject m = GameObject.Find("Message");
                GameObject.Destroy(m);
                if (clickAction != null)
                    clickAction(0);
            });
        }
    }

    public static GameObject CreatePrefab(GameObject parent, GameObjectItem item)
    {

        GameObject go = loadPrefab(item.prefab);
        Debug.Log(item.name);
        go.name = item.name;
        if (!string.IsNullOrEmpty(item.sprite))
        {
            var sprite = Resources.Load(item.sprite, typeof(Sprite)) as Sprite;
            go.GetComponent<Image>().sprite = sprite;
        }

        if (!string.IsNullOrEmpty(item.text))
        {
            go.GetComponentInChildren<Text>().text = item.text;
        }
        if (item.fontSize != 0)
        {
            go.GetComponentInChildren<Text>().fontSize = item.fontSize;
        }
        if (!string.IsNullOrEmpty(item.color))
        {
            Color fontColor = new Color(255, 0, 0);
            if (item.color == "red")
            {
                fontColor = new Color(255, 0, 0);
            }
            go.GetComponentInChildren<Text>().color = fontColor;
        }
        if (!(item.x == 0 && item.y == 0))
        {
            go.transform.position = new Vector3(item.x, item.y);
        }

        if (item.width > 0)
        {
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(item.width, item.height);
        }
        go.transform.SetParent(parent.transform, false);
        return go;
    }

    public static T Find<T>(string path)
    {
        //获取按钮游戏对象
        GameObject obj = GameObject.Find(path);
        //获取按钮脚本组件
        var comm = obj.GetComponent<T>();
        Debug.Log(comm);
        return comm;
    }


    public static GameObject loadPrefab(string path)
    {
        GameObject go = (GameObject)Resources.Load(path);
        if (go)
        {
            return GameObject.Instantiate(go) as GameObject;
        }
        return null;
    }

    public static IEnumerator Post<T>(string route, object data, Action<T> action, Action<int> errAction = null)
    {
        string url = "http://118.190.101.131:3001" + route;
        string json = JsonConvert.SerializeObject(data);
        Debug.Log(url + json);
        byte[] body = Encoding.UTF8.GetBytes(json);
        UnityWebRequest unityWeb = new UnityWebRequest(url, "POST");
        unityWeb.uploadHandler = new UploadHandlerRaw(body);
        unityWeb.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        unityWeb.downloadHandler = new DownloadHandlerBuffer();
        yield return unityWeb.SendWebRequest();
        //异常处理，很多博文用了error!=null这是错误的，请看下文其他属性部分
        if (unityWeb.isHttpError || unityWeb.isNetworkError)
        {
            Debug.Log("errrrrrrrrrrrrrrrr");
            Debug.Log(unityWeb.error);
        }
        else
        {
            Debug.Log(unityWeb.downloadHandler.text);
            Debug.Log(111111111111111111);

            var result = JsonConvert.DeserializeObject<GameResponse<T>>(unityWeb.downloadHandler.text);
            Debug.Log(result.data);
            if (result.err == 1)
                errAction(result.err);
            else
                action(result.data);
        }
    }
}


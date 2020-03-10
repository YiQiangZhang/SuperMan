using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class Chat
{
    public  const float REFRESH_TIME = 1;
    public static IEnumerator Init(GameObject gameObject, UnityAction sendAction)
    {
        //创建聊天窗口
        var chatGo = GameObject.Find("聊天窗口");
        if (chatGo == null)
        {
            refreshTime = REFRESH_TIME;
            chatGo = Game.CreatePrefab(gameObject, new GameObjectItem()
            {
                type = 1,
                prefab = "Prefabs/Chat",
                name = "聊天窗口",
                width = 400,
                height = 400,
                x = -380,
                y = -150
            });

            Game.Find<Button>("ChatCancel").onClick.AddListener(() =>
            {
                Debug.Log("关闭");
                GameObject m = GameObject.Find("聊天窗口");
                GameObject.Destroy(m);
            });

            Game.Find<Button>("ChatSend").onClick.AddListener(sendAction);

            //请求聊天服务
            yield return GetChat();
        }
    }

    public static ChatResponse all = new ChatResponse()
    {
        result = new List<ChatItem>()
    };

    public static float refreshTime = REFRESH_TIME;

    public static IEnumerator GetChat()
    {
        if(refreshTime != -1) 
            refreshTime += Time.deltaTime;
        var chatGo = GameObject.Find("聊天窗口");
        if (chatGo != null)
        {
            if (refreshTime >= REFRESH_TIME)
            {
                refreshTime = -1;
                yield return User.Post("/Chat/getChat", new
                {
                    toId = "world",
                    lastTime = all.lastTime
                }, (ChatResponse res) =>
                {
                    if(res.lastTime > all.lastTime)
                    {
                        all.lastTime = res.lastTime;
                    }
                    Debug.Log(res.result);
                    string path = "Canvas/聊天窗口/ScrollRect/ScrollView/Content";
                    GameObject content = GameObject.Find(path);
                    //创建预制体
                    if (res.result != null && res.result.Count > 0)
                    {
                        for (int i = 0; i < res.result.Count; i++)
                        {
                            var chatItem = Game.CreatePrefab(content, new GameObjectItem()
                            {
                                name = (all.result.Count).ToString(),
                                type = 1,
                                prefab = "Prefabs/ChatItem",
                            });
                            all.result.Add(res.result[i]);
                            Debug.Log(chatItem);
                            Game.Find<Text>(string.Format("{0}/{1}/{2}", path, chatItem.name, "Text")).text = res.result[i].content;
                            Game.Find<Text>(string.Format("{0}/{1}/{2}", path, chatItem.name, "Name")).text = res.result[i].name;
                        }
                        var rect = Game.Find<ScrollRect>("Canvas/聊天窗口/ScrollRect");
                        DOTween.To(() => rect.verticalNormalizedPosition = 0f, v => rect.verticalNormalizedPosition = v, 0, 0.2f).SetEase(Ease.InElastic);
                    }
                    refreshTime = 0;
                });
            }
        }
    }
}


public class ChatResponse
{
    public long lastTime { get; set; }
    public List<ChatItem> result { get; set; }

}

public class ChatItem
{
    public string name { get; set; }
    public string motto { get; set; }
    public string pic { get; set; }
    public string id { get; set; }
    public string fromId { get; set; }
    public string toId { get; set; }
    public string content { get; set; }
    public long createOn { get; set; }
}
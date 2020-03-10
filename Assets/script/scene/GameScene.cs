using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameObject go = Game.CreatePrefab(gameObject, new GameObjectItem
        {
            name = "伙伴按钮",
            text = "伙伴",
            width = 100,
            height = 60,
            prefab = "Prefabs/Button",
            type = 1,
            x = 600,
            y = -350
        });
        var button = go.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            HeroHelper.ShowHeroSence(gameObject);
        });

        GameObject bag = Game.CreatePrefab(gameObject, new GameObjectItem
        {
            name = "背包按钮",
            text = "背包",
            width = 100,
            height = 60,
            prefab = "Prefabs/Button",
            type = 1,
            x = 500,
            y = -350
        });

        var bagButton = bag.GetComponent<Button>();
        bagButton.onClick.AddListener(() =>
        {
            Bag.ShowBag();
        });

        GameObject chat = Game.CreatePrefab(gameObject, new GameObjectItem
        {
            name = "聊天按钮",
            text = "聊天",
            width = 100,
            height = 60,
            prefab = "Prefabs/Button",
            type = 1,
            x = -600,
            y = -350
        });
        chat.GetComponent<Button>().onClick.AddListener(() =>
        {
            StartCoroutine(Chat.Init(gameObject, () =>
            {
                InputField text = Game.Find<InputField>("ChatText");

                if (!string.IsNullOrEmpty(text.text))
                {
                    Debug.Log("send");
                    StartCoroutine(User.Post("/Chat/send", new
                        {
                            toId = "world",
                            content = text.text
                        }, (ChatResponse res) =>
                            {
                                Debug.Log("send1");
                                text.text = "";
                                Chat.refreshTime = Chat.REFRESH_TIME;
                            })
                    );
                }
            }));
        });
    }

    // Update is called once per frame

    void Update()
    {
        StartCoroutine(Chat.GetChat());
    }
}

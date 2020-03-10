using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapScene : MonoBehaviour
{
    float speed = 600;
    public Joystick joystick;
    MapManager mapManager;
    // Use this for initialization
    void Start()
    {
        GameObject parent = GameObject.Find("UI");

        SpriteHelper.Init();
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1f/60;
        User.configs = User.HttpSend<List<Value>>("/config/enum", new { });
        var userResource = new List<string>();
        userResource.Add("金币");
        userResource.Add("绑定钻石");
        userResource.Add("钻石");
        foreach (var item in userResource)
        {
            Game.ChangeText(item + "/Count", User.GetCountByName(item).ToString());
        }
        mapManager = new MapManager(GameObject.Find("Map"),"主场景");
        GameObject go = Game.CreatePrefab(parent, new GameObjectItem
        {
            name = "伙伴按钮",
            text = "伙伴",
            width = 116,
            height = 119,
            prefab = "Prefabs/Button",
            type = 1,
            x = 600,
            y = -300,
            sprite ="mainScene/14伙伴"
        });
        var button = go.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            HeroHelper.ShowHeroSence(parent);
        });

        GameObject bag = Game.CreatePrefab(parent, new GameObjectItem
        {
            name = "背包按钮",
            text = "背包",
            width = 116,
            height = 119,
            prefab = "Prefabs/Button",
            type = 1,
            x = 500,
            y = -300,
            sprite = "mainScene/13背包"
        });

        var bagButton = bag.GetComponent<Button>();
        bagButton.onClick.AddListener(() =>
        {
            Bag.ShowBag();
        });

        GameObject chat = Game.CreatePrefab(parent, new GameObjectItem
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
            StartCoroutine(Chat.Init(parent, () =>
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
        GameObject role =User.GetUser();
        GameObject bg = mapManager.GetBg();
        var bgRect = (bg.transform as RectTransform);
        var roleRect = (role.transform as RectTransform);
        Game.ChangeText("x", ((int)(roleRect.localPosition.x + bgRect.localPosition.x)).ToString());
        Game.ChangeText("y", ((int)(roleRect.localPosition.y + bgRect.localPosition.y)).ToString());

        Vector3 bgMove = new Vector3(0, 0, 0);
        Vector3 roleMove = new Vector3(0, 0, 0);
        SpriteHelper.Update(role, joystick.sickPos);
        var mStepX = joystick.sickPos.x * speed * Time.deltaTime;
        var mStepY = joystick.sickPos.y * speed * Time.deltaTime;
        var xLimit = (bgRect.rect.width - 1696) / 2;
        var yLimit = (bgRect.rect.height - 848) / 2;
        var xRoleLimit = (1696 - 100) / 2;
        var yRoleLimit = (848 - 100) / 2;
        if (mStepX > 0)
        {
            Debug.Log("右移动");
        }

        if (mStepX < 0)
        {
            Debug.Log("左移动");
        }
        if (-xLimit < bg.transform.localPosition.x - mStepX && bg.transform.localPosition.x - mStepX < xLimit)
        {
            //只要背景还有移动的空间，就可以走
            if(mStepX > 0)
            {
                //玩家的位置没有大于0，就要继续走
                if(role.transform.localPosition.x < 0)
                {
                    roleMove.x += mStepX;
                }
                else
                {
                    bgMove.x -= mStepX;
                }
            }
            else
            {
                //玩家的位置没有大于0，就要继续走
                if (role.transform.localPosition.x > 0)
                {
                    roleMove.x += mStepX;
                }
                else
                {
                    bgMove.x -= mStepX;
                }
            }
        }
        else
        {
            if (-xRoleLimit < role.transform.localPosition.x + mStepX && role.transform.localPosition.x + mStepX < xRoleLimit)
            {
                roleMove.x += mStepX;
            }
        }

        if (-yLimit < bg.transform.localPosition.y - mStepY && bg.transform.localPosition.y - mStepY < yLimit)
        {
            if (mStepY > 0)
            {
                //玩家的位置没有大于0，就要继续走
                if (role.transform.localPosition.y < 0)
                {
                    roleMove.y += mStepY;
                }
                else
                {
                    bgMove.y -= mStepY;
                }
            }
            else
            {
                //玩家的位置没有大于0，就要继续走
                if (role.transform.localPosition.y > 0)
                {
                    roleMove.y += mStepY;
                }
                else
                {
                    bgMove.y -= mStepY;
                }
            }
        }
        else
        {
            if (-yRoleLimit < role.transform.localPosition.y + mStepY && role.transform.localPosition.y + mStepY < yRoleLimit)
            {
                roleMove.y += mStepY;
            }
        }
        bg.transform.Translate(bgMove);
        role.transform.Translate(roleMove);
    }

    void FixedUpdate()
    {
        WorldSync.Record(MapManager.curMapManager.CurRoom);
    }
}

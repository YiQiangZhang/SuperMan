using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class FIghter
{
    public FIghter()
    {

    }
    public GameObject go;

    public int heroNo;

    public List<Value> values;

    public string Name;

    public int isAttack;

    public int position;

    public string Id;

}

public class FightScene
{
    public static void Create(GameObject parent)
    {
        parent = GameObject.Find("Top");
        var bg = Game.CreatePrefab(parent, new GameObjectItem
        {
            name = "Fight",
            width = 1334,
            height = 750,
            prefab = "Prefabs/bg",
            sprite = "bg/8战斗背景",
        });

        GameObject back = Game.CreatePrefab(bg, new GameObjectItem
        {
            name = "返回按钮",
            width = 54,
            height = 54,
            prefab = "Prefabs/Button",
            type = 1,
            x = -600,
            y = 350,
            fontSize = 30,
            sprite = "0返回按钮",
        });

        back.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject m = GameObject.Find("Fight");
            GameObject.Destroy(m);
        });

        var npcs = new List<string>()
        {
            "player/8/player8 00001",
            "player/9/player9 00001",
            "player/11/player11 00001",
            "player/15/player15 00001",
            "player/8/player8 00001",
            "player/9/player9 00001"
        };

        for (int i = 0; i < Battle.battleList.Count; i++)
        {
            var goItem = new GameObjectItem()
            {
                name = i.ToString(),
                x = Battle.battleList[i].x,
                y = Battle.battleList[i].y + 50,
                width = 250,
                height = 200,
                prefab = "Prefabs/fight/NPC",
                sprite = npcs[i]
            };
            Game.CreatePrefab(bg, goItem);
        }

        for (int i = 0; i < 5; i++)
        {
            var goItem = new GameObjectItem()
            {
                name = "skill" + i.ToString(),
                x = -580 + 120 * i,
                y = -300,
                width = 113,
                height = 113,
                prefab = "Prefabs/fight/GO",
                sprite = "hero/group/2特技框"
            };
            Game.CreatePrefab(bg, goItem);
        }

        //创建BOSS
        var boss = new GameObjectItem()
        {
            name = "boss",
            x = 400,
            y = 0,
            width = 500,
            height = 500,
            prefab = "Prefabs/fight/GO",
            sprite = "player/16/enemy_0003 00001"
        };
        Game.CreatePrefab(bg, boss);

        var bossBlood = new GameObjectItem()
        {
            name = "bossBlood",
            x = 350,
            y = 330,
            prefab = "Prefabs/fight/BossBlood",
        };
        Game.CreatePrefab(bg, bossBlood);
        var bossItems = new List<string>()
        {
            "手",
            "棍棒",
            "腿",
        };
        for (int i = 0; i < bossItems.Count; i++)
        {
            var bossItem = new GameObjectItem()
            {
                name = bossItems[i],
                x = 570,
                y = 280 - i * 40,
                prefab = "Prefabs/fight/BossItem",
            };
            Game.CreatePrefab(bg, bossItem);
            Game.ChangeText("Fight/" + bossItems[i] + "/Text", bossItems[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            var goItem = new GameObjectItem()
            {
                name = "npc" + i.ToString(),
                x = -Battle.battleList[i].x,
                y = -Battle.battleList[i].y - 100,
                width = 250,
                height = 200,
                prefab = "Prefabs/fight/NPC",
                sprite = npcs[i]
            };
            var npc = Game.CreatePrefab(bg, goItem);
            npc.GetComponent<RectTransform>().SetPositionAndRotation(npc.transform.position, new Quaternion(0, 180, 0, 0));
        }
        Sequence seq = DOTween.Sequence();
        List<Tween> ts = new List<Tween>();
        Attack(seq, "0", "npc1");
        Attack(seq, "1", "npc2");
    }


    // Use this for initialization

    public static void Attack(Sequence seq, string formStr, string toStr)
    {
        var form = GameObject.Find(formStr);
        var to = GameObject.Find(toStr);
        var start = form.transform.position;
        var attackTo = new Vector3(to.transform.position.x - 100, to.transform.position.y);
        seq.Append(form.transform.DOMove(attackTo, 2f, true));
        seq.Append(to.transform.DOShakePosition(1, 5, 10, 50, true));
        seq.Append(form.transform.DOMove(start, 2f, true));
    }


    // Update is called once per frame

}

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Hero
{
    public string id { get; set; }

    public int heroNo { get; set; }

    public Dictionary<int, long> values = new Dictionary<int, long>();

    public string GetName()
    {
        var c = User.GetConfig(heroNo);
        return c.name;
    }
    public long GetValueByName(string name)
    {
        var c = User.GetConfigByName(name);
        if (c == null)
            return 0;
        return this.values.ContainsKey(c.valueNo) ? this.values[c.valueNo] : 0;
    }

    public T GetAttrByName<T>(string name)
    {
        var c = User.GetConfig(heroNo);
        return c.TryGet<T>(name);
    }

    public void SetValueByName(string name, long value)
    {
        var c = User.GetConfigByName(name);
        this.values[c.valueNo] = value;
    }
}

public class HeroLvupRes
{
    public List<Hero> list;
}


public class HeroHelper
{
    public static Dictionary<string, string> dic = new Dictionary<string, string>()
    {
        {"Power","战斗力" },
        {"Lv","等级" },
        {"Attack","攻击" },
        {"Defend","防御" },
        {"HP","血量" },
        {"Physique","体质" },
        {"Brain","智力" },
        {"Agile","敏捷" },
        {"Physical","力量" },
    };

    public static void Refresh(Hero hero)
    {

        Game.ChangeImage("Race", "hero/" + hero.GetAttrByName<string>("种族"));
        Game.ChangeImage("Profession", "hero/desc/" + hero.GetAttrByName<string>("职业"));
        Game.ChangeImage("VerticalDrawing", "VerticalDrawing/" + (hero.heroNo % 6 + 1));
        Game.ChangeText("Left/Name", hero.GetName());

        //设置 种族，职业，英雄图片，名字，品质，技能，普通技能
        foreach (var item in dic)
        {
            var v = hero.GetValueByName(item.Value);
            Debug.Log(item.Key);
            Debug.Log(v);
            Game.ChangeText(item.Key + "/Count", v.ToString());
        };
    }
    public static void ShowHeroDescPanel(GameObject gameObject, Hero hero)
    {
        var HeroDesc = GameObject.Find("HeroDesc");
        if (HeroDesc == null)
        {
            HeroDesc = Game.CreatePrefab(gameObject, new GameObjectItem
            {
                name = "英雄详情",
                prefab = "Prefabs/HeroDesc",
            });
            //

            Refresh(hero);


            GameObject.Find("LvUp").GetComponent<Button>().onClick.AddListener(() =>
            {
                var nameGo = GameObject.Find("Left/Name");

                //调用升级接口
                var res = User.HttpSend<HeroLvupRes>("/hero/lvUp", new
                {
                    heroId = hero.id,
                });
                User.heros = res.list;

                Refresh(User.heros.Find(r=>r.id == hero.id));


            });
            GameObject.Find("HeroDescBack").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject.Destroy(HeroDesc);
            });
        }
    }

    public static void ShowHeroSence(GameObject gameObject)
    {
        var heroPanel = GameObject.Find("HeroPanel");
        if (heroPanel == null)
        {
            heroPanel = Game.CreatePrefab(gameObject, new GameObjectItem
            {
                name = "HeroPanel",
                prefab = "Prefabs/HeroPanel",
            });
            heroPanel.GetComponentInChildren<Image>().color = new Color(147, 175, 191);
            //返回
            GameObject back = Game.CreatePrefab(heroPanel, new GameObjectItem
            {
                name = "返回按钮",
                width = 60,
                height = 60,
                prefab = "Prefabs/Button",
                type = 1,
                x = -620,
                y = 340,
                fontSize = 20,
                sprite = "0返回按钮"
            });
            back.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject m = GameObject.Find("HeroPanel");
                GameObject.Destroy(m);
            });
            GameObject battle = Game.CreatePrefab(heroPanel, new GameObjectItem
            {
                name = "阵容按钮",
                width = 161,
                height = 166,
                prefab = "Prefabs/Button",
                type = 1,
                x = 580,
                y = -300,
                fontSize = 40,
                sprite = "hero/16队伍按钮",
            });
            battle.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("show");
                Battle.ShowBattlePanel(gameObject);
            });
            //创建几个按钮
            GameObject go = Game.CreatePrefab(heroPanel, new GameObjectItem
            {
                name = "全部按钮",
                text = "全部",
                width = 80,
                height = 80,
                prefab = "Prefabs/Button",
                type = 1,
                x = -620,
                y = 234,
                fontSize = 30,
                sprite = "hero/8全部按钮底框"
            });
            var button = go.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {

            });
            var arr = new string[] { "超能者", "夜精灵", "光精灵", "科学工匠", "泰尔达人" };
            for (int i = 0; i < arr.Length; i++)
            {
                GameObject item = Game.CreatePrefab(heroPanel, new GameObjectItem
                {
                    name = arr[i],
                    width = 80,
                    height = 80,
                    prefab = "Prefabs/Button",
                    type = 1,
                    x = -615,
                    y = 150 - i * 100,
                    fontSize = 30,
                    sprite = "hero/" + (i + 10)

                });
                item.GetComponent<Button>().onClick.AddListener(() =>
                {

                });
            }
            User.heros = User.HttpSend<List<Hero>>("/hero/enter", new
            {
            });
            var heroListPanel = GameObject.Find("HeroListPanel");
            for (var i = 0; i < User.heros.Count; i++)
            {
                GameObject item = Game.CreatePrefab(heroListPanel, new GameObjectItem
                {
                    name = User.heros[i].id,
                    text = User.heros[i].GetName().ToString(),
                    prefab = "Prefabs/HeroItem",
                    type = 1,
                    fontSize = 30
                });
                Game.ChangeText(string.Format("HeroPanel/HeroList/HeroListPanel/{0}/Lv", User.heros[i].id), User.heros[i].GetValueByName("等级") + "级");

                item.GetComponent<Button>().onClick.AddListener(() =>
                {
                    var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                    var hero = User.heros.Find(r => r.id == buttonSelf.name);
                    ShowHeroDescPanel(gameObject, hero);
                    //弹出 英雄培养界面
                });
            }
        }
    }

    public static void AddChooseListener(GameObject gameObject, GameObject btnGo, Action<GameObject, GameObject> action)
    {
        btnGo.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("AddChooseListener");
            var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            GameObject heroList = GameObject.Find("选择列表");
            if (heroList != null)
            {
                return;
            }
            Debug.Log("AddChooseListener1");

            //调用弹窗组件，列表信息由外部传入
            heroList = Game.CreatePrefab(gameObject, new GameObjectItem
            {
                name = "选择列表",
                prefab = "Prefabs/ChooseList",
            });
            var content = GameObject.Find("BoxView/BoxContent");
            foreach (var h in User.heros)
            {
                var heroItem = Game.CreatePrefab(content, new GameObjectItem
                {
                    name = h.id,
                    prefab = "Prefabs/HeroItem",

                });
                heroItem.GetComponent<Button>().onClick.AddListener(() =>
                {
                    var chooseHero = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                    action(buttonSelf, chooseHero);
                    GameObject m = GameObject.Find("选择列表");
                    GameObject.Destroy(m);
                });
                var hasFight = h.GetValueByName("出战");
                Game.ChangeText(string.Format("BoxContent/{0}/Text", h.id), User.GetConfig(h.heroNo).name, hasFight > 0 ? "red" : "");
                Game.ChangeText(string.Format("BoxContent/{0}/Lv", h.id), h.GetValueByName("等级").ToString());
            }

            Game.Find<Button>("ChooseCancel").onClick.AddListener(() =>
            {
                Debug.Log("关闭");
                GameObject m = GameObject.Find("选择列表");
                GameObject.Destroy(m);
            });
        });

    }


}

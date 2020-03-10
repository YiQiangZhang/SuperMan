using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class Battle
{
    public static List<GameObjectItem> battleList = new List<GameObjectItem>()
                    {
                        new GameObjectItem(){x = -450 ,y =180},
                        new GameObjectItem(){x = -300 ,y =120},
                        new GameObjectItem(){x = -150 ,y =60},
                        new GameObjectItem(){x = -450 ,y =-220},
                        new GameObjectItem(){x = -300 ,y =-160},
                        new GameObjectItem(){x = -150 ,y =-100},
                    };
    public static void ChooseClick()
    {
        var ids = Enumerable.Range(1, 6).Select(i =>
        {
            var has = User.heros.Find(r =>
            {
                return r.GetValueByName("出战") == i;
            });
            return has == null ? "" : has.id;
        });

        User.heros = User.HttpSend<List<Hero>>("/hero/setBattle", new { ids = ids });
    }

    public static void ShowBattlePanel(GameObject gameObject)
    {

        var battlePanel = GameObject.Find("BattlePanel");
        if (battlePanel == null)
        {
            battlePanel = Game.CreatePrefab(gameObject, new GameObjectItem
            {
                name = "BattlePanel",
                prefab = "Prefabs/Panel",
                sprite = "hero/group/0界面底板"
            });
            battlePanel.GetComponentInChildren<Image>().color = new Color(147, 175, 191);
            GameObject back = Game.CreatePrefab(battlePanel, new GameObjectItem
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
                GameObject m = GameObject.Find("BattlePanel");
                GameObject.Destroy(m);
            });

            User.configs = User.HttpSend<List<Value>>("/config/enum", new { });
            User.heros = User.HttpSend<List<Hero>>("/hero/enter", new { });
            Debug.Log(User.heros.Count);



            GameObject battleGo = Game.CreatePrefab(battlePanel, new GameObjectItem
            {
                name = "阵容保存按钮",
                text = "保存",
                width = 80,
                height = 60,
                prefab = "Prefabs/Button",
                type = 1,
                x = -600,
                y = 220,
                fontSize = 30
            });
            battleGo.GetComponent<Button>().onClick.AddListener(ChooseClick);

            var bList = new List<GameObjectItem>();
            for (int i = 0; i < battleList.Count; i++)
            {
                var position = new GameObjectItem()
                {
                    x = battleList[i].x,
                    y = battleList[i].y
                };
                position.name = i.ToString();
                var has = User.heros.Find(r =>
                    {
                        return r.GetValueByName("出战") == i + 1;
                    }
                );
                position.text = has == null ? "+" : User.GetConfig(has.heroNo).name;
                position.width = 133; ;
                position.height = 153;
                position.prefab = "Prefabs/Button";
                position.sprite = "hero/group/1阵容按钮";
                position.fontSize = has == null ? 80 : 30;
                position.color = has == null ? "" : "red";
                GameObject item = Game.CreatePrefab(battlePanel, position);
                HeroHelper.AddChooseListener(gameObject, item, (GameObject start, GameObject end) =>
                 {
                     Debug.Log("choose");
                     //先检查
                     var endHero = User.heros.Find(r => r.id == end.name);
                     var goAttack = endHero.GetValueByName("出战");
                     if (goAttack > 0)
                     {
                         Game.ChangeText("BattlePanel/" + (goAttack - 1).ToString() + "/Text", "+", "", 80);
                         endHero.SetValueByName("出战", 0);
                     }
                     var startHero = User.heros.Find(r => r.GetValueByName("出战") == long.Parse(start.name) + 1);
                     if (startHero != null)
                     {
                         var startHeroPosition = startHero.GetValueByName("出战");
                         Game.ChangeText("BattlePanel/" + (startHeroPosition - 1).ToString() + "/Text", "+", "", 80);
                         startHero.SetValueByName("出战", 0);
                     }

                     //更换
                     endHero.SetValueByName("出战", long.Parse(start.name) + 1);
                     Game.ChangeText("BattlePanel/" + start.name + "/Text", User.GetConfig(endHero.heroNo).name, "red", 30);


                 });
            }


            //创建几个按钮
           // GameObject skillGo = Game.CreatePrefab(battlePanel, new GameObjectItem
           // {
           //     name = "特技按钮",
           //     text = "特技",
           //     width = 100,
           //     height = 60,
           //     prefab = "Prefabs/Button",
           //     type = 1,
           //     x = 100,
           //     y = 320,
           //     fontSize = 30,
           // });
           //
           // skillGo.GetComponent<Button>().onClick.AddListener(() =>
           // {
           //
           // });

            var skillList = new List<GameObjectItem>()
                    {
                        new GameObjectItem(){x = 150 ,y =200},
                        new GameObjectItem(){x = 250 ,y =200},
                        new GameObjectItem(){x = 350 ,y =200},
                        new GameObjectItem(){x = 450 ,y =200},
                        new GameObjectItem(){x = 550 ,y =200},
                    };


            for (int i = 0; i < skillList.Count; i++)
            {
                skillList[i].name = "skill" + i.ToString();
                skillList[i].text = "+";
                skillList[i].width = 113;
                skillList[i].height = 113;
                skillList[i].prefab = "Prefabs/Button";
                skillList[i].fontSize = 50;
                skillList[i].sprite = "hero/group/2特技框";

                Game.CreatePrefab(battlePanel, skillList[i]).GetComponent<Button>().onClick.AddListener(() =>
                {

                });
            }

        }
    }
}
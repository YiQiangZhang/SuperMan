using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleScene : MonoBehaviour
{
    
    void Init()
    {
       

      
    }

    // Start is called before the first frame update
    void Start()
    {
       
       /* Init();
        StartCoroutine(Game.yieldAll(new Action<CallBack>[]{
            (CallBack action) =>
                {
                    StartCoroutine(User.Post("/config/enum", new
                {
                }, (List<Value> res) => {
                    User.configs = res;
                    action.next();
                }));
            },
            (CallBack action) =>
            {
                StartCoroutine(User.Post("/hero/enter", new
                {
                }, (List<Hero> res) =>
                {
                    User.heros = res;
                      action.next();
                }));

            },
            (CallBack action) =>
            {
                var battleList = new List<GameObjectItem>()
                {
                    new GameObjectItem(){x = -700 ,y =80},
                    new GameObjectItem(){x = -700 ,y =-80},
                    new GameObjectItem(){x = -500 ,y =150},
                    new GameObjectItem(){x = -500 ,y =-150},
                    new GameObjectItem(){x = -300 ,y =210},
                    new GameObjectItem(){x = -300 ,y =-210},
                };
                var battlePanel = GameObject.Find("BattlePanel");

                GameObject battleGo = Game.CreatePrefab(battlePanel, new GameObjectItem
                {
                    name = "阵容保存按钮",
                    text = "阵容保存",
                    width = 140,
                    height = 60,
                    prefab = "Prefabs/Button",
                    type = 1,
                    x = -780,
                    y = 320,
                    fontSize = 30
                });
                battleGo.GetComponent<Button>().onClick.AddListener(() =>
                {
                    var ids = Enumerable.Range(1,6).Select(i=>{
                        var has = User.heros.Find(r => {
                                return r.GetValueByName("出战") ==i ;
                             });
                        return has == null ? "" : has.id;
                    });

                    StartCoroutine(User.Post("/hero/setBattle", new
                    {
                            ids = ids
                    }, (List<Hero> res) =>
                    {
                        User.heros = res;
                    }));
                });
                for (int i = 0; i < battleList.Count; i++) {
                    battleList[i].name = i.ToString();
                    var has = User.heros.Find(r => {
                        return r.GetValueByName("出战") == i + 1;
                         }
                    );
                    battleList[i].text = has == null ? "+" : User.GetConfig(has.heroNo).name;
                    battleList[i].width = 120; ;
                    battleList[i].height = 120;
                    battleList[i].prefab = "Prefabs/Button";
                    battleList[i].fontSize =  has == null ?80 : 30;
                    battleList[i].color =  has == null ?"" : "red";
                    GameObject item = Game.CreatePrefab(battlePanel, battleList[i]);
                    HeroHelper.AddChooseListener(gameObject,item, (GameObject start, GameObject end) =>
                    {
                        //先检查
                        var endHero = User.heros.Find(r => r.id == end.name);
                        var position = endHero.GetValueByName("出战");
                        if (position > 0)
                        {
                            //卸载
                            Game.ChangeText("BattlePanel/"+(position-1).ToString()+"/Text","+");
                            endHero.SetValueByName("出战",0);
                        }
                        var startHero = User.heros.Find(r=> r.GetValueByName("出战") == long.Parse(start.name)+1);
                        if(startHero !=null){
                             var startHeroPosition = startHero.GetValueByName("出战");
                            //卸载
                            Game.ChangeText("BattlePanel/"+(startHeroPosition-1).ToString()+"/Text","+");
                            startHero.SetValueByName("出战",0);
                        }

                        //更换
                        endHero.SetValueByName("出战",long.Parse(start.name)+1);
                        Game.ChangeText("BattlePanel/"+start.name+"/Text",User.GetConfig(endHero.heroNo).name);
                    });
                }


                //创建几个按钮
                GameObject skillGo = Game.CreatePrefab(battlePanel, new GameObjectItem
                {
                    name = "特技按钮",
                    text = "特技",
                    width = 120,
                    height = 60,
                    prefab = "Prefabs/Button",
                    type = 1,
                    x = 0,
                    y = 320,
                    fontSize = 30
                });

                skillGo.GetComponent<Button>().onClick.AddListener(() =>
                {

                });

                var skillList = new List<GameObjectItem>()
                {
                    new GameObjectItem(){x = 100 ,y =200},
                    new GameObjectItem(){x = 250 ,y =200},
                    new GameObjectItem(){x = 400 ,y =200},
                    new GameObjectItem(){x = 550 ,y =200},
                    new GameObjectItem(){x = 700 ,y =200},
                };


                for (int i = 0; i < skillList.Count; i++)
                {
                    skillList[i].name = "skill"+i.ToString();
                    skillList[i].text = "+";
                    skillList[i].width = 120;
                    skillList[i].height = 120;
                    skillList[i].prefab = "Prefabs/Button";
                    skillList[i].fontSize = 50;
                    Game.CreatePrefab(battlePanel, skillList[i]).GetComponent<Button>().onClick.AddListener(() =>
                    {

                    });
                }

                action.next();
            }
        }));
*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

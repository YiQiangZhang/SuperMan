using UnityEngine;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapManager
{
    public static MapManager curMapManager;
    public Dictionary<string, Room> mapDic = new Dictionary<string, Room>();
    GameObject root;
    public string CurRoom;
    public MapManager(GameObject r, string roomName)
    {
        Debug.Log(r.name);
        Debug.Log("1232312312");
        curMapManager = this;
        this.root = r;
        this.CurRoom = roomName;
        BuildRoom(roomName);
    }

    public GameObject GetBg()
    {
        return mapDic[CurRoom].bgGO;
    }

    public void BuildRoom(string roomName)
    {
        Room room = null;
        if (!mapDic.TryGetValue(roomName, out room))
        {
            var value = User.GetConfigByName(roomName);
            room = new Room(value);
            mapDic.Add(roomName, room);
            room.Build(this.root);
        }
        mapDic[this.CurRoom].bgGO.SetActive(false);
        room.bgGO.SetActive(true);
        this.CurRoom = roomName;
        //构建房间信息
        //构建玩家
    }

    public void ClearnRoom()
    {

    }
}

public class Room
{
    //房间还有入口和出口

    public Value config { get; set; }

    public List<NPC> npcs { get; set; }
    public GameObject bgGO { get; set; }

    public Room(Value v)
    {
        this.npcs = new List<NPC>();
        this.config = v;
    }

    public Vector3 GetEnterPoint(string exitPoint)
    {
        return new Vector3(0, 0, 0);
    }


    public void Build(GameObject root)
    {

        //绘制出背景
        //var bg = config.TryGet<string>("背景图");
        var width = config.TryGet<int>("宽");
        var height = config.TryGet<int>("高");
        this.bgGO = Game.CreatePrefab(root, new GameObjectItem
        {
            name = this.config.name,
            width = width,
            height = height,
            prefab = "Prefabs/bg",
            sprite = "bg/" + config.TryGet<string>("背景图"),
        });
        this.bgGO.SetActive(false);
        //绘制场景
        if (this.config.name == "郊外")
        {
            var button = Game.CreatePrefab(GameObject.Find("UI"), new GameObjectItem
            {
                name = "挂机按钮",
                x = 0,
                y = -300,
                width = 120,
                height = 120,
                prefab = "Prefabs/Button",
                sprite = "19挂机奖励按钮",
            });
            Game.CreatePrefab(button, new GameObjectItem
            {
                name = "挂机按钮文本",
                text = "挂机奖励",
                y = -50,
                width = 240,
                height = 50,
                prefab = "Prefabs/Text",
                fontSize = 25
            });
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                var guaJi = Game.CreatePrefab(GameObject.Find("UI"), new GameObjectItem
                {
                    name = "挂机",
                    prefab = "Prefabs/GuaJi",
                });

                Game.Find<Button>("GuaJiButton").onClick.AddListener(() =>
                {
                    GameObject.Destroy(guaJi);
                });
                //处理挂机界面
            });

        }

        List<Value> values = User.GetConfigByEnumTypeAndAttr("roomNPC", "房间", config.name);
        //构建NPC
        foreach (var item in values)
        {
            npcs.Add(new NPC(bgGO, item));
        }
    }

}

public class NPC
{
    public GameObject go { get; set; }

    public NPC(GameObject root, Value v)
    {

        this.config = v;
        var npcType = this.config.TryGet<string>("NPC类型");
        Debug.Log(npcType);
        var prefab = "Prefabs/room/NPC";
        if (npcType == "传送门")
            prefab = "Prefabs/room/传送门";

        this.go = Game.CreatePrefab(root, new GameObjectItem
        {
            name = v.name,
            x = this.config.TryGet<int>("x"),
            y = this.config.TryGet<int>("y"),
            prefab = prefab,
        });
        this.go.GetComponentInChildren<Text>().text = v.name;
        if (npcType == "传送门")
        {
            //增加传送事件
            this.go.GetComponent<Button>().onClick.AddListener(() =>
            {
                var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                //根据传送门按钮name，查找要传送的地图

                var buttonConfig = User.GetConfigByName(buttonSelf.name);
                var targetRoomPosition = buttonConfig.TryGet<string>("目标传送门");
                var targetConfig = User.GetConfigByName(targetRoomPosition);
                var targetRoom = targetConfig.TryGet<string>("房间");
                Debug.Log(string.Format("传送至{0} : {1}", targetRoom, targetRoomPosition));
                var g = GameObject.Find("UI").transform.Find("挂机按钮");
                Debug.Log(g);
                if (g != null)
                {
                    if (targetRoom == "郊外")
                    {
                        g.gameObject.SetActive(true);
                    }
                    else
                    {
                        g.gameObject.SetActive(false);
                    }
                }
                MapManager.curMapManager.BuildRoom(targetRoom);
            });
        }

        if (npcType == "NPC")
        {
            this.go.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                Game.ShowMessage(this.config.TryGet<string>("提示"), messageType: 1);
            });
        }

        if (npcType == "Fight")
        {
            this.go.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                FightScene.Create(root);
            });
        }

        if (npcType == "DrawCard")
        {
            this.go.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                //创建预制体抽卡
                var bagGO = GameObject.Find("抽卡");
                if (bagGO == null)
                {
                    var top = GameObject.Find("Top");
                    var drawCard = Game.CreatePrefab(top, new GameObjectItem()
                    {
                        type = 1,
                        name = "抽卡",
                        prefab = "Prefabs/Room/DrawCard",
                    });

                    var oneButton = Game.Find<Button>("DrawOne");
                    oneButton.onClick.AddListener(() =>
                    {
                        var res = User.HttpSend<Prop[]>("/game/drawCard", new
                        {
                        });
                        RewardHelper.ShowRewards(res);
                        Debug.Log(res);
                    });

                    var tenButton = Game.Find<Button>("DrawTen");
                    tenButton.onClick.AddListener(() =>
                    {
                        var res = User.HttpSend<Prop[]>("/game/drawCardTen", new
                        {
                        });
                        RewardHelper.ShowRewards(res);
                        Debug.Log(res);
                    });

                    var drawCardBack = Game.Find<Button>("DrawCardBack");
                    drawCardBack.onClick.AddListener(() =>
                    {
                        GameObject.Destroy(drawCard);
                    });
                }
            });
        }
    }

    public Value config { get; set; }


    public Rect GetRect()
    {
        return new Rect(100, 100, 100, 100);
    }
}
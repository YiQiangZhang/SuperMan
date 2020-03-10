using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class Bag 
{
    public static void ShowBag()
    {
        var bagGO = GameObject.Find("背包窗口");
        if (bagGO == null)
        {
            GameObject parent = GameObject.Find("UI");
            var bag = Game.CreatePrefab(parent, new GameObjectItem()
            {
                type = 1,
                name = "背包窗口",
                prefab = "Prefabs/Bag",
            });

            var path = "背包窗口/list/GridPanel";
            GameObject panel = GameObject.Find(path);

            Dictionary<int, UserValue> all = User.GetTotalUserValue();

            foreach (KeyValuePair<int, UserValue> item in all)
            {
                var valueConfig = User.GetConfig(item.Key);
                Debug.Log(valueConfig.name);
                Game.CreatePrefab(panel, new GameObjectItem()
                {
                    type = 1,
                    prefab = "Prefabs/Prop",
                    text = valueConfig.name,
                    name = valueConfig.name,
                    sprite = "value/1"
                });
                Game.ChangeText(string.Format("{0}/{1}/PropName", path, valueConfig.name), valueConfig.name);
                Game.ChangeText(string.Format("{0}/{1}/PropCount", path, valueConfig.name), item.Value.count.ToString());
            }

            var cannelBtn = Game.Find<Button>("BagCannel");
            cannelBtn.onClick.AddListener(() =>
            {
                GameObject.Destroy(bag);
            });
        }
    }

}

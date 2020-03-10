
using UnityEngine;
using UnityEngine.UI;

public class RewardHelper
{
    public static void ShowRewards(Prop[] props)
    {
        //创建弹窗
        var rewardsGO = GameObject.Find("奖励");
        if (rewardsGO == null)
        {
            GameObject parent = GameObject.Find("Top");
            rewardsGO = Game.CreatePrefab(parent, new GameObjectItem()
            {
                type = 1,
                name = "奖励",
                prefab = "Prefabs/Rewards",
            });

            var path = "奖励/List/Panel";
            GameObject panel = GameObject.Find(path);
            for (var i = 0; i < props.Length; i++)
            {
                var prop = Game.CreatePrefab(panel, new GameObjectItem()
                {
                    type = 1,
                    name = i.ToString(),
                    prefab = "Prefabs/Prop",
                });
                Game.ChangeText(path + "/" + i.ToString() + "/PropName", User.GetConfig(props[i].valueNo).name);
                Game.ChangeText(path + "/" + i.ToString() + "/PropCount", props[i].count.ToString());
            }
            var cannelBtn = Game.Find<Button>("RewardCannel");
            cannelBtn.onClick.AddListener(() =>
            {
                GameObject.Destroy(rewardsGO);
            });
        }
    }
}

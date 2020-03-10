using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using DG.Tweening;
using UnityEngine.UI;
using SocketIO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class MessageBase
{
    public string name;
    public int messageId;
    public string content;
    public Vector3 vector;
}


public class WorldSync
{

    public static int StartFrameCount = 0;

    public static void Init()
    {
        StartFrameCount = 0;
    }

    public static bool Compare(Vector3 last, Vector3 b)
    {
        return last.x == b.x
                 && last.y == b.y
                 && last.z == b.z;
    }
    public static Vector3 last;
    public static void Record(string sence)
    {
        GameObject role = User.GetUser();
        if (last != null)
        {
            if (Compare(last, User.GetUser().transform.position))
            {
                Debug.Log("未移动");
                return;
            }
        }
        GameObject bg = MapManager.curMapManager.GetBg();
        var bgRect = (bg.transform as RectTransform);
        var roleRect = (role.transform as RectTransform);
        Vector3 v = new Vector3(roleRect.localPosition.x , roleRect.localPosition.y , 0);

        Send(new MessageBase()
        {
            name = User.SessionID,
            messageId = Time.frameCount + StartFrameCount,
            content = sence,
            vector = v
        });
    }
    public static int count = 0;
    public static void Send(MessageBase message)
    {
        if (message.messageId % 10 == 0)
        {
            var socket = Game.Find<SocketIOComponent>("SocketIO");

            var dic = new Dictionary<string, string>()
            {
                {"name" ,message.name },
                {"messageId" ,message.messageId.ToString() },
                {"content" ,message.content },
                {"x" ,message.vector.x.ToString() },
                {"y" ,message.vector.y.ToString() },
            };
            socket.Emit("ServerListener", new JSONObject(dic), (JSONObject json) =>
            {
               // Debug.Log("ok");
            });
            last = User.GetUser().transform.position;
       
        }
        //发送消息
    }

    public static void Result(MessageBase message)
    {
        //绘制物体


    }
}

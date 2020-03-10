using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SocketManagerA : MonoBehaviour {

    // Use this for initialization

    SocketIOComponent m_socket;

    void Start()
    {
        m_socket = GetComponent<SocketIOComponent>();

        if (m_socket != null)
        {

            //系统的事件
            m_socket.On("open", OnSocketOpen);
            m_socket.On("error", OnSocketError);
            m_socket.On("close", OnSocketClose);
            //自定义的事件
            m_socket.On("ClientListener", OnClientListener);

            Invoke("SendToServer", 3);
        }
    }

    public void SendToServer()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["email"] = "some@email.com11111111111111";
        m_socket.Emit("ServerListener", new JSONObject(data), OnServerListenerCallback);

        //断开连接,会触发close事件
        //socket.Close();
    }
    #region 注册的事件

    public void OnSocketOpen(SocketIOEvent ev)
    {
        Debug.Log("OnSocketOpen updated socket id " + m_socket.sid);
    }

    public void OnClientListener(SocketIOEvent e)
    {   
        try
        {
            Debug.Log(string.Format("OnClientListener name: {0}, data: {1}", e.name, e.data));
            var cmd = e.data.GetField("cmds");
            Debug.Log("1");
            foreach (var item in cmd.list)
            {
                var last = new Vector3(float.Parse(item["x"].str), float.Parse(item["y"].str));
                var role = GameObject.Find("门卫3");
                if (role == null)
                {
                    role = Game.CreatePrefab(GameObject.Find("Canvas"), new GameObjectItem
                    {
                        name = "门卫3",
                        x = (int)last.x,
                        y = (int)last.y,
                        prefab = "Prefabs/room/Role",
                    });
                }
                Debug.Log("3");

                Vector3 v = last- role.transform.position   ;
                SpriteHelper.Update(role, v);
                Debug.Log(string.Format("{0}：{1}，{2}：{3}",role.transform.position.x, role.transform.position.x, last.x, last.y));
                role.GetComponent<RectTransform>().DOMove(last, 10 * 2f / 60);
            }
        }catch(Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
       
        Debug.Log("end");
    }

    public void OnSocketError(SocketIOEvent e)
    {
        Debug.Log("OnSocketError: " + e.name + " " + e.data);
    }

    public void OnSocketClose(SocketIOEvent e)
    {
        Debug.Log("OnSocketClose: " + e.name + " " + e.data);
    }

    #endregion

    public void OnServerListenerCallback(JSONObject json)
    {
        Debug.Log(string.Format("OnServerListenerCallback data: {0}", json));
    }
}

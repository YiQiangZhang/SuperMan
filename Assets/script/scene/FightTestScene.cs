using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FightTestScene : MonoBehaviour {
    NetworkClient client;
    void StartClient()
    {
        var client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, (NetworkMessage message) =>
        {
            Debug.Log("sgType.Connect");

        });

        client.RegisterHandler(MsgType.Disconnect, (NetworkMessage message) =>
        {
            Debug.Log("sgType.Disconnect");

        });

        client.RegisterHandler(MsgType.Error, (NetworkMessage message) =>
        {
            Debug.Log("sgType.CoErrornnect");

        });
        client.Connect("127.0.0.1", 8078);
    }

    void StartServer()
    {

    }
    // Use this for initialization
    void Start () {

        
        FightScene.Create(gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

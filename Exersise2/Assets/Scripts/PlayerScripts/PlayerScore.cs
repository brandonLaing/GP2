using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour {

  [SyncVar]
  public int score = 0;
  [SyncVar]
  public string playerName = string.Empty;

  public int Score
  {
    get
    {
      return score;
    }
    set
    {
      if (isServer)
      {
        score = value;
        if (score < 0)
        {
          score = 0;
        }
      }
    }
  }

  public int spacePressedCount = 0;
  private ChatController myChat;

  private void Start()
  {
    if (GetComponent<NetworkIdentity>().isLocalPlayer)
    {
      Debug.Log("Player score Start");
      myChat = GameObject.FindGameObjectWithTag("ChatController").GetComponent<ChatController>();
    }
  }

  private void Update()
  {
    if (GetComponent<NetworkIdentity>().isLocalPlayer)
    {
      if (myChat.name != playerName)
      {
        playerName = myChat.name;
      }
    }
  }
}

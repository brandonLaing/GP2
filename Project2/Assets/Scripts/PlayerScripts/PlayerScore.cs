using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour {

  [SyncVar]
  public int score = 0;
  [SyncVar]
  public string playerName = string.Empty;

  private ScoreBoardController scoreBoard;

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
        if (value > 0)
        {
          score = value;
        }
        else
        {
          score = 0;
        }
      }
    }
  }

  private ChatController myChat;

  private void Start()
  {
    if (GetComponent<NetworkIdentity>().isLocalPlayer)
    {
      Debug.Log("Player score Start");
      myChat = GameObject.FindGameObjectWithTag("ChatController").GetComponent<ChatController>();
    }

    scoreBoard = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<ScoreBoardController>();
    scoreBoard.AddNewPlayer(this);
  }

  // adds point to score and sends message to update score board
  public void AddPoint(int points)
  {
    Score += points;
    scoreBoard.UpdatePlayerScore(this);
  }

  // constantly checks if the user name is correct and updates if it isnt
  private void Update()
  {
    if (GetComponent<NetworkIdentity>().isLocalPlayer)
    {
      if (myChat.name != playerName)
      {
        playerName = myChat.GetPlayerName();
        scoreBoard.UpdatePlayerName(this);
      }
    }

    if (scoreBoard.GetDisplayedScore(this) != Score)
    {
      scoreBoard.UpdatePlayerScore(this);
    }
  }

  // on destroy removes this player from score board
  private void OnDestroy()
  {
    scoreBoard.RemovePlayer(this);
  }
}

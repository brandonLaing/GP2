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

  public int MaxScore = 20;

  public GameController gameController;

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
      CmdSetPlayerName(myChat.localPlayerName);
    }

    gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    scoreBoard = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<ScoreBoardController>();
    scoreBoard.AddNewPlayer(this);
  }

  [Command]
  void CmdSetPlayerName(string playerName)
  {
    this.playerName = playerName;
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
    if (isLocalPlayer)
    {
      if (playerName != myChat.localPlayerName)
      {
        CmdSetPlayerName(myChat.localPlayerName);

      }
    }
    //if (isLocalPlayer)
    //{
    //  playerName = myChat.localPlayerName;
    //}

    if (scoreBoard.GetDisplayedScore(this) != Score)
    {
      scoreBoard.UpdatePlayerScore(this);
    }

    CheckScore();

    scoreBoard.UpdatePlayerName(this);

  }

  private void CheckScore()
  {
    if (score >= MaxScore)
    {
      gameController.EndGame(this.gameObject);
    }
  }

  // on destroy removes this player from score board
  private void OnDestroy()
  {
    scoreBoard.RemovePlayer(this);
  }
}

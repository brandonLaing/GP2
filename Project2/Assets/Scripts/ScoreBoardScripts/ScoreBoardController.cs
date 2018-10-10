using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardController : MonoBehaviour
{
  private List<GameObject> players = new List<GameObject>();
  private List<ScoreBoardPlayerInfo> playerInfos = new List<ScoreBoardPlayerInfo>();

  public GameObject playerScoreUiInfoPrefab;
  public Transform playerInfoHolder;

  public GameObject scoreBoardObject;

  private void Start()
  {
    scoreBoardObject.SetActive(false);
  }

  // Checks to display scoreboard
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab))
    {
      scoreBoardObject.SetActive(true);
    }
    if (Input.GetKeyUp(KeyCode.Tab))
    {
      scoreBoardObject.SetActive(false);
    }
  }

  // updates players score
  public void UpdatePlayerScore(PlayerScore player)
  {
    foreach (ScoreBoardPlayerInfo playerInfo in playerInfos)
    {
      if (playerInfo.associatedPlayer == player)
      {
        playerInfo.SetScoreText(player.score);

      }
    }
  }

  // updates player name
  public void UpdatePlayerName(PlayerScore playerScore)
  {
    foreach (ScoreBoardPlayerInfo playerInfo in playerInfos)
    {
      if (playerScore == playerInfo.associatedPlayer)
      {
        playerInfo.SetNameText(playerScore.playerName);
      }
    }
  }

  // addes a new player and sets everything up
  public void AddNewPlayer(PlayerScore playerScore)
  {
    foreach (ScoreBoardPlayerInfo playerInfo in playerInfos)
    {
      if (playerScore == playerInfo.associatedPlayer)
      {
        return;
      }
    }
    ScoreBoardPlayerInfo newInfo = Instantiate(playerScoreUiInfoPrefab, playerInfoHolder).GetComponent<ScoreBoardPlayerInfo>();
    newInfo.associatedPlayer = playerScore;
    newInfo.SetNameText(playerScore.playerName);
    newInfo.SetScoreText(0);
    playerInfos.Add(newInfo);
  }

  // remvoes player
  public void RemovePlayer(PlayerScore playerScore)
  {
    foreach (ScoreBoardPlayerInfo playerInfo in playerInfos)
    {
      if (playerScore == playerInfo.associatedPlayer)
      {
        Destroy(playerInfo.gameObject);
        playerInfos.Remove(playerInfo);
      }
    }
  }

  public int GetDisplayedScore(PlayerScore playerScore)
  {
    foreach (ScoreBoardPlayerInfo playerInfo in playerInfos)
    {
      if (playerScore == playerInfo.associatedPlayer)
      {
        return playerInfo.GetScoreDisplayed();
      }
    }
    return -1;
  }
}

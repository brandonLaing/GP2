using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardPlayerInfo : MonoBehaviour
{
  public Text nameText;
  public Text scoreText;
  public PlayerScore associatedPlayer;

  public void SetNameText(string name)
  {
    nameText.text = name;
  }

  public void SetScoreText(int score)
  {
    scoreText.text = score.ToString() ;
  }

  public string GetNameDisplayed()
  {
    return nameText.text;
  }
  public int GetScoreDisplayed()
  {
    return System.Convert.ToInt32(scoreText.text);
  }
}

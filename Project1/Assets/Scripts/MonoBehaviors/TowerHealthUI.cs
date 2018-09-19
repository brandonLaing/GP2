using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthUI : MonoBehaviour
{
  private TowerController tower;
  private Text textBox;

  private void Start()
  {
    textBox = GetComponent<Text>();
  }

  private void Update()
  {
    if (tower == null)
    {
      try
      {
        tower = GameObject.FindGameObjectWithTag("Tower").GetComponent<TowerController>();

      }
      catch
      {
        Debug.Log("Couldnt Find a tower");

      }
    }
    else
    {
      textBox.text = "Tower Health: " + tower.towerHealth;
    }

  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
  public int towerHealth = 200;

  private void Update()
  {
    if (towerHealth <= 0)
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");

    }
  }
  public void TakeDamage(int damage)
  {
    towerHealth -= damage;

  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

  private float _health;
  public float Health
  {
    get
    {
      return _health;
    }
    private set
    {
      if (value < minHealth)
      {
        Kill();
        _health = minHealth;
      }
      else if (value > maxHealth)
      {
        _health = maxHealth;
      }
      else
      {
        _health = value;
      }
      currentHealth = (int)Health;
    }
  }

  public int currentHealth;

  public float startingHealth;
  public float minHealth;
  public float maxHealth;

  public bool healOverTime;
  public float healthPerSecond;
  public bool dontHealWhileAttacked;
  public float timeToWait;

  public DateTime lastAttack;

  private void Start()
  {
    Health = startingHealth;

    lastAttack = DateTime.Now;

    StartCoroutine(HealOverTime());
  }

  private IEnumerator HealOverTime()
  {
    while (true)
    {
      if (healOverTime)
      {
        if (DateTime.Now - lastAttack > TimeSpan.FromSeconds(timeToWait) || !dontHealWhileAttacked)
        {
          Heal(healthPerSecond * Time.deltaTime);
        }
      }

      yield return new WaitForEndOfFrame();
    }
  }
  public void Heal(float heal)
  {
    Health += heal;
  }

  public void Damage(float damage)
  {
    lastAttack = DateTime.Now;
    Health -= damage;
  }

  public void Kill()
  {

  }
}

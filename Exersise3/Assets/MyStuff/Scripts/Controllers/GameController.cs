using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameController : MonoBehaviour
{
  public List<GameObject> allGlowables = new List<GameObject>();

  public List<GameObject> chosenGlows = new List<GameObject>();
  public Dictionary<string, object> foundObjects = new Dictionary<string, object>();

  public DateTime startTime;

  [Range(0, 10)]
  public int objectsToGlow = 2;

  private void Start()
  {
    startTime = DateTime.Now;

    // find all the props
    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Props"))
    {
      allGlowables.Add(obj);
    }

    // chose some random objects for glow
    for (int i = 0; i < objectsToGlow && allGlowables.Count > 0; i++)
    {
      ChooseObjectToGlow();

    }

    // actually add glows to those objects
    foreach (GameObject obj in chosenGlows)
    {
      AddGlows(obj);
    }

  }

  private void Update()
  {
    // shift escape exit game
    if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.LeftShift))
    {
      ExitGame();
    }
  }

  // gets a random object from allGlowables then removes it from there and adds it to the list of glowables.
  private void ChooseObjectToGlow()
  {
    int randomInt = UnityEngine.Random.Range(0, allGlowables.Count);

    chosenGlows.Add(allGlowables[randomInt]);
    allGlowables.RemoveAt(randomInt);
  }

  // add the glow componet to a object and assigns it a random glow color and tells it not to glow
  private void AddGlows(GameObject obj)
  {
    GlowObject go = obj.AddComponent<GlowObject>();
    go.glowColor = UnityEngine.Random.ColorHSV(0.2F, 1, 0.2F, 1, 0.2F, 1, 0, 0);
    go.Glow(false);
  }

  // Called when object is found.
  // passes its object and how long it took to find
  public void ObjectFound(GameObject glowObj, TimeSpan timeToFind)
  {
    foundObjects.Add(glowObj.name, timeToFind.Seconds);
    chosenGlows.Remove(glowObj);

    if (chosenGlows.Count == 0) NextLevel();
  }

  // once all objects are found send out analytics and load new scene
  private void NextLevel()
  {
    // send out event
    // time to finish the level is on the last object
    Analytics.CustomEvent("player_finished_level", foundObjects);
    // load next scene
    SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
  }

  // quits the game but before it does it sends out an update with the time that the player quit at and each object they player had to find
  private void ExitGame()
  {
    TimeSpan timeToQuit = DateTime.Now.Subtract(startTime);

    // add all the remaining objects to the scene with the quit time
    foreach (GameObject obj in chosenGlows)
    {
      foundObjects.Add(obj.name, timeToQuit.Seconds);
    }

    // send the event
    Analytics.CustomEvent("player_quit_level", foundObjects);

    // quit the game
    Application.Quit();
    SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    Debug.LogError("Game Quit");
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
  public ObstaclePlacer obPlacer;
  public GameObject aiPrefab;

  public TileGenerator tileGen;

  public List<AIProjectOne> allAIOnBoard = new List<AIProjectOne>();

  public Transform endPoint;

  public Transform[] spawnPoints;

  public int roundNumber = 0;

  private int startPosition;

  private int upPositions = 0;
  private int upChange = 1;

  private int downPositions = 0;
  private int downChange = 1;

  private int extraHealth = 0;

  private bool aiTurn = false;
  private bool rebuildingPath = false;

  private void Start()
  {
    startPosition = spawnPoints.Length / 2;

  }

  private void Update()
  {
    CheckIfAIStillMoving();

  }

  public void CheckIfAIStillMoving()
  {
    if (aiTurn && allAIOnBoard.Count == 0)
    {
      EndRound();

    }
  }

  public void StartNextRound()
  {
    obPlacer.enabled = false;
    SpawnAIForNextRound();

  }

  public void EndRound()
  {
    obPlacer.enabled = true;
    aiTurn = false;

  }

  public void SpawnAIForNextRound()
  {
    if (!aiTurn)
    {
      aiTurn = true;

      roundNumber++;

      obPlacer.tilesToPlace += (roundNumber * 2 + 3);

      for (int i = startPosition - downPositions; i <= startPosition + upPositions; i++)
      {
        GameObject newAI = Instantiate(aiPrefab, spawnPoints[i].transform.position + Vector3.up, aiPrefab.transform.rotation);

        AIProjectOne newAIScript = newAI.GetComponent<AIProjectOne>();

        newAIScript.currentNode = spawnPoints[i].GetComponent<TileInfo>().tileNode;

        newAIScript.endNode = endPoint.GetComponent<TileInfo>().tileNode;

        newAIScript.tileGen = this.tileGen;

        newAIScript.PathFindToEnd();

        newAIScript.roundMng = this;

        newAIScript.energy += extraHealth;

        allAIOnBoard.Add(newAIScript);

      }
    }

    if (upPositions + downPositions + 1 < spawnPoints.Length)
    {
      downPositions += downChange;
      upPositions += upChange;

    }

    else
    {
      extraHealth += 2;
    }

  }

  public void RebuildRoots()
  {
    if (!rebuildingPath)
    {
      StartCoroutine(RebuildCoroutine());

    }

    else
    {
      Debug.LogWarning("Already rebuilding path");

    }

  }

  private IEnumerator RebuildCoroutine()
  {
    rebuildingPath = true;

    foreach(AIProjectOne ai in allAIOnBoard)
    {
      ai.RebuildPath();

      yield return new WaitForEndOfFrame();
    }

    rebuildingPath = false;
  }
}

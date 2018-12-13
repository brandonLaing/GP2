using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
  public GameObject blackMoku, whiteMoku;

  public bool isBlacksTurn = true;

  public GameObject[,] stones = new GameObject[19, 19];

  private Vector2Int[] checkDirections = new Vector2Int[]
  {
    Vector2Int.up,
    Vector2Int.down,
    Vector2Int.right,
    Vector2Int.left
  };

  // Use this for initialization
  void Start()
  {

    GameObject[] existingStones = GameObject.FindGameObjectsWithTag("Moku");
    for (int i = 0; i < existingStones.Length; i++)
    {
      Vector3 existingStoneLocation = existingStones[i].transform.position;
      stones[Mathf.RoundToInt(existingStoneLocation.x), Mathf.RoundToInt(existingStoneLocation.y)] = existingStones[i];
    }

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      int bx = Mathf.RoundToInt(mousePosition.x), by = Mathf.RoundToInt(mousePosition.y);

      if (bx >= 0 && bx <= 18 && by >= 0 && by <= 18 && stones[bx, by] == null)
      {
        GameObject newMoku = GameObject.Instantiate(isBlacksTurn ? blackMoku : whiteMoku);
        newMoku.transform.position = new Vector3(bx, by, 0);

        if (isBlacksTurn)
          newMoku.GetComponent<MokuInfo>().mokuColor = MokuColor.black;
        else
          newMoku.GetComponent<MokuInfo>().mokuColor = MokuColor.white;

        stones[bx, by] = newMoku;

        RemoveCapturedStones();

        isBlacksTurn = !isBlacksTurn;
      }
    }
  }

  private List<Vector2Int> checkedPositions;
  private StringBuilder sb;

  private void RemoveCapturedStones()
  {
    // TODO: Fill out this function.
    // Any stones that are now surrounded because of the placement of the new stone
    // should be removed.

    // You do NOT need to worry about scoring - just remove any stones that are captured.
    List<Vector2Int> tilesToCheck = new List<Vector2Int>();


    for (int i = 0; i < 19; i++)
    {
      for (int j = 0; j < 19; j++)
      {
        if (stones[i, j] != null)
        {
          sb = new StringBuilder();
          sb.AppendLine("Starting Check for (" + i + ", " + j + ")"); sb.AppendLine();
          checkedPositions = new List<Vector2Int>();
          // start checking current moku

          if (!CheckForOpening(new Vector2Int(i, j)))
          {
            sb.AppendLine("Didn't find an opening for (" + i + ", " + j + ")");

            foreach (Vector2Int position in checkedPositions)
            {
              if (stones[position.x, position.y].GetComponent<MokuInfo>().mokuColor == MokuColor.black && !isBlacksTurn)
              {
                Destroy(stones[position.x, position.y]);
                stones[position.x, position.y] = null;
              }
              else if (stones[position.x, position.y].GetComponent<MokuInfo>().mokuColor == MokuColor.white && isBlacksTurn)
              {
                Destroy(stones[position.x, position.y]);
                stones[position.x, position.y] = null;
              }
            }
          }

          Debug.Log(sb);
        }
      }
    }
  }


  // true = there is a opening, false = there isnt an opening
  private bool CheckForOpening(Vector2Int startStonePosition)
  {
    // get the gameObject for this moku
    var startTile = stones[startStonePosition.x, startStonePosition.y];
    // say we have checked this moku
    checkedPositions.Add(startStonePosition);

    sb.AppendLine("Checking For Opening on " + startStonePosition);

    // go through each of its connections
    for (int i = 0; i < checkDirections.Length; i++)
    {
      // grab that ones gameObject
      var checkedTile = CheckPositionForEmpty(startStonePosition, checkDirections[i]);
      sb.AppendLine("Checking tile" + (startStonePosition + checkDirections[i]));

      // check if the checked is null and if it is exit the loop
      if (checkedTile == null)
      {
        sb.AppendLine((startStonePosition + checkDirections[i]) + " is null");

        return true;
      }
      else if (checkedTile != this.gameObject && startTile.GetComponent<MokuInfo>().mokuColor == checkedTile.GetComponent<MokuInfo>().mokuColor)
      {
        sb.AppendLine((startStonePosition + checkDirections[i]) + " is the same type as " + startStonePosition);
        if (!checkedPositions.Contains(startStonePosition + checkDirections[i]))
        {
          if (CheckForOpening(startStonePosition + checkDirections[i]))
          {
            return true;
          }
        }
      }
    }

    return false;
  }

  private GameObject CheckPositionForEmpty(Vector2Int position, Vector2Int direction)
  {
    var checkPos = position + direction;

    try
    {
      if (stones[checkPos.x, checkPos.y] != null)
      {
        return stones[checkPos.x, checkPos.y];
      }
    }
    // this isnt great but its an easy fix
    catch
    {
      return this.gameObject;
    }

    return null;
  }
}

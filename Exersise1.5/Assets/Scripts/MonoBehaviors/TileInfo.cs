using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** TileInfo Does:
 * This holds the node for each tile
 * and works with the AI to send paths to the current position and moving the player to this location
 */
public class TileInfo : MonoBehaviour
{
  public Node tileNode;   // node that holds info about his tile

  // sets the AI to this nodes transform
  public void MoveAIToCordinates(bool debug, AI ai)
  {
    if (debug)
    {
      Debug.Log("New Start Node Set: " + this.transform.name + "\n");

    }

    ai.transform.position = new Vector3(this.transform.position.x, ai.transform.position.y, this.transform.position.z);

  }

  // creates list of way points from the A* system if it fails it can send a debug message
  public void SendCordinatesToAI(bool debug, AI ai)
  {
    // if debugging is enables it shows your choices connections
    if (debug)
    {
      tileNode.DisplayConnections();

    }

    // makes new vector3 list for the way points
    List<Vector3> vectorList;

    // tries to make a A* way point list then add those points to the vectorList then sets the new start node to this node
    try
    {
      vectorList = PathFinder.Dijkstra(ai.startNode, this.tileNode);

      foreach (Vector3 vector3 in vectorList)
      {
        ai.moveQue.Add(vector3);

      }

      ai.startNode = this.tileNode;

    } catch // if it cant find a path say that and return a empty list
    {
      if (debug)
      {
        Debug.LogWarning("Couldn't find a path to: " + this.tileNode.transform.name);

      }
    }
  }
}

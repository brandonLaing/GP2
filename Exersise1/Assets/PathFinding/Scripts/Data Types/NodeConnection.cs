using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnection : IComparable<NodeConnection>
{
  public Node startNode;
  public Node endNode;

  public int weightCarried;

  public NodeConnection previousConnection;

  #region Constructors
  // This sets up the first connection in the chain
  public NodeConnection(Node start)
  {
    startNode = start;
    endNode = start;
    previousConnection = null;
    weightCarried = 0;

  }

  public NodeConnection(Node node, NodeConnection connection)
  {

    startNode = connection.endNode;
    endNode = node;
    weightCarried += connection.weightCarried;
    previousConnection = connection;

  }

  // whenever a new node is found to make a connection it makes a new connection through here
  public NodeConnection(NodeConnection connection, Node newNode, int weightAdded)
  {
    Debug.Log("Making new node from another");

    startNode = connection.endNode;
    endNode = newNode;
    previousConnection = connection;

    Debug.Log("Weight1: " + connection.weightCarried + "\nWeight2: " + weightAdded + "\nWeightCombined: " + (connection.weightCarried + weightAdded));

    weightCarried = connection.weightCarried + weightAdded;

    Debug.Log(weightCarried);


  }

  // meant for testing and not really for actual use
  public NodeConnection(Node start, Node end, int weight)
  {
    startNode = start;
    endNode = end;
    weightCarried = weight;

  }

  #endregion

  #region Others

  #region Idea that would be cool to work
  //public NodeConnection<T> operator +(NodeConnection<T> connection1, Node newNode, int weightAdded)
  //{
  //  return new NodeConnection<T>(connection1, newNode, weightAdded);


  //}
    
  #endregion

  // Instead of having to search through every and find cheapest this will all me to just sort the list of connections by weight
  public int CompareTo(NodeConnection other)
  {
    return weightCarried.CompareTo(other.weightCarried);

  }

  #endregion

}

#region Retired
//public NodeConnection(Node start, Node end)
//{
//  startNode = start;
//  end = endNode;

//}

//public NodeConnection(Node start, Node end, int weight)
//{
//  startNode = start;
//  endNode = end;
//  weightCarried = weight;

//}

//public class NodeConnectionRoot
//{
//  private NodeConnection root;

//  public NodeConnection Root()
//  {
//    return root;

//  }

//  public NodeConnectionRoot(NodeConnection rootConnection)
//  {
//    rootConnection.startNode = null;
//    rootConnection.previousConnection = null;

//    this.root = rootConnection;

//  }

//}

#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatController : MonoBehaviour {

  private Dictionary<int, string> namesByConnectionId = new Dictionary<int, string>();
  private Dictionary<string, int> connectionIdsByName = new Dictionary<string, int>();

  private Color[] playerColors = { Color.red, Color.blue, Color.green, Color.black, Color.white, Color.yellow };

  public int maxNameLength = 20;
  public string localPlayerName;

  public List<string> messages = new List<string>();

  public string GetPlayerName()
  {
    return localPlayerName;
  }

  /** OnChatMessageReceived:
   * Adds the chat message to the list of messages
   */
  public void OnChatMessageReceived(string sender, string message)
  {
    messages.Add(string.Format("{0} : {1}", sender, message));

  }

  /** GetNameById:
   * Simply returns the name when given the connection Id
   */
  public string GetNameById(int connectionId)
  {
    return namesByConnectionId[connectionId];
  }

  /** GetIdByName:
 * Simply returns the connection Id when given the name
 */
  public int GetIdByName(string name)
  {
    return connectionIdsByName[name];
  }

  /** SetLocalPlayerName:
   * sets the local player name after reciving it from the server
   */
  public void SetLocalPlayerName(string playerName)
  {
    localPlayerName = playerName;

  }

  /** AnnouncePlayer:
   * sends a message saying a new player has joined
   */
  public void AnnouncePlayer(string playerName)
  {
    messages.Add(string.Format("***** Player {0} joined *****", playerName));
    Debug.Log("Pretty sure I'm announcing a player named " + playerName);
  }

  /** SetPlayerName:
   * when the client gets a name back from the client it calls to set that name and makes sure that name is uniqe
   */
  internal string SetPlayerName(string playerName, int connectionId)
  {
    playerName = EnsureUnique(playerName, connectionId);

    if (namesByConnectionId.ContainsKey(connectionId))
    {
      connectionIdsByName.Remove(namesByConnectionId[connectionId]);
      namesByConnectionId.Remove(connectionId);

    }

    connectionIdsByName.Add(playerName, connectionId);
    namesByConnectionId.Add(connectionId, playerName);

    return playerName;
  }

  // this is the only thing i copied over
  private string EnsureUnique(string playerName, int connectionId)
  {
    if (namesByConnectionId.ContainsKey(connectionId) && namesByConnectionId[connectionId] == playerName) return playerName;

    // Somebody else has this name; return an altered form of it.
    int suffix = 0;
    while (connectionIdsByName.ContainsKey(playerName))
    {
      string suffixString = (++suffix).ToString();
      while (playerName.Length + suffixString.Length > maxNameLength) playerName = playerName.Substring(0, playerName.Length - 1);
      playerName += suffixString;
    }

    return playerName;

  }
}

class PlayerConnectionInfo
{
  public int connectionId;
  public string playerName;
  public Color playerColor;

  public PlayerConnectionInfo(int connectionId, string playerName, Color playerColor)
  {
    this.connectionId = connectionId;
    this.playerName = playerName;
    this.playerColor = playerColor;
  }
}
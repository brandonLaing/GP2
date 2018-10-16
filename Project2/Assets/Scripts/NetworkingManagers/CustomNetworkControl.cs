using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class CustomNetworkControl : NetworkManager
{
  public string playerName;
  public string ipAdress;
  private ChatController myChat;

  #region Const network Id message numbers
  private const short PlayerNameMessage = 2001;
  private const short AssignPlayerNameMessage = 2002;
  private const short PlayerJoinedGameMessage = 2003;

  private const short PlayerSendChatMessageId = 3000;
  private const short ChatMessageReceived = 3001;
  #endregion

  public class ChatMessage : MessageBase
  {
    public string sender;
    public string message;
  }

  /** OnClientConnect:
   * Called when connected to server and sends back info on its player name back to the server
   */
  public override void OnClientConnect(NetworkConnection conn)
  {
    Debug.Log("Received connect message");
    myChat = GameObject.FindGameObjectWithTag("ChatController").GetComponent<ChatController>();
    client.Send(PlayerNameMessage, new StringMessage(playerName));
  }

  /** StartNetworkHost:
   * This starts up the host for a sever
   */
  public void _StartNetworkHost()
  {
    Debug.Log("Starting host and registering handler");

    StartHost();

    networkAddress = GetLocalIP();
    ipAdress = GetLocalIP();

    RegisterSeverListeners();
    RegisterClientListeners();
  }
  
  /** StartNetworkClient:
   * this sets this user to be a client
   */
  public void _StartNetworkClient()
  {
    Debug.Log("Starting client and registering handler");
    if (ipAdress != null)
    {
      networkAddress = ipAdress;
    }
    else
    {
      networkAddress = "localhost";
    }

    StartClient();
    RegisterClientListeners();
  }

  /** RegisterClientListeners:
   * Sets up haneler for assigning a player name, knowing when someone joins a game, and reciving a chat message
   */
  private void RegisterClientListeners()
  {
    client.RegisterHandler(AssignPlayerNameMessage, OnNameAssigned);
    client.RegisterHandler(PlayerJoinedGameMessage, OnOtherPlayerJoinedGame);

    client.RegisterHandler(ChatMessageReceived, OnChatMessageRecived);
  }

  /** OnChatMessageRecived:
   * This is called when the network recives a message and it takes the message class and sends it to the chat controller
   */
  public void OnChatMessageRecived(NetworkMessage netMsg)
  {
    ChatMessage received = netMsg.ReadMessage<ChatMessage>();

    myChat.OnChatMessageReceived(received.sender, received.message);
  }

  /** OnOtherPlayerJoinedGame:
   * Called when a message from the network says another player has joined and sends that to the chat controller
   */
  public void OnOtherPlayerJoinedGame(NetworkMessage netMsg)
  {
    string playerName = netMsg.ReadMessage<StringMessage>().value;
    myChat.AnnouncePlayer(playerName);

    Debug.Log("Other player joined message recived, name is " + playerName);
  }

  /** OnNameAssigned:
   * This gets the net message for a player name then sets it in the chat controller.
   */
  public void OnNameAssigned(NetworkMessage netMsg)
  {
    string playerName = netMsg.ReadMessage<StringMessage>().value;
    myChat.SetLocalPlayerName(playerName);

    Debug.Log("Client player name assigned to " + playerName);
  }

  /** RegisterSeverListeners:
   * sets up listeners for the server
   */
  private void RegisterSeverListeners()
  {
    NetworkServer.RegisterHandler(PlayerNameMessage, OnPlayerNameReceived);
    NetworkServer.RegisterHandler(PlayerSendChatMessageId, PlayerSendChatMessage);
  }

  /** OnPlayerSendChatMessage:
   * this is caught by the host and it modifies a incoming message then sends it back to all the other players
   */
  public void PlayerSendChatMessage(NetworkMessage netMsg)
  {
    string message = netMsg.ReadMessage<StringMessage>().value.Trim();

    if (message.Length > 100)
    {
      message = message.Substring(0, 100).Trim();
    }

    string senderName = myChat.GetNameById(netMsg.conn.connectionId);

    foreach (NetworkInstanceId n in netMsg.conn.clientOwnedObjects)
    {
      GameObject G = NetworkServer.FindLocalObject(n);
      if (G.GetComponent<PlayerMatUpdate>() != null)
      {
        senderName += " (" + G.GetComponent<PlayerMatUpdate>().GetColorName() + ") ";
      }
    }

    ChatMessage chatMessage = new ChatMessage() { sender = senderName, message = message };
    NetworkServer.SendToAll(ChatMessageReceived, chatMessage);
  }

  /** OnPlayerNameReceived:
   * When a players name is recived it sends message back to the client to give the client its name, it then sends a message to all the clients that a new player has joined
   */
  public void OnPlayerNameReceived(NetworkMessage netMsg)
  {
    string playerName = netMsg.ReadMessage<StringMessage>().value;
    playerName = myChat.SetPlayerName(playerName, netMsg.conn.connectionId);

    // add stuff from 128 to 135 and set the player name in score
    foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
    {
      PlayerScore pS = player.GetComponent<PlayerScore>();
      if (pS != null)
      {
        pS.playerName = playerName;
      }
    }

    NetworkServer.SendToClient(netMsg.conn.connectionId, AssignPlayerNameMessage, new StringMessage(playerName));
    NetworkServer.SendToAll(PlayerJoinedGameMessage, new StringMessage(playerName));

    Debug.Log("Recived player name " + playerName);
  }

  /** OnSendChatMessage:
   * This sends a message to the sever to let it know a message has been sent
   */
  public void OnSendChatMessage(string message)
  {
    client.Send(PlayerSendChatMessageId, new StringMessage(message));
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.LeftShift))
    {
      Application.Quit();
    }
  }

  public void UpdateIpFromInputBox(string ip)
  {
    Debug.Log("updateing ip");

    if (ValidateIp(ip))
    {
      ipAdress = ip;
      networkAddress = ip;

    }
  }

  string GetLocalIP()
  {
    string hostName = System.Net.Dns.GetHostName();

    foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostEntry(hostName).AddressList)
    {
      try
      {

      if (ValidateIp(ip))
      {
        return ip.ToString();
      }
      }
      catch
      {
        Debug.Log(ip);

      }
    }

    return string.Empty;
  }

  private bool ValidateIp(System.Net.IPAddress ip)
  {
    string[] ipSplit = ip.ToString().Trim().Split('.');
    bool temp = false;
    for (int i = 0; i < ipSplit.Length; i++)
    {
      if (i == 0)
      {
        if (Convert.ToInt16(ipSplit[i]) >= 1 && Convert.ToInt16(ipSplit[i]) <= 233)
        {
          temp = true;
        }
        else
        {
          return false;
        }
      }
      else
      {
        if (Convert.ToInt16(ipSplit[i]) >=0 && Convert.ToInt16(ipSplit[i]) < 255)
        {
          temp = true;
        }
        else
        {
          return false;
        }
      }
    }
    return temp;
  }

  private bool ValidateIp(string ip)
  {
    string[] ipSplit = ip.ToString().Trim().Split('.');
    bool temp = false;

    if (ipSplit.Length != 4)
    {
      return false;
    }

    for (int i = 0; i < ipSplit.Length; i++)
    {
      if (i == 0)
      {
        if (!(Convert.ToInt16(ipSplit[i]) >= 1 && Convert.ToInt16(ipSplit[i]) <= 233))
        {
          return false;
        }
      }
      else
      {
        if (!(Convert.ToInt16(ipSplit[i]) >= 0 && Convert.ToInt16(ipSplit[i]) < 255))
        {
          return false;
        }
      }
    }

    return true;
  }
}

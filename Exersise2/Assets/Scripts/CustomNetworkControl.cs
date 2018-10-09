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
    RegisterSeverListeners();
    RegisterClientListeners();
  }

  /** StartNetworkClient:
   * this sets this user to be a client
   */
  public void _StartNetworkClient()
  {
    Debug.Log("Starting client and registering handler");

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
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.Quit();
    }
  }

}

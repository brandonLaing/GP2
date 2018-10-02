using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPannelController : MonoBehaviour
{
  public Text chatTextBox;
  public InputField messageInput;
  public Button sendButton;

  public int numberOfLines = 5;

  public List<string> displayedLines = new List<string>();

  private CustomNetworkControl myNetworkControl;
  private ChatController myChat;

  private void Start()
  {
    chatTextBox.text = string.Empty;

    messageInput.ActivateInputField();
    messageInput.Select();

    try
    {
      myNetworkControl = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<CustomNetworkControl>();
    }
    catch
    {
      Debug.LogError("Couldnt get a networkController");
    }

    myChat = GameObject.FindGameObjectWithTag("ChatController").GetComponent<ChatController>();
  }

  private void Update()
  {
    if (myNetworkControl != null)
    {
      displayedLines.Clear();
      for (int i = myChat.messages.Count - 1; i >= 0 && displayedLines.Count < numberOfLines; i--)
      {
        displayedLines.Add(myChat.messages[i]);
      }
      displayedLines.Reverse();
      RefreshMessageDisplay();
    }
    if (Input.GetKeyDown(KeyCode.Return) && !messageInput.IsActive())
    {
      messageInput.ActivateInputField();
      messageInput.Select();

    }

    if (Input.GetKeyDown(KeyCode.Escape) && messageInput.IsActive())
    {
      messageInput.text = string.Empty;
      messageInput.DeactivateInputField();
    }
  }

  public void _SendMessage()
  {
    string message = messageInput.text.Trim();
    if (message.Length > 0)
    {
      if (myNetworkControl == null)
      {
        displayedLines.Add(message);
        if (displayedLines.Count > numberOfLines)
        {
          displayedLines.RemoveAt(0);
        }

        RefreshMessageDisplay();
      }
      else
      {
        myNetworkControl.OnSendChatMessage(message);
      }

      messageInput.text = string.Empty;
    }
  }

  private void RefreshMessageDisplay()
  {
    chatTextBox.text = string.Empty;
    for (int i = 0; i < displayedLines.Count; i++)
    {
      chatTextBox.text += displayedLines[i];
      if (i < displayedLines.Count - 1) chatTextBox.text += "\n";

    }

  }

  public void _OnEndSendEdit()
  {
    if (Input.GetKeyDown(KeyCode.Return))
    {
      _SendMessage();
    }
  }
}

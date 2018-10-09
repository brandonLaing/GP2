using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatIpController : NetworkBehaviour
{
  public Text IpTextBox;

  private ChatController myChat;
  private CustomNetworkControl myNetworkControl;

  private void Start()
  {
    myChat = GetComponentInParent<ChatController>();

    try
    {
      myNetworkControl = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<CustomNetworkControl>();
    }
    catch
    {
      Debug.LogError("Couldnt get a networkController");
    }

    if (isServer)
    {
      IpTextBox.text += myNetworkControl.networkAddress;
    }
    else
    {
      gameObject.SetActive(false);
    }
  }
}

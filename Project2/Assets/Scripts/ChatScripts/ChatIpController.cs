/**
 * This will get the ip from the network controller and display it to a text box
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatIpController : NetworkBehaviour
{
  public Text IpTextBox;

  private CustomNetworkControl myNetworkControl;

  private void Start()
  {
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
      gameObject.SetActive(true);
      IpTextBox.text += myNetworkControl.networkAddress;
    }
    else
    {
      gameObject.SetActive(false);
    }
  }
}

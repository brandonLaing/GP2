using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextIpAdress : MonoBehaviour {

  private CustomNetworkControl myNetworkController;

  private void Start()
  {
    try
    {
      myNetworkController = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<CustomNetworkControl>();
    }
    catch
    {
      Debug.LogError("Couldn't find a networkController");
    }
  }

  public void AssignPlayerIp()
  {
    if (myNetworkController != null)
    {
      myNetworkController.UpdateIpFromInputBox(GetComponent<UnityEngine.UI.InputField>().text);
    }
    else
    {
      Debug.LogError("Tried to assign player name to non existent networkController");
    }
  }
}

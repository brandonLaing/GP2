using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum MyMaterials
{
  red, blue, black, green, purple, pink, numOfTypes
}
public class PlayerMatUpdate : NetworkBehaviour {

  [SyncVar]
  public MyMaterials myMat;
  public Material[] myMaterials;

  [HideInInspector]
  public int currentMat = -1;

  private MeshRenderer myMesh;

  private void Start()
  {
    myMesh = GetComponent<MeshRenderer>();
    AssignNewColor();
  }

  private void AssignNewColor()
  {
    if (isLocalPlayer)
    {
      int numberOfPlayers = 0;

      foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
      {
        if (player != null && player != this.gameObject)
        {
          numberOfPlayers++;
        }
      }

      if (numberOfPlayers >= (int)MyMaterials.numOfTypes)
      {
        numberOfPlayers = (numberOfPlayers % (int)MyMaterials.numOfTypes);
      }

      Debug.LogWarning(numberOfPlayers);

      myMat = (MyMaterials)(numberOfPlayers);

    }
  }

  void Update () {
    UpdateCharecterMaterial();

  }

  private void UpdateCharecterMaterial()
  {
    myMesh.material = myMaterials[(int)myMat];
    currentMat = (int)myMat;
  }

  public string GetColorName()
  {
    return myMat.ToString();
  }

}

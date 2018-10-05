using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireControlsScript : NetworkBehaviour {

  public Transform firePoint;
  public GameObject projectile;
  public float launchForce = 100;

  private void Update()
  {
    if (Input.GetMouseButtonDown(1))
    {
      CmdFire();
    }
  }

  [Command]
  private void CmdFire()
  {
    GameObject newProjectile = GameObject.Instantiate(projectile, firePoint.position, firePoint.rotation);
    newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * launchForce);

    NetworkServer.Spawn(newProjectile);
  }

  [ClientRpc]
  private void RpcShowHit()
  {
    Debug.Log("I got hit");
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (!isServer) return;

    if (collision.gameObject.tag == "Projectile")
    {
      RpcShowHit();
    }
  }


}

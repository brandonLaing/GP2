using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerScore))]
public class FireControlsScript : NetworkBehaviour {

  public Transform firePoint;
  public GameObject projectile;
  public float launchForce = 100;
  public PlayerScore myScore;
  public ParticleSystem hitEffect;

  private void Start()
  {
    myScore = GetComponent<PlayerScore>();
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(1))
    {
      CmdFire();
    }

    if (Input.GetKey(KeyCode.L))
    {
      Cursor.lockState = CursorLockMode.Locked;
    }
  }

  [Command]
  private void CmdFire()
  {
    if (myScore == null)
    {
      myScore = GetComponent<PlayerScore>();
    }
    GameObject newProjectile = GameObject.Instantiate(projectile, firePoint.position, firePoint.rotation);
    newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * launchForce);
    newProjectile.GetComponent<PuffballController>().owningPlayer = myScore;

    NetworkServer.Spawn(newProjectile);
  }

  [ClientRpc]
  private void RpcShowHit()
  {
    if (isLocalPlayer)
    {
      Debug.Log("I got hit");
    }
  }

  [ClientRpc]
  private void RpcDisplayHitEffect(Vector3 hitPosition)
  {
    Debug.Log("Spawing Effect");

    GameObject newHitIndecator = GameObject.Instantiate(hitEffect.gameObject, hitPosition, Quaternion.identity);
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (!isServer) return;

    Debug.Log("Hit");

    if (collision.gameObject.tag == "Projectile")
    {
      RpcShowHit();
      RpcDisplayHitEffect(collision.contacts[0].point);
      collision.gameObject.GetComponent<PuffballController>().owningPlayer.AddPoint(1);
    }
  }
}

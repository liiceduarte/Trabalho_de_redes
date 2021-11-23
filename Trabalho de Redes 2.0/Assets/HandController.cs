using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Collider handCollider;
    public Player owner;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            PhotonView target = other.GetComponent<PhotonView>();
            if(target.Owner != owner){
                other.gameObject.GetComponent<AttackHandler>().TakeHit();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DoDamageOnCollision : MonoBehaviour
{
    [SerializeField] float force = 10;
    [SerializeField] bool applyForce = false;
    [SerializeField] bool doHit = true;

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PhotonView>().IsMine){
            if(doHit)
                other.gameObject.GetComponent<AttackHandler>().TakeHit();

            if(applyForce){
                other.gameObject.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * force, ForceMode.Impulse);
            }
        }
    }
}

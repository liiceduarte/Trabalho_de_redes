using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CrownController : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Rigidbody ballRigidbody;
    [SerializeField] private Collider ballCollider;
    [SerializeField] float dropForce = 3;
    private bool isCollected = false;

    private void Update() {
        if(isCollected && target != null && PhotonNetwork.IsMasterClient){
            transform.position = target.transform.position + offset;
        }
    }

    private void OnCollisionEnter(Collision other) {
        PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
        if(photonView.IsMine){ 
            if(!isCollected ){
                isCollected = true;
                target = other.transform;
                this.GetComponent<PhotonView>().RPC("PlayerCollected",RpcTarget.All, photonView.ViewID);
            }
        }
    }

    public void DropBall(){
        this.GetComponent<PhotonView>().RPC("BallDroped",RpcTarget.All);
    }

    [PunRPC]
    void PlayerCollected(int pvID){
        PhotonView[] obs = FindObjectsOfType<PhotonView>();
        PhotonView targetPlayer = null;
        foreach(PhotonView p in obs){
            if(p.ViewID == pvID){
                target = p.gameObject.transform;
                targetPlayer = p;
            }
        }
        if(PhotonNetwork.IsMasterClient){
            if(targetPlayer != null)
                GameController.instance.PlayerHasTheBall(targetPlayer.Owner.ActorNumber);
        }
        ballCollider.enabled = false;
        ballRigidbody.isKinematic = true;
    }

    [PunRPC]
    void BallDroped(){
        target = null;
        ballRigidbody.isKinematic = false;
        ballCollider.enabled = true;
        if(PhotonNetwork.IsMasterClient){
            ballRigidbody.AddForce(new Vector3(Random.Range(-1,1), Random.Range(-1,1), Random.Range(-1,1)).normalized * dropForce, ForceMode.Impulse);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CrownController : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Rigidbody ballRigidbody;
    [SerializeField] private Collider ballCollider;
    [SerializeField] float dropForce = 3;
    private bool isCollected = false;
    public static CrownController instance;
    [SerializeField] float uncollectableDelay = 2;
    private float uncollectableTimer = 2;

    private void Awake() {
        instance = this;
    }
    private void Update() {
        if(isCollected && target != null && PhotonNetwork.IsMasterClient){
            transform.position = target.transform.position + offset;
        }
        uncollectableTimer -= Time.deltaTime;
        if(uncollectableTimer <= 0){
            uncollectableTimer = 0;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(!other.gameObject.CompareTag("Player")) return;
        if(uncollectableTimer > 0) return;
        
        PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
        if(photonView.IsMine){ 
            if(!isCollected ){
                isCollected = true;
                target = other.transform;
                this.GetComponent<PhotonView>().RPC("PlayerCollected",RpcTarget.All, photonView.Owner);
            }
        }
    }

    public void DropBall(){
        this.GetComponent<PhotonView>().RPC("BallDroped",RpcTarget.All);
    }

    [PunRPC]
    void PlayerCollected(Player owner){
        PhotonView[] obs = FindObjectsOfType<PhotonView>();
        PhotonView targetPlayer = null;
        foreach(PhotonView p in obs){
            if(p.Owner == owner && p.gameObject.CompareTag("Player")){
                target = p.gameObject.transform;
                targetPlayer = p;
            }
        }
        if(PhotonNetwork.IsMasterClient){
            if(targetPlayer != null)
                GameController.instance.PlayerHasTheBall(targetPlayer.Owner.ActorNumber);
        }
        isCollected = true;
        ballCollider.enabled = false;
        ballRigidbody.isKinematic = true;
    }

    [PunRPC]
    void BallDroped(){
        target = null;
        ballRigidbody.isKinematic = false;
        ballCollider.enabled = true;
        isCollected = false;
        uncollectableTimer = uncollectableDelay;
        
        if(PhotonNetwork.IsMasterClient){
            ballRigidbody.AddForce(new Vector3(Random.Range(-1,1), Random.Range(-1,1), Random.Range(-1,1)).normalized * dropForce, ForceMode.Impulse);
        }
    }
}

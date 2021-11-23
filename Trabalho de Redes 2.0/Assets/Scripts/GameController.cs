using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviour
{
    private int[] playerPoints;
    private int playerWithBall = -1;
    public static GameController instance;
    [SerializeField] private float delayBetweenPoints = 2;
    private float pointTimer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        playerPoints = new int[PhotonNetwork.CurrentRoom.PlayerCount];
        if(PhotonNetwork.IsMasterClient){
            this.GetComponent<PhotonView>().RPC("RPCUpdatePoints",RpcTarget.All, playerPoints);
        }
    }

    private void Update() {
        if(playerPoints.Length < PhotonNetwork.CurrentRoom.PlayerCount){
            playerPoints = new int[PhotonNetwork.CurrentRoom.PlayerCount];
        }
        if(!PhotonNetwork.IsMasterClient) return;
        
        if(playerWithBall >= 0){
            pointTimer += Time.deltaTime;

            if(pointTimer >= delayBetweenPoints){
                playerPoints[playerWithBall-1]++;
                this.GetComponent<PhotonView>().RPC("RPCUpdatePoints",RpcTarget.Others, playerPoints);
                pointTimer -= delayBetweenPoints;
            }
        }
    }

    public int PlayerWithBall(){
        return playerWithBall;
    }

    public void PlayerHasTheBall(int playerId){
        playerWithBall = playerId;
        pointTimer = 0;
    }

    public void BallWasDroped(){
        this.GetComponent<PhotonView>().RPC("BallDroped",RpcTarget.All);
    }

    [PunRPC]
    void RPCUpdatePoints(int[] playerPoints){
        if(!PhotonNetwork.IsMasterClient){
            this.playerPoints = playerPoints;
        }
    }

    [PunRPC]
    void UpdatePlayerWithBall(int id){
        playerWithBall = id;
    }

    [PunRPC]
    void BallDroped(){
        playerWithBall = -1;
        pointTimer = 0;
    }
}

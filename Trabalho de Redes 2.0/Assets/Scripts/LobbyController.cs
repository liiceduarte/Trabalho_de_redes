using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyController : MonoBehaviour
{
    private int playersReady = 0;
    private bool roomReady = false;
    [SerializeField] private float timeUntilBegin = 5;
    private float beginTimer = 0;
    private bool isLoadingScene = false;
    private void OnTriggerEnter(Collider other) {
        if(PhotonNetwork.IsMasterClient){ // somente no servidor é feita a verificação
            if(other.gameObject.CompareTag("Player")){
                playersReady++;
                CheckPlayersReady();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(PhotonNetwork.IsMasterClient){ // somente no servidor é feita a verificação
            if(other.gameObject.CompareTag("Player")){
                playersReady--;
                CheckPlayersReady();
            }
        }
    }

    private void CheckPlayersReady(){
        roomReady = playersReady == PhotonNetwork.CurrentRoom.PlayerCount;
        beginTimer = timeUntilBegin;
    }
    
    private void Update() {
        if(PhotonNetwork.IsMasterClient){
             if(roomReady){
                 beginTimer -= Time.deltaTime;

                 if(beginTimer <= 0){
                     StartGame();
                 }
             }
        }
    }

    private void StartGame(){
        if(PhotonNetwork.IsMasterClient && !isLoadingScene){
            PhotonNetwork.LoadLevel(2);
            isLoadingScene = true;
        }
    }
}

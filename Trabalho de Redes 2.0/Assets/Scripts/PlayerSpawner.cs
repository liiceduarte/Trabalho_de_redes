using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] PhotonView photonView;
    [SerializeField] CinemachineVirtualCamera cin_vitualCamera;
    [SerializeField] GameObject defaultWeapon;
    private string avatarName;
    private GameObject currentPlayer = null;

    private void Awake() {
        avatarName = NetworkController.instance.playerPrefabs[NetworkController.instance.selectedPrefabID].name;
        if(avatarName == ""){
            avatarName = "PlayerArmature";
        }
        SpawnPlayer();
    }

    public void SpawnPlayer(){
        //if(currentPlayer != null) DestroyPlayer();

        currentPlayer = PhotonNetwork.Instantiate(avatarName ,transform.position,  Quaternion.identity);
        cin_vitualCamera.Follow = currentPlayer.transform.GetChild(0).transform;
        photonView = currentPlayer.GetComponent<PhotonView>();
    }

    public void DestroyPlayer(){
        currentPlayer = null;
        PhotonNetwork.Destroy(photonView);
    }

    public void ChangePlayerAvatar(string avatarName){
        this.avatarName = avatarName;
        SpawnPlayer();
    }
}

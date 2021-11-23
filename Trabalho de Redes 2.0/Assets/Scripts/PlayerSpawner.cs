using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] PhotonView photonView;
    [SerializeField] CinemachineVirtualCamera cin_vitualCamera;
    [SerializeField] int defaultWeapon = 0;
    private int currentWeapon = 0;
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
        currentPlayer.GetComponent<AttackHandler>().UpdateWeapon(NetworkController.instance.weaponId);
        cin_vitualCamera.Follow = currentPlayer.transform.GetChild(0).transform;
        photonView = currentPlayer.GetComponent<PhotonView>();
    }

    public void DestroyPlayer(){
        currentPlayer = null;
        PhotonNetwork.Destroy(photonView);
    }

    public void UpdateWeapon(){
        currentPlayer.GetComponent<AttackHandler>().UpdateWeapon(NetworkController.instance.weaponId);
    }

    public void ChangePlayerAvatar(string avatarName){
        this.avatarName = avatarName;
        SpawnPlayer();
    }
}

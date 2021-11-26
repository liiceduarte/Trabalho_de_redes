using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SkinCollectable : MonoBehaviour
{
    public int skinId = 0;
    [SerializeField] PlayerSpawner spawner;
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine){
            NetworkController.instance.skinId = skinId;
            spawner.UpdateSkin();
        }
    }
}

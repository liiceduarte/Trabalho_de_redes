using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class HatCollectable : MonoBehaviour
{
    [SerializeField] int hatId = 0;
    [SerializeField] PlayerSpawner spawner;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine){
            NetworkController.instance.hatId = hatId;
            spawner.UpdateHat();
        }
    }
}

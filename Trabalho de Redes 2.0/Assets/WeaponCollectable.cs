using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WeaponCollectable : MonoBehaviour
{
    [SerializeField] int weaponId = 0;
    [SerializeField] PlayerSpawner spawner;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine){
            NetworkController.instance.weaponId = weaponId;
            spawner.UpdateWeapon();
        }
    }
    
}

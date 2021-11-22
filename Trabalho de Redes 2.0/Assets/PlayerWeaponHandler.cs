using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] Transform weaponHolder;
    [SerializeField] GameObject currentWeapon;
    public void SpawnWeapon(string weaponName){
        currentWeapon = PhotonNetwork.Instantiate(weaponName, weaponHolder.position, weaponHolder.rotation);
        currentWeapon.transform.parent = weaponHolder;
    }
}

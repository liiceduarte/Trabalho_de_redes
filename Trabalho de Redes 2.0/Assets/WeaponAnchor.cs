using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class WeaponAnchor : MonoBehaviour
{
    public Transform handTransform;
    [SerializeField] HandController handController;

    public void Hit(){
        StartCoroutine("DisableWeapon");
    }

    IEnumerator DisableWeapon(){
        yield return new WaitForSeconds(0.5f);
        handController.handCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        handController.handCollider.enabled = false;
    }

    public void SetOwner(Player owner){
        handController.owner = owner;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float attackDelay;
    [SerializeField] WeaponAnchor weaponAnchor;
    float attackTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<PhotonView>().IsMine)
            weaponAnchor.SetOwner(GetComponent<PhotonView>().Owner);
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        if(attackTimer < 0)
            attackTimer = 0;
    }

    public void OnAttack(InputValue iv){
        if(iv.isPressed){
            Debug.Log("attacou");
            if(attackTimer == 0){
                animator.SetLayerWeight(animator.GetLayerIndex("MoveArms"), 1);
                animator.SetTrigger("AttackTrigger");
                weaponAnchor.Hit();
                StartCoroutine("Attack");
                attackTimer = attackDelay;
            }
        }
    }

    public void TakeHit(){
        this.GetComponent<PhotonView>().RPC("PlayerHit",RpcTarget.All, this.GetComponent<PhotonView>().Owner.ActorNumber);
    }

    public void SpawnWeapon(GameObject prefab){
        WeaponAnchor anchor = GetComponentInChildren<WeaponAnchor>();
        GameObject weapon = Instantiate(prefab, anchor.transform);
        weapon.transform.localEulerAngles = Vector3.zero;
    }

    public void UpdateWeapon(int weaponId){
        this.GetComponent<PhotonView>().RPC("SetWeapon",RpcTarget.All, weaponId, this.GetComponent<PhotonView>().Owner);
    }

    IEnumerator Attack(){
        yield return new WaitForSeconds(2);
        animator.SetLayerWeight(animator.GetLayerIndex("MoveArms"), 0);
        animator.ResetTrigger("AttackTrigger");
    }

    [PunRPC]
    public void SetWeapon(int weaponId, Player owner){
        if(this.GetComponent<PhotonView>().Owner == owner){
            foreach(Transform c in weaponAnchor.handTransform){
                c.gameObject.SetActive(false);
            }
            weaponAnchor.handTransform.GetChild(weaponId).gameObject.SetActive(true);
        }
    }

    [PunRPC]
    public void PlayerHit(int playerId){
        if(playerId == GameController.instance.PlayerWithBall()){
            CrownController.instance.DropBall();
        }
    }
}

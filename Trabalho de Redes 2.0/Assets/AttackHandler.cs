using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float attackDelay;
    float attackTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
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
                attackTimer = attackDelay;
            }
        }
    }

    IEnumerator DisableArms(){
        yield return new WaitForSeconds(2);
        animator.SetLayerWeight(animator.GetLayerIndex("MoveArms"), 0);
        animator.ResetTrigger("AttackTrigger");
    }
}

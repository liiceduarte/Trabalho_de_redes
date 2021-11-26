using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOffset : MonoBehaviour
{
    [SerializeField] Animator target;
    [SerializeField] float offset;
   
    private void Awake() {
        target.Play("Hammer", 0, offset);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoor : MonoBehaviour
{
public Animator animator;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            animator.Play("Open");
        }
    }

     private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            animator.Play("Close");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    [SerializeField] private Animator targetDummy;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet")|| other.gameObject.CompareTag("Weapon"))
        {
            targetDummy.SetTrigger("Death");
            Debug.Log("I've been hit");
        }
    }

    public void ActivateDummy ()
    {
        targetDummy.SetTrigger("Activate");
    }
}

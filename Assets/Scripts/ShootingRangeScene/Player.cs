using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] Transform head;

    public void TakeDamage(float damage)
    {
        health -= damage;
        print($"Player health = {health}");
    }

    public Vector3 GetHeadPosition() { return head.position; }
}

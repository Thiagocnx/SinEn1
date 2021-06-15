using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] Transform head;
    public static float life;

    public bool contact = false;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public Vector3 GetHeadPosition()
    {
        return head.position;
    }
    private void Update()
    {
        life = health;
        //contact = true;
    }
}


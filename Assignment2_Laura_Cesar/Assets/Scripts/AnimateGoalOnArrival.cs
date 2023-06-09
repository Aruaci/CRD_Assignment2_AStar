using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateGoalOnArrival : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("arrivvvaaaa");
            MeshFilter.Destroy(this);
        }
    }
}

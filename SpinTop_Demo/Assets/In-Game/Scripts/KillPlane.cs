using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Kill " + other.name);
        SpinTopPhysics st = other.transform.parent.GetComponent<SpinTopPhysics>();
        st?.Kill();
    }
}

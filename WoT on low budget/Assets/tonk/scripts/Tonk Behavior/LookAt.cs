using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    int layerMask = ~0;
    public RaycastHit hit;
    void FixedUpdate()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask);
        Debug.DrawRay(transform.position,transform.forward*1000,Color.red);
    }
}

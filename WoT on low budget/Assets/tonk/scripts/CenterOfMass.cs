using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 _CenterOfMass;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.centerOfMass=_CenterOfMass;
    }
    private void OnDrawGizmos(){
        Gizmos.color=Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * _CenterOfMass ,0.1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{

    [SerializeField] private GameObject Target;
    void Start(){    
        Invoke("tp",1f);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            tp();
            //Target.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    void tp(){
        Target.transform.localPosition = transform.localPosition;
        Target.transform.localRotation = transform.localRotation;
    }
}

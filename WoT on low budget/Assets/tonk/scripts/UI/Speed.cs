using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Speed : MonoBehaviour
{
    Text t;
    public Rigidbody rb;
    void Start(){
        t = GetComponent<Text>();
    }
    void Update()
    {
        t.text = ((int)(rb.velocity.magnitude*3.6f)).ToString("0") + "Km/h";
    }
}

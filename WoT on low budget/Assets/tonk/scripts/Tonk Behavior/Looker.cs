using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{
    public LookAt Dot;
    public TowerNGun TnG;
    void FixedUpdate()
    {  
        transform.LookAt(Dot.hit.point,Vector3.up);
        Debug.DrawRay(transform.position,transform.forward*1000,Color.red);
        if(Dot.hit.point!=Vector3.zero){
            TnG.XrotSet=transform.localEulerAngles.x;
            TnG.ZrotSet=transform.localEulerAngles.y;
        }
        //Debug.Log(Dot.hit.point);
    }
}

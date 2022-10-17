using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerNGun : MonoBehaviour
{
    [SerializeField]
    Transform T,G;

    [Range (0,360)]
    public float ZrotSet;

    [Range (0,360)]
    public float XrotSet;

    [SerializeField]
    float RotSpeed,DepressionSpeed;
    float Tpass,Gpass;
    bool ClosestPass;
    void Update()
    {
        //Turret lock
        if((Input.GetKey(KeyCode.Mouse1))) return;
        //Tower Rotation
        if(ZrotSet==360) ZrotSet=0;
        Tpass = T.localEulerAngles.y - ZrotSet;
        if(Mathf.Abs(Tpass)>180)
            ClosestPass=false;
        else
            ClosestPass=true;
        if(Mathf.Abs(Tpass) < RotSpeed * Time.deltaTime)
            T.localRotation = Quaternion.Euler(-90,0,ZrotSet);
        else if(Tpass > 0)
            T.Rotate(Vector3.forward * (ClosestPass ? -RotSpeed : RotSpeed) * Time.deltaTime, Space.Self);
        else if(Tpass < 0)
            T.Rotate(Vector3.forward * (ClosestPass ? RotSpeed : -RotSpeed) * Time.deltaTime, Space.Self);

        //Gun Depression
        if(XrotSet>7 && XrotSet<180) XrotSet=7;
        if(XrotSet<345 && XrotSet>180) XrotSet=345;
        if(XrotSet==360) XrotSet=0;
        Gpass = G.localEulerAngles.x - XrotSet;
        if(Mathf.Abs(Gpass)>180)
            ClosestPass=false;
        else
            ClosestPass=true;
        if(Mathf.Abs(Gpass) < DepressionSpeed * Time.deltaTime)
            G.localRotation = Quaternion.Euler(XrotSet,0,0);
        else if(Gpass > 0)
            G.Rotate(Vector3.right * (ClosestPass ? -DepressionSpeed : DepressionSpeed) * Time.deltaTime, Space.Self);
        else if(Gpass < 0)
            G.Rotate(Vector3.right * (ClosestPass ? DepressionSpeed : -DepressionSpeed) * Time.deltaTime, Space.Self);
    }
}

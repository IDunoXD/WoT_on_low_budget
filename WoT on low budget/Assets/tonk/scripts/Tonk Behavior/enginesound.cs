using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enginesound : MonoBehaviour
{
    // Start is called before the first frame update
    public RearWheelDrive driver;
    public AudioSource idle,loop,start,off;
    public float afktime=6;
    float time,maxTorque;
    bool flip=true;
    float v1,v2;
    void Start()
    {
        v1=idle.volume;
        v2=loop.volume;
        time=afktime;
        maxTorque=driver.maxTorque;
    }

    // Update is called once per frame
    void Update()
    {
        idle.volume=v1*Mathf.InverseLerp(driver.maxTorque,0,Mathf.Abs(driver.torgue));
        loop.volume=v2*Mathf.InverseLerp(0,driver.maxTorque,Mathf.Abs(driver.torgue));
        if(Mathf.Abs(driver.torgue)<=0)
            time-=Time.deltaTime;
        else
            time=afktime;
        if(Input.GetKeyDown(KeyCode.E)&&!flip){
            idle.enabled=true;
            loop.enabled=true;
            start.PlayOneShot(start.clip);
            time=afktime;
            Invoke("StartEngine",0.7f);
            flip=true;
        }else if((Input.GetKeyDown(KeyCode.E)&&flip)||(time<=0 && flip)){
            idle.enabled=false;
            loop.enabled=false;
            off.PlayOneShot(off.clip);
            driver.maxTorque=0;
            flip=false;
        }
    }
    void StartEngine(){
        driver.maxTorque=maxTorque;
    }
}

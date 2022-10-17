using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BigRookGames.Weapons;
using UnityEngine.UI;

public class CoolDown : MonoBehaviour
{
    public GunfireController cd;
    Text t;
    float xd;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {   
        xd=cd.timeLastFired + cd.cooldown - Time.time;
        if(xd<0){
            t.color=Color.green;
            t.text = "4.00";
        }else{
            t.color=Color.red;
            t.text = xd.ToString("0.00");
        }
    }
}

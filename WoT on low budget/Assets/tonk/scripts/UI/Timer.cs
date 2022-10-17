using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text t;
    public Score s;
    bool Finish=false;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(s.GetScore() != 0)
            t.text = ((int)Time.time/60).ToString("00") + ":" + ((int)Time.time%60).ToString("00") + ":" + ((int)((Time.time%1)*60)).ToString("00");
        else if(!Finish){
            t.text = "Finish time " + t.text;
            Finish=true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Score s;
    void OnDestroy(){
        if(s) s.ScoreUpdate();
    }
}

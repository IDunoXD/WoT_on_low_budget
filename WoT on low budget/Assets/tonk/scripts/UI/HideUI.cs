using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    bool flip=true;
    public GameObject ui;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)&&!flip){
            ui.SetActive(true);
            flip=true;
        }else if(Input.GetKeyDown(KeyCode.V)&&flip){
            ui.SetActive(false);
            flip=false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class KeyListener : MonoBehaviour
{
    public string KeyName;
    Button b;
    void Start()
    {
        b = GetComponent<Button>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyName))
            b.interactable=false;
        if(Input.GetKeyUp(KeyName))
            b.interactable=true;
    }
}

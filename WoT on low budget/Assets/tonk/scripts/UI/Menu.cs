using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    bool flip=true;
    public GameObject menu;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)&&!flip){
            menu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            flip=true;
        }else if(Input.GetKeyDown(KeyCode.Escape)&&flip){
            menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            flip=false;
        }
    }
}

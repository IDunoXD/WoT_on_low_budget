using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Quit : MonoBehaviour
{
    Button b;
    void Start()
    {
        b = GetComponent<Button>();
        b.onClick.AddListener(Click);
    }
    void Click(){
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    int score = 15;
    Text t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        t.text = "Targets: " + score.ToString() + "/15 left";
    }

    // Update is called once per frame
    public void ScoreUpdate()
    {
        score--;
        t.text = "Targets: " + score.ToString() + "/15 left";
    }
    public int GetScore(){
        return score;
    }
}

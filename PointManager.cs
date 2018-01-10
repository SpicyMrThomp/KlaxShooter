using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour {
    static public PointManager instance;
    protected float score = 0;

    private void Awake()
    {
        if(instance != this)
        {
            instance = this;
        }
    }

    public float getScore()
    {
        return score;
    }

    public void setScore(float s)
    {
        score += s;
        if (score <= 0)
            score = 0;
        CanvasManager.instance.UpdateScore(score);
    }
}

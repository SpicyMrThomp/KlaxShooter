using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    Text score, health, wave, remaining;
    static public CanvasManager instance;

    private void Awake()
    {
        if(instance != this)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start ()
    {
        if (GameObject.FindGameObjectWithTag("Canvas Score")!= null)
        {
            score = GameObject.FindGameObjectWithTag("Canvas Score").GetComponent<Text>();
        }
        if (GameObject.FindGameObjectWithTag("Canvas Health") != null)
        {
            health = GameObject.FindGameObjectWithTag("Canvas Health").GetComponent<Text>();
        }
        if (GameObject.FindGameObjectWithTag("Canvas Wave") != null)
        {
            wave = GameObject.FindGameObjectWithTag("Canvas Wave").GetComponent<Text>();
        }
        if (GameObject.FindGameObjectWithTag("Canvas Remaining") != null)
        {
            remaining = GameObject.FindGameObjectWithTag("Canvas Remaining").GetComponent<Text>();
        }
    }

    public void UpdateScore(float s)
    {
        if(score != null)
            score.text = "Score: " + s.ToString();
    }

    public void UpdateHealth(float h)
    {
        if(health != null)
            health.text = "Health: " + h.ToString();
    }

    public void UpdateWave(float w)
    {
        if (wave != null)
            wave.text = "Wave: " + w.ToString();
    }

    public void UpdateRemaining(float r, float arr)
    {
        if (remaining != null)
            remaining.text = "Matches: " + r.ToString() + "/" + arr.ToString();
    }
}

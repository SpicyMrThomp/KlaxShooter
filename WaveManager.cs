using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    public static WaveManager instance;
    int currentWave = 1;
    List<float> colorMatchRequirements;
    float colorMatchRequirement;
    float colorMatchRemaining;

	// Use this for initialization
	void Start ()
    {
        colorMatchRequirements = new List<float>();
        NewWave(currentWave);
        instance = this;
	}

    public void UpdateWave(int i)
    {
        if(currentWave < currentWave + i)
        {
            currentWave += i;
            NewWave(currentWave);
        }
    }

    void NewWave(int wave)
    {
        colorMatchRequirement = wave * 2f;
        colorMatchRemaining = colorMatchRequirement;

        if(EnemyManager.instance != null)
        {
            foreach (GameObject enemy in EnemyManager.instance.GetEnemyList())
            {
                if (enemy.activeInHierarchy)
                {
                    enemy.SetActive(false);
                }
            }
        }

        Debug.Log("Wave: " + currentWave);
        CanvasManager.instance.UpdateWave(currentWave);
        Debug.Log("Matches Remaining: " + colorMatchRemaining);
        CanvasManager.instance.UpdateRemaining(colorMatchRequirement - colorMatchRemaining, colorMatchRequirement);
    }

    public void SetColorMatch(int i = 0, float j = 0)
    {
        colorMatchRemaining -= j;
        if (colorMatchRemaining <= 0)
        {
            UpdateWave(1);
        }
        Debug.Log("Matches Remaining: " + colorMatchRemaining);
        CanvasManager.instance.UpdateRemaining(colorMatchRequirement - colorMatchRemaining, colorMatchRequirement);
    }

    public int GetWave()
    {
        return currentWave;
    }
}

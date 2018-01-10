using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    List<GridNode> topRowGridNodes = new List<GridNode>();
    List<GridNode> middleTopRowGridNodes = new List<GridNode>();
    List<GridNode> middleBottomRowGridNodes = new List<GridNode>();
    List<GridNode> bottomRowGridNodes = new List<GridNode>();

    List<List<GridNode>> gridNodeListList = new List<List<GridNode>>();
    GameObject gridPointer;
    int selectedIndex = 0;
    ParticleSystem ps;

    public AudioClip scoreSound;

    // Use this for initialization
    void Start ()
    {
        PopulateGrid();
        ps = gridPointer.GetComponent<ParticleSystem>();
        instance = this;
	}

    void PopulateGrid()
    {
        for(int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if(i < 4)
            {
                topRowGridNodes.Add(new GridNode(Color.black, this.gameObject.transform.GetChild(i).gameObject, false));
            }
            else if (i < 8)
            {
                middleTopRowGridNodes.Add(new GridNode(Color.grey, this.gameObject.transform.GetChild(i).gameObject, false));
            }
            else if (i < 12)
            {
                middleBottomRowGridNodes.Add(new GridNode(Color.black, this.gameObject.transform.GetChild(i).gameObject, false));
            }
            else if (i < 16)
            {
                bottomRowGridNodes.Add(new GridNode(Color.grey, this.gameObject.transform.GetChild(i).gameObject, false));
            }
        }

        gridNodeListList.Add(topRowGridNodes);
        gridNodeListList.Add(middleTopRowGridNodes);
        gridNodeListList.Add(middleBottomRowGridNodes);
        gridNodeListList.Add(bottomRowGridNodes);

        gridPointer = GameObject.FindGameObjectWithTag("Grid Pointer");
    }

    public void InsertToGrid(Color c)
    {
        for(int i = gridNodeListList[selectedIndex].Count - 1; i >= 0; i--)
        {
            if(!gridNodeListList[selectedIndex][i].IsUsed())
            {
                gridNodeListList[selectedIndex][i].SetColor(c);
                gridNodeListList[selectedIndex][i].SetUsed(true);
                CheckMatch(gridNodeListList[selectedIndex], i);
                return;
            }
            else
            {
                if (i == 0)
                {
                    PushColor(gridNodeListList[selectedIndex], c);
                    CheckMatch(gridNodeListList[selectedIndex], i);
                    return;
                }
            }
        }
    }

    protected void PushColor(List<GridNode> list, Color c)
    {
        for(int i = list.Count - 1; i >= 0; i--)
        {
            if(i == 0)
            {
                list[i].SetColor(c);
                return;
            }
            else
            {
                list[i].SetColor(list[i - 1].GetColor());
            }
        }
    }

    protected void CheckMatch(List<GridNode> list, int index)
    {
        bool hMatch = false, vMatch = false;

        hMatch = CheckHorizontal(list);
        vMatch = CheckVertical(index);


        if (hMatch)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetColor(Color.clear);
                list[i].SetUsed(false);
                PointManager.instance.setScore(1);
            }
        }

        if(vMatch)
        {
            for (int i = 0; i < gridNodeListList.Count; i++)
            {
                gridNodeListList[i][index].SetColor(Color.clear);
                gridNodeListList[i][index].SetUsed(false);
                PointManager.instance.setScore(1);
            }
        }

        if(hMatch || vMatch)
        {
            ps.Play();
            SoundManager.instance.PlayGridSound(scoreSound);
            WaveManager.instance.SetColorMatch(ColorManager.instance.GetSelectedIndex(), 1);
        }
    }

    protected bool CheckHorizontal(List<GridNode> list)
    {
        Color tempColor = Color.black;
        bool match = false;
        for(int i = 0; i < list.Count; i++)
        {
            if (i == 0)
                tempColor = list[i].GetColor();
            else
            {
                if (tempColor == list[i].GetColor())
                    match = true;
                else
                {
                    match = false;
                    break;
                }
                tempColor = list[i].GetColor();
            }    
        }

        return match;
    }

    protected bool CheckVertical(int index)
    {
        Color tempColor = Color.black;
        bool match = false;

        for(int i = 0; i < gridNodeListList.Count; i++)
        {
            if (i == 0)
                tempColor = gridNodeListList[i][index].GetColor();
            else
            {
                if (tempColor == gridNodeListList[i][index].GetColor())
                    match = true;
                else
                {
                    match = false;
                    break;
                }
                tempColor = gridNodeListList[i][index].GetColor();
            }
        }

        return match;
    }

    public void SetSelectedRow(int i)
    {
        selectedIndex += i;

        if(selectedIndex > gridNodeListList.Count - 1)
        {
            selectedIndex = 0;
        }
        else if(selectedIndex < 0)
        {
            selectedIndex = gridNodeListList.Count - 1;
        }

        gridPointer.GetComponent<RectTransform>().anchoredPosition = new Vector2(gridPointer.GetComponent<RectTransform>().anchoredPosition.x, (100f * (((float)selectedIndex + 1f) / (float)gridNodeListList.Count)) * -1);
    }
}

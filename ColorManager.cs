using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    List<GridNode> colorNodes = new List<GridNode>();
    List<Color> colors = new List<Color>();
    
    GameObject gridPointer;
    int selectedIndex = 0;

    // Use this for initialization
    void Start()
    {
        PopulateColorList();
        PopulateGrid();
        instance = this;
    }

    void PopulateColorList()
    {
        colors.Add(Color.magenta);
        colors.Add(Color.cyan);
        colors.Add(Color.red);
        colors.Add(Color.yellow);
        colors.Add(Color.blue);
    }

    public List<Color> GetColorList()
    {
        return colors;
    }

    void PopulateGrid()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            colorNodes.Add(new GridNode(colors[i], this.gameObject.transform.GetChild(i).gameObject, false));
        }

        gridPointer = GameObject.FindGameObjectWithTag("Color Pointer");
    }

    public void SetSelectedColor(int i)
    {
        selectedIndex += i;

        if (selectedIndex > colorNodes.Count - 1)
        {
            selectedIndex = 0;
        }
        else if (selectedIndex < 0)
        {
            selectedIndex = colorNodes.Count - 1;
        }

        float fudge = 25f;
        gridPointer.GetComponent<RectTransform>().anchoredPosition = new Vector2(colorNodes[selectedIndex].GetObject().GetComponent<RectTransform>().anchoredPosition.x + fudge, gridPointer.GetComponent<RectTransform>().anchoredPosition.y);
    }

    public Color GetSelectedColor()
    {
        return colorNodes[selectedIndex].GetColor();
    }

    public int GetSelectedIndex()
    {
        return selectedIndex;
    }
}

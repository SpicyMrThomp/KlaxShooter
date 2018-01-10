using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GridNode
{
    protected GameObject obj;
    protected Color objColor;
    protected bool used = false;

    public GridNode(Color c, GameObject o = null, bool u = false)
    {
        SetObject(o);
        SetColor(c);
        SetUsed(u);
    }

    public void SetObject(GameObject o)
    {
        obj = o;
    }

    public void SetColor(Color c)
    {
        objColor = c;
        obj.GetComponent<Image>().color = objColor;
    }

    public void SetUsed(bool u)
    {
        used = u;
    }

    public GameObject GetObject()
    {
        return obj;
    }

    public Color GetColor()
    {
        return objColor;
    }

    public bool IsUsed()
    {
        return used;
    }
}

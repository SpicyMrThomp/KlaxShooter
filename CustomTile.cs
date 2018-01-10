using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTile : MonoBehaviour {
    public Texture texture;
    public bool one = true;
    public Color color = Color.grey;

	// Use this for initialization
	void Start ()
    {
        float fuck = 0;
        this.gameObject.GetComponent<MeshRenderer>().material.mainTexture = texture;
        if(one)
            this.gameObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(this.transform.localScale.x, this.transform.localScale.y);
        else
        {
            fuck = (1 / SetNearest(this.transform.localScale.y));
            this.gameObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(fuck, 1);
        }

        this.gameObject.GetComponent<MeshRenderer>().material.color = color;
    }

    float SetNearest(float f)
    {
        float newF = f / Mathf.CeilToInt(f);
        if (Mathf.Abs(newF - 1) < Mathf.Abs(newF - .75f))
            return 1;
        else if (Mathf.Abs(newF - 1) > Mathf.Abs(newF - .75f) && Mathf.Abs(newF - .75f) < Mathf.Abs(newF - .5f))
            return .75f;
        else if (Mathf.Abs(newF - .75f) > Mathf.Abs(newF - .5f) && Mathf.Abs(newF - .5f) < Mathf.Abs(newF - .25f))
            return .5f;
        else
            return .25f;
    }
}

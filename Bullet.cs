using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    Color color;
    ParticleSystem ps;
    ParticleSystem.MainModule main;

	private void OnEnable()
    {
        SetColor(ColorManager.instance.GetSelectedColor());
        ps = this.gameObject.GetComponent<ParticleSystem>();
        main = ps.main;
        main.startColor = color;
    }

    public void SetColor(Color c)
    {
        color = c;
        this.gameObject.GetComponent<MeshRenderer>().material.color = color;
    }

    public Color GetColor()
    {
        return color;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            //if(col.gameObject.GetComponent<Enemy>())
            //{
            //    //Debug.Log(col.gameObject.GetComponent<Enemy>().GetHeadColor().ToString());
            //    Enemy enemy = col.gameObject.GetComponent<Enemy>();
            //    if (color == enemy.GetHeadColor())
            //        GridManager.instance.InsertToGrid(enemy.GetHeadColor());
            //    else
            //        enemy.KillEnemy(this.gameObject.GetComponent<Collision>());
            //}
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    protected float health = 3;

	protected float getHealth()
    {
        return health;
    }

    public void setHealth(float hP)
    {
        health += hP;
    }

    public virtual void KillCharacter()
    {
        Destroy(this.gameObject);
    }
}

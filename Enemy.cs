using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    Rigidbody rb;
    GameObject player;
    float speed = 1;
    List<GameObject> segments = new List<GameObject>();
    GameObject head;
    Color headColor;
    bool colorMatched = false;
    ParticleSystem ps;
    ParticleSystem.MainModule psMain;
    ParticleSystem.ColorOverLifetimeModule psColor;
    ParticleSystem.SizeOverLifetimeModule psSize;
    ParticleSystem.SizeOverLifetimeModule psSizeOG;
    ParticleSystem.MinMaxGradient gradient;
    public AudioClip deathSound;

	// Use this for initialization
	void Awake () {
        rb = this.gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (ps == null)
            ps = this.gameObject.GetComponent<ParticleSystem>();

        PopulateSegments();
	}

    private void OnEnable()
    {
        ps = this.gameObject.GetComponent<ParticleSystem>();
        psMain = ps.main;
        psColor = ps.colorOverLifetime;
        psColor.enabled = true;

        if (ps != null)
        {
            gradient = psColor.color;
            gradient.gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.yellow, 0f), new GradientColorKey(new Color(255, 185, 0), 1)},
                                                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f) });

            psColor.color = new ParticleSystem.MinMaxGradient(gradient.gradient);
        }
    }

    // Update is called once per frame
    void Update ()
    {
		if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(!colorMatched)
        {
            if(headColor != head.gameObject.GetComponent<MeshRenderer>().material.color)
            {
                SetHeadColor(headColor);
                colorMatched = true;
            }
        }
	}

    private void FixedUpdate()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, player.transform.position, Time.fixedDeltaTime * speed);
        this.gameObject.transform.LookAt(player.transform);
    }

    void PopulateSegments()
    {
        Vector3 tempHeadPos = Vector3.zero;

        if(this.gameObject.transform.childCount == 1)
        {
            head = this.gameObject.transform.GetChild(0).gameObject;
            return;
        }
        else
        {
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                segments.Add(this.gameObject.transform.GetChild(i).gameObject);

                if (segments[i].transform.localPosition.x > tempHeadPos.x ||
                    segments[i].transform.localPosition.y > tempHeadPos.y ||
                    segments[i].transform.localPosition.z > tempHeadPos.z)
                {
                    tempHeadPos = segments[i].transform.localPosition;
                    head = segments[i];
                }
            }
        }
    }

    public void SetHeadColor(Color c)
    {
        headColor = c;
        head.gameObject.GetComponent<MeshRenderer>().material.color = headColor;
    }

    public Color GetHeadColor()
    {
        return headColor;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Bullet")
        {
            if (col.gameObject.GetComponent<Bullet>())
            {
                //Debug.Log(col.gameObject.GetComponent<Enemy>().GetHeadColor().ToString());
                Bullet bullet = col.gameObject.GetComponent<Bullet>();
                if (headColor == bullet.GetColor())
                {
                    GridManager.instance.InsertToGrid(headColor);
                    if (this.gameObject.activeInHierarchy)
                        StartCoroutine(RemoveEnemy(col));
                    else
                        return;
                }
                else
                    StartCoroutine(KillEnemy(col));
            }
        }
    }

    public IEnumerator RemoveEnemy(Collision col)
    {
        col.gameObject.SetActive(false);
        yield return new WaitForSeconds(0);
        this.gameObject.SetActive(false);
    }

    public IEnumerator KillEnemy(Collision col)
    {
        col.gameObject.SetActive(false);
        ps.Play();
        rb.velocity = Vector3.zero;

        float radius = 3f;
        Collider[] colList = Physics.OverlapSphere(this.gameObject.transform.position, radius);
        foreach (Collider coll in colList)
        {
            if(coll.gameObject != this.gameObject)
            {
                Rigidbody r = coll.gameObject.GetComponent<Rigidbody>();
                if(r != null)
                    r.AddExplosionForce(700, this.gameObject.transform.position, radius);
            }
        }

        SoundManager.instance.PlaySoundEffect(deathSound);
        yield return new WaitForSeconds(ps.main.duration);
        this.gameObject.SetActive(false);
        
    }
}

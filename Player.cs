using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    bool jump = false;
    bool attack = false;
    bool altAttack = false;
    bool dash = false;
    bool canFire = true;
    bool nextLane = false;
    bool previousLane = false;

    float horizontalAxis;
    float verticalAxis;
    float altHorizontalAxis;
    float altVerticalAxis;
    float dHorizontalAxis;
    float dVerticalAxis;
    bool canDVerticalAxis = true;
    bool canDHorizontalAxis = true;

    float jumpForce = 300f;
    float speed = 5;
    float speedForce = 200f;
    float dashForce = 500f;

    LineRenderer line;
    Animator anim;

    List<GameObject> balls = new List<GameObject>();
    float ballCount = 10;

    ParticleSystem ps;
    public AudioClip shootSound;

    float touchTime = 1;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        PopulateBalls();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        DetectInput();
        UpdateBalls();
    }

    protected virtual void FixedUpdate()
    {
        if (attack || altAttack && canFire)
        {
            Fire();
            canFire = false;
        }
        else if(!attack && !altAttack)
        {
            canFire = true;
        }

        if (Mathf.Abs(rb.velocity.magnitude) < speed)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.AddForce(new Vector3(horizontalAxis,0,verticalAxis) * speedForce);
        }

        if (dash)
        {
            StartCoroutine(Dash());
        }
    }


    void DetectInput()
    {
        jump = InputManager.AButton();
        altAttack = InputManager.RightTrigger();
        dash = InputManager.XButton();
        nextLane = InputManager.RightBumper();
        previousLane = InputManager.LeftBumper();

        horizontalAxis = InputManager.MainHorizontal();
        verticalAxis = InputManager.MainVertical();
        altHorizontalAxis = InputManager.AltHorizontal();
        altVerticalAxis = InputManager.AltVertical();
        dHorizontalAxis = InputManager.DHorizontal();
        dVerticalAxis = InputManager.DVertical();
        
        LookAtAltStickPosition(InputManager.AltJoystick());
        
        if(nextLane)
        {
            GridManager.instance.SetSelectedRow(1);
        }
        if(previousLane)
        {
            GridManager.instance.SetSelectedRow(-1);
        }

        if ((dVerticalAxis > 0) && canDVerticalAxis)
        {
            
            canDVerticalAxis = false;
        }
        else if(dVerticalAxis < 0 && canDVerticalAxis)
        {
            
            canDVerticalAxis = false;
        }
        else if(dVerticalAxis == 0)
        {
            canDVerticalAxis = true;
        }

        if ((dHorizontalAxis > 0) && canDHorizontalAxis)
        {
            ColorManager.instance.SetSelectedColor(1);
            canDHorizontalAxis = false;
        }
        else if (dHorizontalAxis < 0 && canDHorizontalAxis)
        {
            ColorManager.instance.SetSelectedColor(-1);
            canDHorizontalAxis = false;
        }
        else if (dHorizontalAxis == 0)
        {
            canDHorizontalAxis = true;
        }
    }

    protected virtual void LookAtScreenPointer(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        Vector3 lookPosition = gameObject.transform.position - Camera.main.transform.position;
        float midpoint = lookPosition.magnitude/* * .5f*/;

        Vector3 lookAt = ray.origin + ray.direction * midpoint;
        gameObject.transform.LookAt(new Vector3(lookAt.x, this.gameObject.transform.position.y, lookAt.z));
    }

    protected virtual void LookAtAltStickPosition(Vector3 pos)
    {
        if(pos != Vector3.zero)
        {
            Vector3 lookPosition = this.gameObject.transform.position + new Vector3(pos.y, 0, pos.x);

            gameObject.transform.LookAt(new Vector3(lookPosition.x, this.gameObject.transform.position.y, lookPosition.z));
        }
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.transform.parent == this.gameObject.transform)
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), col.collider);
            return;
        }

        if (col.gameObject.tag == "Enemy")
        {
            KillCharacter();
            return;
        }
    }

    protected void PopulateBalls()
    {
        for(int i = 0; i < ballCount; i++)
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Shit");
            obj = Instantiate(obj, this.gameObject.transform.position, Quaternion.identity, null);
            obj.SetActive(false);
            balls.Add(obj);
        }
    }

    protected GameObject GetBall()
    {
        foreach(GameObject ball in balls)
        {
            if(!ball.activeInHierarchy)
            {
                ball.SetActive(true);
                return ball;
            }
        }

        return null;
    }

    protected void UpdateBalls()
    {
        foreach(GameObject ball in balls)
        {
            if(!CameraController.instance.WithinCameraBounds(ball.transform.position) && ball.activeInHierarchy)
            {
                ball.SetActive(false);
                ball.transform.position = Vector3.zero;
                ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    protected virtual void Fire()
    {
        GameObject obj = GetBall();
        if (obj != null)
        {
            obj.transform.position = this.gameObject.transform.position + this.gameObject.transform.forward;
            obj.GetComponent<Rigidbody>().velocity = Vector2.zero;
            obj.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * 2000);
            ps.Play();
            SoundManager.instance.PlayPlayerSound(shootSound);
        }
    }

    protected virtual void Flip()
    {
        if (horizontalAxis < 0 && this.gameObject.transform.localScale.x > 0)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }
        else if (horizontalAxis > 0 && this.gameObject.transform.localScale.x < 0)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }
    }

    public void KillCharacter()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    protected IEnumerator Dash()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector3(horizontalAxis, 0, verticalAxis) * dashForce);
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        yield return new WaitForSeconds(.25f);
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
    }
}

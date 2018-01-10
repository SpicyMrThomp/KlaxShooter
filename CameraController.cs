using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    static public CameraController instance;

    private Camera camera;
    private float countdown = 0;
    private float timer = 1;
    private Vector3 shiftedPosition;
    private Vector3 lastPosition;
    private GameObject player;

    private float minRotation = -1f, maxRotation = 1f;

    private void Awake()
    {
        if (instance != this)
        {
            instance = this;
        }
    }

    private void Start()
    {
        lastPosition = this.gameObject.transform.position;
        camera = this.gameObject.GetComponent<Camera>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (countdown <= timer && countdown > 0)
        {
            ShakeCamera();
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void ShakeCamera()
    {
        if (countdown <= timer && countdown > 0)
        {
            Vector2 randomVector = UnityEngine.Random.insideUnitCircle * .1f;
            shiftedPosition = new Vector3(lastPosition.x + randomVector.x, lastPosition.y + randomVector.y, lastPosition.z);
            countdown -= .25f;
            this.gameObject.transform.position = shiftedPosition;

            if (countdown <= 0)
            {
                countdown = 0;
                this.gameObject.transform.position = lastPosition;
            }
        }
    }

    public void StartShakingCamera()
    {
        if (countdown == 0)
            countdown = timer;
    }

    public bool WithinCameraBounds(Vector3 pos)
    {
        float fudgeRoom = 10f;
        Vector3 screenPos = camera.WorldToScreenPoint(pos);

        if (screenPos.x <= 0 - fudgeRoom || screenPos.x >= camera.pixelRect.width + fudgeRoom)
            return false;
        else if (screenPos.y <= 0 - fudgeRoom || screenPos.y >= camera.pixelRect.height + fudgeRoom)
            return false;
        else return true;
    }

    public Vector3 GetUpperLeftBounds()
    {
        return camera.ScreenToWorldPoint(new Vector3(0,0,0));
    }

    public Vector3 GetUpperRightBounds()
    {
        return camera.ScreenToWorldPoint(new Vector3(camera.pixelRect.width, 0, 0));
    }

    public Vector3 GetLowerLeftBounds()
    {
        return camera.ScreenToWorldPoint(new Vector3(0, 0, camera.pixelRect.height));
    }

    public Vector3 GetLowerRightBounds()
    {
        return camera.ScreenToWorldPoint(new Vector3(camera.pixelRect.width, 0,camera.pixelRect.height));
    }

    public Vector3 GetCameraWidthHeight()
    {
        return camera.ScreenToWorldPoint(new Vector3(camera.pixelRect.width, camera.pixelRect.height, 0));
    }
}

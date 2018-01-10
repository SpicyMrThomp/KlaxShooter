using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager instance;
    List<GameObject> enemyList = new List<GameObject>();
    float enemyCount = 25f;
    bool spawning = false;

	// Use this for initialization
	void Start ()
    {
        PopulateList();
        instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!spawning)
            StartCoroutine(SpawnEnemy());
	}

    void PopulateList()
    {
        for(int i = 0; i < enemyCount; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), Vector3.zero, Quaternion.identity, null);
            enemyList.Add(obj);
            obj.SetActive(false);
        }
    }

    GameObject GetEnemy()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }

        return null;
    }

    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }

    IEnumerator SpawnEnemy()
    {
        spawning = true;
        int num = Random.Range(1, 4);

        yield return new WaitForSecondsRealtime(num);
        GameObject obj = GetEnemy();

        obj.transform.position = SetRandomEnemyPosition();

        obj.GetComponent<Rigidbody>().velocity = Vector2.zero;
        obj = SetRandomBasicEnemy(obj);

        spawning = false;
    }

    Vector3 SetRandomEnemyPosition()
    {
        //Enemy Position
        Vector3 pos = new Vector3();
        int num = Random.Range(1, 4);

        switch (num)
        {
            case 1: //Up
                pos = new Vector3(Random.Range(CameraController.instance.GetCameraWidthHeight().x, CameraController.instance.GetCameraWidthHeight().x + 5),
           0, CameraController.instance.GetCameraWidthHeight().z + 5);
                break;
            case 2: //Down
                pos = new Vector3(Random.Range(CameraController.instance.GetCameraWidthHeight().x, CameraController.instance.GetCameraWidthHeight().x + 5),
           0, CameraController.instance.GetCameraWidthHeight().z + 5 * -1);
                break;
            case 3: //Left
                pos = new Vector3(CameraController.instance.GetCameraWidthHeight().z + 5 * -1,
           0, Random.Range(CameraController.instance.GetCameraWidthHeight().x * -1, CameraController.instance.GetCameraWidthHeight().x));
                break;
            case 4: //Right
                pos = new Vector3(CameraController.instance.GetCameraWidthHeight().z + 5,
           0, Random.Range(CameraController.instance.GetCameraWidthHeight().x * -1, CameraController.instance.GetCameraWidthHeight().x));
                break;
            default:
                break;
        }

        return pos;
    }

    GameObject SetRandomBasicEnemy(GameObject obj)
    {
        int num = Random.Range(0, WaveManager.instance.GetWave());
        float newSpeed = (num + 1) * .75f;
        Enemy objScript = obj.GetComponent<Enemy>();

        objScript.SetHeadColor(ColorManager.instance.GetColorList()[num]);
        objScript.SetSpeed(newSpeed);

        return obj;
    }
}

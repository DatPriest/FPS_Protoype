using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> enemyList = new List<GameObject>();

    public int limit = 100;
    private int counter = 0;
    public bool enableSpawning;

    List<Coroutine> coroutines = new List<Coroutine>();

    // Start is called before the first frame update
    void Start()
    {
        if (enableSpawning)
            coroutines.Add(StartCoroutine(SpawnObject()));
    }

    private void Update()
    {
        /*
        if (true)
        {
            StopCoroutine(coroutines[0]);
        }*/
    }

    IEnumerator SpawnObject()
    {
        while (counter < limit)
        {
            enemyList.Add(Instantiate(
                prefab,
                new Vector3(Random.Range(0, 10), 3, Random.Range(0, 10)),
                Quaternion.identity
                ));
            counter++;
            //Debug.Log($"Spawned Enemy Nr.{counter}");
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}

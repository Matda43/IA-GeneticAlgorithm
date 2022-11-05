using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnGenerator))]
public class LevelGenerator : MonoBehaviour
{
    public GameObject plateformPrefab;

    public int numberOfPlatforms = 200;
    public float levelWidth = 3f;
    public float minY = 1.5f;
    public float maxY = 2.5f;

    SpawnGenerator spawnGenerator;
    GameObject[] plateforms;
    void Start()
    {
        plateforms = new GameObject[numberOfPlatforms];
        generatePlateforms();
        spawnGenerator = GetComponent<SpawnGenerator>();
        spawnGenerator.genese(plateforms[0].transform.position, levelWidth);
    }

    void Update()
    {
        
    }

    void generatePlateforms()
    {
        Vector3 spawnPosition = new Vector3(0, -4, 0);

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);
            GameObject go = Instantiate(plateformPrefab, spawnPosition, Quaternion.identity);
            plateforms[i] = go;
        }
    }

    public float getLevelWidth()
    {
        return levelWidth;
    }
}

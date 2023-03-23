using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{

  [SerializeField]
  float spawnInterval;

  [SerializeField]
  GameObject platform;

  [Range(0F, 100.0f)]
  public float _randomizeX = 5;

  [Range(0F, 100.0f)]
  public float _randomizeSpeed = 5;
  [SerializeField]
  float timeBetweenSpawns;
  float nextSpawnTime; 

  private Vector3 startPos;

  // Start is called before the first frame update
  void Start()
    {
      startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      if (Time.time > nextSpawnTime)
      {
        Spawn();
        nextSpawnTime = Time.time + timeBetweenSpawns;
      }
    }

  void Spawn()
  {
    Vector3 spawnPos = startPos;
    GameObject nextPlatform = Instantiate(platform);
    // position the cloud
    float startX = UnityEngine.Random.Range(spawnPos.x - _randomizeX, spawnPos.x + _randomizeX);
    float speed = UnityEngine.Random.Range(0, _randomizeSpeed);
    nextPlatform.transform.position = new Vector3(startX, spawnPos.y, 0);
    nextPlatform.GetComponent<RisingPlatform>().platformSpeed = speed;
    // activate the cloud
    nextPlatform.gameObject.SetActive(true);
  }
   
}

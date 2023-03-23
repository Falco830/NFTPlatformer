using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlatform : MonoBehaviour
{
  [SerializeField]
  string playerTag = "Player";
  [SerializeField]
  GameObject platformSpawner;
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.transform.tag == playerTag)
    {
      platformSpawner.SetActive(true);
      Destroy(this.gameObject);
    }
  }
}

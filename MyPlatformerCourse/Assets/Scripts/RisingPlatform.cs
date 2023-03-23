using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour
{

  [SerializeField]
  public float platformSpeed = 5;

  [SerializeField]
  float platformDestroyHeight = 100;

  Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    rb.velocity = new Vector2(0, platformSpeed);
      if (this.gameObject.transform.position.y > platformDestroyHeight)
      {
        Destroy(this.gameObject);
      }
    }
}

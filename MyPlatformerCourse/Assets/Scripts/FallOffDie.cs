using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffDie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.tag.Equals("Player"))
      {
        collision.gameObject.GetComponent<Player>().TakeDamage(1);

        if (!collision.gameObject.GetComponent<Player>().checkPoint)
        {
          collision.gameObject.GetComponent<Player>().checkPoint = FindObjectOfType<Checkpoints>().GetComponent<Checkpoints>().checkpoints[FindObjectOfType<Checkpoints>().GetComponent<Checkpoints>().checkpoints.Length - 2];
        }
        collision.gameObject.transform.SetPositionAndRotation(collision.gameObject.GetComponent<Player>().checkPoint.transform.position, new Quaternion(0, 0, 0, 0));
      }
    }
}

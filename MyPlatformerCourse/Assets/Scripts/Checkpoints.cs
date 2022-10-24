using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Checkpoint[] checkpoints;

    // Start is called before the first frame update
    void Start()
    {
        setColor();
    }

    public void setColor()
    {
      foreach (Checkpoint checkPoint in checkpoints)
      {
        if (checkPoint.isActive)
        {
          checkPoint.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
          checkPoint.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
      }
    }

    public void setColorToFalse()
    {
      foreach (Checkpoint checkPoint in checkpoints)
      {
        checkPoint.isActive = false;
        checkPoint.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
      }
    }
}

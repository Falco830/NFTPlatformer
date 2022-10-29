using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpecial : Checkpoint
{

  private GameMaster gm;

  void Start()
  {
    gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
  }
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      GameObject.Find("CheckpointsLevel3").GetComponent<Checkpoints>().setColorToFalse();
      other.GetComponent<Player>().checkPoint = this;
      isActive = true;
      GameObject.Find("CheckpointsLevel3").GetComponent<Checkpoints>().setColor();
    }
  }

  private void OnDestroy()
  {
    Checkpoint[] chec = new Checkpoint[GameObject.Find("CheckpointsLevel3").GetComponent<Checkpoints>().checkpoints.Length - 1];
    int i = 0;
    foreach (Checkpoint ch in GameObject.Find("CheckpointsLevel3").GetComponent<Checkpoints>().checkpoints)
    {
      if(ch == this)
      {

      }
      else     
      {
        chec[i++] = ch;
      }
    }
    chec[0].isActive = true;
    GameObject.Find("Player").GetComponent<Player>().checkPoint = chec[0];
    GameObject.Find("CheckpointsLevel3").GetComponent<Checkpoints>().checkpoints = chec;

  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpecial : Checkpoint
{

 // private GameMaster gm;

  void Start()
  {
   // gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
  }
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      GameObject.Find("Checkpoints").GetComponent<Checkpoints>().setColorToFalse();
      other.GetComponent<Player>().checkPoint = this;
      isActive = true;
      GameObject.Find("Checkpoints").GetComponent<Checkpoints>().setColor();
    }
  }

  private void OnDestroy()
  {
    if (GameObject.Find("Checkpoints"))
    {
      Checkpoint[] chec = new Checkpoint[GameObject.Find("Checkpoints").GetComponent<Checkpoints>().checkpoints.Length - 1];
      int i = 0;
      foreach (Checkpoint ch in GameObject.Find("Checkpoints").GetComponent<Checkpoints>().checkpoints)
      {
        if (ch == this)
        {
          //Same Checkpoint
        }
        else
        {
          chec[i++] = ch;
        }
      }
      if (i != 0)
      {
        chec[0].isActive = true;
        if (GameObject.FindGameObjectWithTag("Player"))
        {
          GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().checkPoint = chec[GameObject.Find("Checkpoints").GetComponent<Checkpoints>().checkpoints.Length - 2];
          GameObject.Find("Checkpoints").GetComponent<Checkpoints>().checkpoints = chec;
        }
      }
    }
  }
}

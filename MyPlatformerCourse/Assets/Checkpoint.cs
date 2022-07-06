using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public bool isActive = false;
    public Checkpoints checkpoints;

  private GameMaster gm;

   void Start()
  {
    gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
  }
  private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
          this.GetComponentInParent<Checkpoints>().setColorToFalse();
          other.GetComponent<Player>().checkPoint = this;
          isActive = true;
          this.GetComponentInParent<Checkpoints>().setColor();
        }
    }
}

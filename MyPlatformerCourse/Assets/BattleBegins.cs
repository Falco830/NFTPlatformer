using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBegins : MonoBehaviour
{

  public GameObject gong;
  public GameObject greta;
  public GameObject grim;

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

      gong.GetComponent<GongController>().awake = true;
      greta.GetComponent<GretaController>().awake = true;
      grim.GetComponent<GrimController>().awake = true;

      gong.GetComponent<GongController>().asleep = false;
      greta.GetComponent<GretaController>().asleep = false;
      grim.GetComponent<GrimController>().asleep = false;

    }
  }
}

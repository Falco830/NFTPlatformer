using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleBegins : MonoBehaviour
{

  public GameObject gong;
  public GameObject greta;
  public GameObject grim;



  public GameObject FlyerGong;
  public GameObject FlyerGreta;
  public GameObject FlyerGrim;

  public GameObject[] Flyers;

  public int headsAlive = 3;

  public bool playerAlive = true;


  public AudioSource intro;
  public AudioSource battle;
  public AudioSource finalHits;


  // Start is called before the first frame update
  void Start()
    {
    battle.Stop();
    finalHits.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag.Equals("Player"))
    {

      intro.Stop();
      battle.Play();


      gong.GetComponent<GongController>().awake = true;
      greta.GetComponent<GretaController>().awake = true;
      grim.GetComponent<GrimController>().awake = true;

      gong.GetComponent<GongController>().asleep = false;
      greta.GetComponent<GretaController>().asleep = false;
      grim.GetComponent<GrimController>().asleep = false;

      gong.GetComponent<BoxCollider2D>().enabled = true;
      greta.GetComponent<BoxCollider2D>().enabled = true;
      grim.GetComponent<BoxCollider2D>().enabled = true;

      gong.layer = 9;
      greta.layer = 9;
      grim.layer = 9;

      foreach(GameObject fl in Flyers)
      {
        fl.SetActive(true);
        fl.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
      }
      FlyerGong.SetActive(true);
      FlyerGreta.SetActive(true);
      FlyerGrim.SetActive(true);
      FlyerGong.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
      FlyerGreta.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
      FlyerGrim.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
    }
  }

  public void DeadHeads()
  {
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    headsAlive--;
    Debug.Log("Heads: " + headsAlive);

    if(headsAlive == 1)
    {
      battle.Stop();
      finalHits.Play();
    }

    if (headsAlive == 0 && player != null)
    {
      SceneManager.LoadScene("VictoryScene");
    }
  }



}

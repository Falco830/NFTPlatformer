using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleBegins : MonoBehaviour
{

  public GameObject gong;
  public GameObject greta;
  public GameObject grim;

  public GameObject FallOffDie;

  public GameObject FlyerGong;
  public GameObject FlyerGreta;
  public GameObject FlyerGrim;

  public GameObject[] Flyers;

  public int headsAlive = 3;

  public bool playerAlive = true;


  public AudioSource intro1;
  public AudioSource intro2;
  public AudioSource intro3;
  public AudioSource endingTheme;
  public AudioSource battle1;
  public AudioSource battle2;
  public AudioSource finalHits;
  public AudioSource victoryTheme;


  public BoxCollider2D level1Trigger;
  public BoxCollider2D level3Trigger;

  // Start is called before the first frame update
  void Start()
    {
    SceneManager.sceneLoaded += this.OnLoadCallback;

    intro1.Stop();
    intro2.Stop();
    intro3.Stop();
    endingTheme.Stop();
    battle1.Stop();
    battle2.Stop();
    finalHits.Stop();
    victoryTheme.Stop();
    if (SceneManager.GetActiveScene().name.Equals("Level1Pro"))
    {
      level1Trigger.enabled = true;
      level3Trigger.enabled = false;
      intro1.Play();
    }
    else if (SceneManager.GetActiveScene().name.Equals("Level2Pro"))
    {
      intro2.Play();
      level1Trigger.enabled = false;
      level3Trigger.enabled = false;
    }
    else if (SceneManager.GetActiveScene().name.Equals("Level3Pro"))
    {
      level1Trigger.enabled = false;
      level3Trigger.enabled = true;
      intro3.Play();
    }
  }

  void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
  {
    intro1.Stop();
    intro2.Stop(); 
    intro3.Stop();
    endingTheme.Stop();
    battle1.Stop();
    battle2.Stop();
    finalHits.Stop();
    victoryTheme.Stop();

    if (SceneManager.GetActiveScene().name.Equals("Level1Pro")){
      level1Trigger.enabled = true;
      level3Trigger.enabled = false;
      intro1.Play();
      if(headsAlive < 3)
      {
        headsAlive = 3;
      }

    }else if (SceneManager.GetActiveScene().name.Equals("Level2Pro"))
    {
      level1Trigger.enabled = false;
      level3Trigger.enabled = false;
      intro2.Play();
    }
    else if (SceneManager.GetActiveScene().name.Equals("Level3Pro"))
    {
      level1Trigger.enabled = false;
      level3Trigger.enabled = true;
      intro3.Play();
      if (headsAlive < 3)
      {
        headsAlive = 3;
      }
    }

  }
  // Update is called once per frame
  void Update()
    {
      
    }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    FallOffDie = GameObject.Find("FallOffDie");
    if (collision.tag.Equals("Player"))
    {      
      intro1.Stop();
      intro2.Stop();
      intro3.Stop();
      if (SceneManager.GetActiveScene().name.Equals("Level1Pro"))
      {
        battle1.Play();
      }
      else if (SceneManager.GetActiveScene().name.Equals("Level3Pro"))
      {
        battle2.Play();
      }

      if (!SceneManager.GetActiveScene().name.Equals("Level3Pro"))
      {
        gong.GetComponent<GongController>().awake = true;
        greta.GetComponent<GretaController>().awake = true;
        grim.GetComponent<GrimController>().awake = true;

        gong.GetComponent<GongController>().asleep = false;
        greta.GetComponent<GretaController>().asleep = false;
        grim.GetComponent<GrimController>().asleep = false;

        gong.GetComponent<GongController>().Fist.GetComponent<GretasFist>().asleep = false;
        greta.GetComponentInChildren<GretasFist>().asleep = false;
        grim.GetComponentInChildren<GretasFist>().asleep = false;

        gong.GetComponent<BoxCollider2D>().enabled = true;
        greta.GetComponent<BoxCollider2D>().enabled = true;
        grim.GetComponent<BoxCollider2D>().enabled = true;

        gong.layer = 9;
        greta.layer = 9;
        grim.layer = 9;
      }
      level1Trigger.enabled = false;
      //level1Trigger.isTrigger = false;
      level3Trigger.enabled = false;
      //level3Trigger.isTrigger = false;
      FallOffDie.GetComponent<BoxCollider2D>().enabled = true;
      FallOffDie.SetActive(true);

      /*
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
      FlyerGrim.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;*/
    }
  }

  public void DeadHeads()
  {
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    headsAlive--;

    if(headsAlive == 1)
    {
      if (SceneManager.GetActiveScene().name.Equals("Level1Pro"))
      {
        if (battle1)
        {
          battle1.Stop();
        }
      }
      else if (SceneManager.GetActiveScene().name.Equals("Level3Pro"))
      {
        if (battle2)
        {
          battle2.Stop();
        }
      }
     
      
      finalHits.Play();
    }

    if (headsAlive == 0 && player != null)
    {
      if (FallOffDie)
      {
        FallOffDie.SetActive(false);
      }
      if (!SceneManager.GetActiveScene().name.Equals("Level3Pro"))
      {
        FindObjectOfType<Player>().GetComponent<Player>().checkPoint = FindObjectOfType<Checkpoints>().GetComponent<Checkpoints>().checkpoints[FindObjectOfType<Checkpoints>().GetComponent<Checkpoints>().checkpoints.Length - 2];
        FindObjectOfType<Player>().gameObject.transform.SetPositionAndRotation(FindObjectOfType<Checkpoints>().GetComponent<Checkpoints>().checkpoints[FindObjectOfType<Checkpoints>().GetComponent<Checkpoints>().checkpoints.Length - 2].transform.position, new Quaternion(0, 0, 0, 0));
        finalHits.Stop();
        victoryTheme.Play();

        endingTheme.PlayDelayed(10);
      }
      else
      {
        SceneManager.LoadScene("VictoryScenePro");
      }
    }
  }

  private void OnDestroy()
  {
    SceneManager.sceneLoaded -= this.OnLoadCallback;
  }
}

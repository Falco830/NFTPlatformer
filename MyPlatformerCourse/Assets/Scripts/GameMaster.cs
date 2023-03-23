using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

  private static GameMaster instance;
  public Vector2 lastCheckpointPos;
  [SerializeField] private GameObject player;
  [SerializeField] private GameObject cmvCam;

  [SerializeField] private GameObject sword;

  public GameObject gameoverPanel;

    // Start is called before the first frame update
    void Awake()
    {
    SceneManager.sceneLoaded += this.OnLoadCallback;
    if (instance == null)
    {
      cmvCam = GameObject.Find("CM vcam1");
      InstantiatePlayer();
      instance = this;
      //DontDestroyOnLoad(instance);
    }
    else if (player == null && StaticClass.character != null)
    {
      cmvCam = GameObject.Find("CM vcam1");
      InstantiatePlayer();
      //player = StaticClass.character;
      FindObjectOfType<BattleBegins>().intro1.Stop();
      FindObjectOfType<BattleBegins>().intro2.Stop();
      FindObjectOfType<BattleBegins>().intro3.Stop();
      FindObjectOfType<BattleBegins>().endingTheme.Stop();
      FindObjectOfType<BattleBegins>().battle1.Stop();
      FindObjectOfType<BattleBegins>().battle2.Stop();
      FindObjectOfType<BattleBegins>().finalHits.Stop();
      FindObjectOfType<BattleBegins>().victoryTheme.Stop();
      FindObjectOfType<BattleBegins>().enabled = false;
      FindObjectOfType<BattleBegins>().GetComponent<BoxCollider2D>().enabled = false;
      instance = this;
      //DontDestroyOnLoad(instance);
    }
    /*else if (SceneManager.GetActiveScene().IsValid())
    {
      FindObjectOfType<BattleBegins>().intro.Stop();
      FindObjectOfType<BattleBegins>().battle.Stop();
      FindObjectOfType<BattleBegins>().finalHits.Stop();
      FindObjectOfType<BattleBegins>().victoryTheme.Stop();
      FindObjectOfType<BattleBegins>().enabled = false;
      FindObjectOfType<BattleBegins>().GetComponent<BoxCollider2D>().enabled = false;
      InstantiatePlayer();
      instance = this;
      DontDestroyOnLoad(instance);
    }*/
    else
    {
      //Destroy(gameObject);
    }

    
    LevelUnocked();
  }
  void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
  {
    if(SceneManager.GetActiveScene().name != "MainMenuSampleScene" && SceneManager.GetActiveScene().name != "LevelSelection")
    {
      if (!FindObjectOfType<Player>())
      {
        if (instance == null)
        {
          InstantiatePlayer();
          instance = this;
          //DontDestroyOnLoad(instance);
        }
        else if (player == null && StaticClass.character != null)
        {
          cmvCam = GameObject.Find("CM vcam1");
          InstantiatePlayer();
          //player = StaticClass.character;
          FindObjectOfType<BattleBegins>().intro1.Stop();
          FindObjectOfType<BattleBegins>().intro2.Stop();
          FindObjectOfType<BattleBegins>().intro3.Stop();
          FindObjectOfType<BattleBegins>().endingTheme.Stop();
          FindObjectOfType<BattleBegins>().battle1.Stop();
          FindObjectOfType<BattleBegins>().battle2.Stop();
          FindObjectOfType<BattleBegins>().finalHits.Stop();
          FindObjectOfType<BattleBegins>().victoryTheme.Stop();
          FindObjectOfType<BattleBegins>().enabled = false;
          //FindObjectOfType<BattleBegins>().GetComponent<BoxCollider2D>().enabled = false;

          instance = this;
          //DontDestroyOnLoad(instance);
        }
        /*else if (SceneManager.GetActiveScene().IsValid())
        {
          FindObjectOfType<BattleBegins>().intro.Stop();
          FindObjectOfType<BattleBegins>().battle.Stop();
          FindObjectOfType<BattleBegins>().finalHits.Stop();
          FindObjectOfType<BattleBegins>().victoryTheme.Stop();
          FindObjectOfType<BattleBegins>().enabled = false;
          FindObjectOfType<BattleBegins>().GetComponent<BoxCollider2D>().enabled = false;
          InstantiatePlayer();
          instance = this;
          DontDestroyOnLoad(instance);
        }*/
        else
        {
          Destroy(gameObject);
         
        }
        LevelUnocked();
      }

    }
    else
    {
      Destroy(gameObject);
    }

  }
  public void ChangeShot(int shot, float positiony, float screeny)
  {
      cmvCam = FindObjectOfType<CinemachineVirtualCamera>().gameObject;
      cmvCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = shot;
      cmvCam.GetComponent<CinemachineVirtualCamera>().transform.position.Set(0, positiony, 0);

      cmvCam = FindObjectOfType<CinemachineFramingTransposer>().gameObject;
      cmvCam.GetComponent<CinemachineFramingTransposer>().m_ScreenY = screeny;
    //cmvCam.GetComponent<CinemachineVirtualCamera>().);
  }
    public void WideShot()
  {
    cmvCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 80;
  }
  public void NormalShot()
  {
    cmvCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 60;
  }
  public void CloseUpShot()
  {
    cmvCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40;
  }

  private void InstantiatePlayer()
    {
      if(StaticClass.character != null)
      {
        
        player = Instantiate(StaticClass.character);
        if(StaticClass.GFX != null)
        {
        player.GetComponent<Player>().damage = StaticClass.damage;
        player.GetComponent<Player>().attackRange = StaticClass.attackRange;
        player.GetComponent<Player>().speed = StaticClass.speed;
        player.GetComponent<Player>().jumpForce = StaticClass.jumpForce;
        if (StaticClass.health <= 0 && StaticClass.lives <= 0)
        {
          StaticClass.health = 6;
          StaticClass.lives = 3;
        }
        player.GetComponent<Player>().health = StaticClass.health;
        player.GetComponent<Player>().lives = StaticClass.lives;

        if (StaticClass.newWeapon)
        {
          player.GetComponent<Player>().NewWeaponRenderer.sprite = StaticClass.GFX;
          player.GetComponent<Player>().NewWeaponRenderer.gameObject.SetActive(true);
          player.GetComponent<Player>().weaponRenderer.sprite = null;
        }
        else
        {
          player.GetComponent<Player>().weaponRenderer.sprite = StaticClass.GFX;
          player.GetComponent<Player>().NewWeaponRenderer.gameObject.SetActive(false);
        }
        

        }

        if (cmvCam)
        {
          cmvCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
          cmvCam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
        }

      }
      else
      {
        if (StaticClass.health <= 0 && StaticClass.lives <= 0)
        {
          StaticClass.health = 6;
          StaticClass.lives = 3;
        }
        player = Instantiate(player);

        cmvCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        cmvCam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
      }

    }

    public void LevelUnocked()
    {
      if(StaticClass.level != 3)
      {
        
        switch(SceneManager.GetActiveScene().ToString())
          {
            case "Level1Pro":
              StaticClass.level = 1;
            break;
          case "Level2Pro":
            StaticClass.level = 2;
            break;
          case "Level3Pro":
            StaticClass.level = 3;
            break;
          default:
            StaticClass.level = 1;
            break;
        }
      }
    }

    public void GameMasterWakeup()
    {
      Awake();
    }

    public void DestroyCharacter()
    {
      StartCoroutine(DestroyPlayer("MainMenuSampleScene"));//"CharacterSelect"));
    }

  private void OnDestroy()
  {
    SceneManager.sceneLoaded -= this.OnLoadCallback;
  }
  IEnumerator DestroyPlayer(string sceneName)
    {
      Destroy(player);
      if (GameObject.Find("CanvasMaster"))
      {
        gameoverPanel = GameObject.Find("CanvasMaster").transform.Find("GameOverPanel").gameObject;
        gameoverPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        Destroy(gameObject);
      }



    }
}

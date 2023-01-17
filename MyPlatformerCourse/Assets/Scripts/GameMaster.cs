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
    if (instance == null)
          {
            InstantiatePlayer();
            instance = this;
            DontDestroyOnLoad(instance);
          }
          else if (StaticClass.character != null)
            {
            player = StaticClass.character;
            cmvCam = GameObject.Find("CM vcam1");
            InstantiatePlayer();
            instance = this;
            DontDestroyOnLoad(instance);
          }
          else
          {
            Destroy(gameObject);
          }
    //LevelUlnocked();
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
        player.GetComponent<Player>().weaponRenderer.sprite = StaticClass.GFX;

        }
        
          
        cmvCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        cmvCam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
      }
      else
      {
        player = Instantiate(player);
        cmvCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        cmvCam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
      }

    }

   /* public void LevelUnocked()
    {
      if(StaticClass.level != 3)
      {
        switch(SceneManager.GetActiveScene().ToString())
          {
            case "Level1Custom":
              StaticClass.level = 1;
            break;
          case "Level2Custom":
            StaticClass.level = 2;
            break;
          case "Level3Custom":
            StaticClass.level = 3;
            break;
          default:
            StaticClass.level = 1;
            break;
        }
      }
    }*/

    public void GameMasterWakeup()
    {
      Awake();
    }

    public void DestroyCharacter()
    {
      StartCoroutine(DestroyPlayer("MainMenuSampleScene"));//"CharacterSelect"));
    }

    IEnumerator DestroyPlayer(string sceneName)
    {
      Destroy(player);
      gameoverPanel = GameObject.Find("Canvas").transform.Find("GameOverPanel").gameObject;
      gameoverPanel.SetActive(true);
      yield return new WaitForSeconds(1f);   
      SceneManager.LoadScene(sceneName);
      Destroy(gameObject);     
    }
}

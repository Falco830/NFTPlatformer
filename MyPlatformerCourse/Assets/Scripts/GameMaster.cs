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

    Debug.Log("Instance " + instance);
    Debug.Log("StaticClass.character " + StaticClass.character);
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
    }

    private void InstantiatePlayer()
    {
      Debug.Log("Instantiateing Player");
      if(StaticClass.character != null)
      {
        player = Instantiate(StaticClass.character);
        
      Debug.Log("Player " + StaticClass.weapon);
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

    public void GameMasterWakeup()
    {
      Awake();
    }

    public void DestroyCharacter()
    {
      StartCoroutine(DestroyPlayer("CharacterSelect"));
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

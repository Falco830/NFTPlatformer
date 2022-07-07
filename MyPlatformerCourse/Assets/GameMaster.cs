using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameMaster : MonoBehaviour
{

  private static GameMaster instance;
  public Vector2 lastCheckpointPos;
  [SerializeField] private GameObject player;
  [SerializeField] private GameObject cmvCam;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
          {
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
      if(StaticClass.character != null)
    {
      player = Instantiate(StaticClass.character);
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
}

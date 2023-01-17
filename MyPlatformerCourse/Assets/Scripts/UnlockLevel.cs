using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnlockLevel : MonoBehaviour
{
    private Button button;
    public GameObject chains;
    public string sceneName;

    private void Start()
    {
        button = GetComponent<Button>();

        switch (sceneName)
        {
          case "Level2Custom":
            if (StaticClass.level >= 2)
            {
              button.interactable = true;
              chains.SetActive(false);
            }          
            break;
          case "Level3Custom":
            if (StaticClass.level >= 3)
            {
              button.interactable = true;
              chains.SetActive(false);
            }
            break;
        }

        /*if (StaticClass.level > (SceneManager.GetSceneByName(sceneName).buildIndex))
        {
          button.interactable = true;
          chains.SetActive(false);
        }*/
        /*if (PlayerPrefs.GetInt(sceneName, 0) == 3)
        {
          button.interactable = true;
          chains.SetActive(false);
        }*/
  }
}

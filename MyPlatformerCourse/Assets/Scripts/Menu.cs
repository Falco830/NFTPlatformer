using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject panel;
  [SerializeField] private GameObject character1;
  public void LoadScene(string sceneName) {
        StaticClass.character = character1;
        Debug.Log(StaticClass.character);
        StartCoroutine(FadeIn(sceneName));

    }

    public void Quit() {
        Application.Quit();
    }

    IEnumerator FadeIn(string sceneName) {
        panel.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}

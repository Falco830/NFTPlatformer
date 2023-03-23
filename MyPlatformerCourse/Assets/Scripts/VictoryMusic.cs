using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMusic : MonoBehaviour
{

  public AudioSource victoryTheme;
  public AudioSource victoryMusic;

  // Start is called before the first frame update
  void Start()
    {
    victoryTheme.Play();
    victoryMusic.PlayDelayed(13);
  }

  private void OnDestroy()
  {
    victoryTheme.Stop();
    victoryMusic.Stop();
  }
}

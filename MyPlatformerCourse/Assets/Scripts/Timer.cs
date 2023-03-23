using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
  public bool gameWon = false;
  bool stopWatchActive = false;
  float currentTime;
  public TextMeshProUGUI currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
      currentTime = StaticClass.time;
      currentTimeText = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
      if(stopWatchActive == true)
      {
        currentTime = currentTime + Time.deltaTime;
      }
      TimeSpan time = TimeSpan.FromSeconds(currentTime);
      currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
    }

    public void StartStopwatch()
    {
      stopWatchActive = true;
    }
    public void StopStopwatch()
    {
      stopWatchActive = false;
      StaticClass.time = currentTime;
    }
    public void ResetStopwatch()
    {     
      currentTime = 0;
    }

  private void OnDestroy()
  {
    if (gameWon)
    {
      ResetStopwatch();
    }
    StaticClass.time = currentTime;
  }
}

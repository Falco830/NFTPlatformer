using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeThreeKings : MonoBehaviour
{
    public BattleBegins battleState;

    // Start is called before the first frame update
    void Start()
    {
    battleState = FindObjectOfType<BattleBegins>();
    }

    // Update is called once per frame
    void Update()
    {
      if (!battleState)
      {
        battleState = FindObjectOfType<BattleBegins>();
      }
    }
  private void OnDestroy()
  {
    battleState.DeadHeads();
  }
}

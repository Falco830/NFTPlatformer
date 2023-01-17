using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using UnityEngine;

public class WideShotTrigger : MonoBehaviour
{
  public GameMaster gm;
  public int shotsize = 60;
  public float positiony = 0;
  public float screeny = .69f;
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag.Equals("Player"))
    {
      gm = FindObjectOfType<GameMaster>();
      gm.ChangeShot(shotsize, positiony, screeny);
    }
  }

}

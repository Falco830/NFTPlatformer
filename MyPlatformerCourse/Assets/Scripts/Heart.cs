using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

    public int numberOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite brokenHeart;
    Player player;
    public int numberOfLives;
    public TextMeshProUGUI lives;


    private void Start()
    {
        player = FindObjectOfType<Player>();
        lives.text = player.lives.ToString();
    }

    private void Update()
    {

        if (player.health > numberOfHearts)
        {
            player.health = numberOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < numberOfHearts)
            {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }

            if (i < player.health)
            {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = brokenHeart;
            }

        }
        if (player.lives < numberOfLives)
        {
          lives.text = player.lives.ToString();
          numberOfLives--;
        }
    }

}

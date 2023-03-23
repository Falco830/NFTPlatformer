using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticClass
{
    public static GameObject character { get; set; }
    public static Player player { get; set; }
    public static int level { get; set; }

    public static Weapon weapon { get; set; }
    public static bool newWeapon { get; set; }
    public static float turnSpeed { get; set; }
  public static int health { get; set; } = 6;
    public static int lives { get; set; } = 3;

  public static float time { get; set; } = 0;

  public static int damage { get; set; }
    public static float speed { get; set; } = 10;
    public static float jumpForce { get; set; } = 35;
    public static float attackRange { get; set; }
    public static Sprite GFX { get; set; }

}

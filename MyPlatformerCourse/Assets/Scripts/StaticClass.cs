using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticClass
{
    public static GameObject character { get; set; }
    public static Player player { get; set; }
    public static int level { get; set; }

    public static Weapon weapon { get; set; }
    public static float turnSpeed { get; set; }
    public static int damage { get; set; }
    public static float attackRange { get; set; }
    public static Sprite GFX { get; set; }

}

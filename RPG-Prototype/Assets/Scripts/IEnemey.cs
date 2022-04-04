using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IEnemy
{
    float Health { get; }
    Vector2 Position { get; }
    Material Material { set; }
    GameObject GameObject { get; }
    string Name { get; }
    Slider HealthBar { get; set; }



    void Attack(float damage);
}

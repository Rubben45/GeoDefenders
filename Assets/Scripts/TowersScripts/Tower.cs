using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clasa abstracta folosita pentru a interactiona cu turnurile si a da reload
public abstract class Tower : MonoBehaviour
{
    public TowerLocation CurrentTowerLocation { get; set; }
    public abstract void ReloadAmmo();

    public enum TowerLocation
    {
        Up,
        Down,
        Left,
        Right
    }

}

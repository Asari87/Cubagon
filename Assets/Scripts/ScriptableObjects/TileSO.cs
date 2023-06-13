using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Cubagon/Tile", order = 1)]
public class TileSO : ScriptableObject
{
    public Sprite sprite;
    public Player owner;
}

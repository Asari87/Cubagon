using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour, IPlayerController
{
    public TileSO tile;
    public abstract void DisableControl();

    public abstract void EnableControl();

    public Action OnNoMovesLeft;
    protected virtual void Awake()
    {
        if(tile != null)
            tile.owner = this;
    }

    public void SetTile(TileSO tile)
    {
        this.tile = tile;
        tile.owner = this;
    }
}

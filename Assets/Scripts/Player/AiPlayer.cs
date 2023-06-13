using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.VisualScripting;

using UnityEngine;

using Random = UnityEngine.Random;

public class AiPlayer : Player
{
    private CubagonManager gameManager;
    private CubagonGrid gridManager;
    private bool activeTurn = false;
    private bool actionPerformed = false;
    protected override void Awake()
    {
        base.Awake();
        gameManager = FindObjectOfType<CubagonManager>();
        gridManager = FindObjectOfType<CubagonGrid>();
    }
    private void Update()
    {
        if (!activeTurn) return;

        List<Coords> potentialTiles = gridManager.grid.SelectByFilter(MyTilesFilter);
        
        actionPerformed = false;

        //Randomize selection each time
        System.Random rnd = new System.Random();
        potentialTiles = potentialTiles.OrderBy(x => rnd.Next()).ToList();

        foreach (Coords potentialTile in potentialTiles)
        {
            List<Coords> perimeter = gridManager.grid.GetTilesAroundPoint(potentialTile.x, potentialTile.y, 2)
                .Where(coords => gridManager.grid.GetValue(coords.x, coords.y) == null)
                .ToList(); 
            if(perimeter.Count > 0)
            {
                gameManager.HandlePlayerInput(this, potentialTile.x, potentialTile.y);
                Coords random = perimeter[Random.Range(0, perimeter.Count)];
                gameManager.HandlePlayerInput(this, random.x, random.y);
                Debug.Log("Success!");
                actionPerformed = true;
                break;
            }

        }

        if (!actionPerformed)
        {
            Debug.Log($"{this}: Failed to find a valid move!");
            OnNoMovesLeft?.Invoke();
            return;
        }


    }

    private bool MyTilesFilter(TileSO tileToCheck)
    {
        if(tileToCheck == null) return false;
        return tileToCheck.owner == this;
    }

    public override void DisableControl()
    {
        activeTurn = false;
    }

    public override void EnableControl()
    {
        activeTurn = true;
    }
}



using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using UnityEngine;



public class DisplayManager : MonoBehaviour
{
    [SerializeField] private TileController tilePrefab;

    private CubagonGrid gameGrid;
    private TileController[,] gameSprites;
    private void Start()
    {
        gameGrid = CubagonGrid.Instance;

        gameSprites = new TileController[gameGrid.width, gameGrid.height];
        gameGrid.grid.OnGridPositionUpdate += HandlerDataUpdate;

    }
    
    private void OnDestroy()
    {
        gameGrid.grid.OnGridPositionUpdate -= HandlerDataUpdate;
    }

    private void HandlerDataUpdate(int x, int y)
    {
        //Check if tile moved
        if (gameGrid.grid.GetValue(x, y) == null)
        {
            gameSprites[x, y].Hide();
            return;
        }
        //Spawn new tile
        if (gameSprites[x,y] == null)
        {
            Vector3 cellCenter = gameGrid.grid.GetWorldPosition(x, y) + Vector3.one * gameGrid.cellSize / 2;
            TileController sr = Instantiate(tilePrefab, cellCenter, Quaternion.identity.normalized, transform);
            sr.transform.localScale = Vector3.one * gameGrid.cellSize * 2;
            gameSprites[x,y] = sr;

        }
        //Update tile visual
        gameSprites[x, y].Show(gameGrid.grid.GetValue(x, y).owner.tile.sprite);

        gameSprites[x, y].Scale();
    }
}

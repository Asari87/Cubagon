
using System;
using System.Collections;

using UnityEngine;

public enum PlayerType { Human, AI}
public class CubagonManager : MonoBehaviour
{
    private CubagonGrid gameGrid;
    private GenericGrid<TileSO> grid;

    private TileSO currentActiveTile;
    private int selectTileX, selectTileY;
    private Player activePlayer;

    [Header("Runtime Filled")]
    [SerializeField] private Player p1;
    [SerializeField] private Player p2;

    [SerializeField] private float turnDelay = .5f;
    private float timeSinceTurnEnded;

    #region Initializations
    private void Awake()
    {
        //Get grid reference
        gameGrid = CubagonGrid.Instance;

        //Initialize players
        GameSettingSO settings = Resources.Load<GameSettingSO>(Constants.GAME_SETTING);
        PlayerFactory pf = new PlayerFactory();
        p1 = pf.CreatePlayer(settings.p1Type);
        p2 = pf.CreatePlayer(settings.p2Type);
    }

    IEnumerator Start()
    {
        //Initialize Board
        grid = gameGrid.grid;
        SpawnPlayerTiles();

        //Can implement UI counter here
        yield return new WaitForSeconds(1);
        yield return SetPlayerTurn();
    }

    
    
    private void SpawnPlayerTiles()
    {
        grid.SetValue(0, 0, p1.tile);
        grid.SetValue(gameGrid.width - 1, 0, p2.tile);
        grid.SetValue(0, gameGrid.height - 1, p2.tile);
        grid.SetValue(gameGrid.width - 1, gameGrid.height - 1, p1.tile);
    }

    #endregion

    #region Player
    /// <summary>
    /// Coroutine, in case I'll want an animation to play between turns
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetPlayerTurn()
    {
        if(activePlayer == null)
            activePlayer = p1;
        else
        {
            activePlayer.OnNoMovesLeft -= HandlePlayerTechLost;
            activePlayer.DisableControl();
            timeSinceTurnEnded = 0;
            while (timeSinceTurnEnded < turnDelay) 
            {
                yield return null;
            }

            activePlayer = activePlayer == p1 ? p2 : p1;
        }
        Debug.Log($"{activePlayer}'s turn");
        


        activePlayer.OnNoMovesLeft += HandlePlayerTechLost;
        activePlayer.EnableControl();
        currentActiveTile = null;

    }
    public void HandlePlayerInput(Player player, Vector3 mPos)
    {
        if (grid.GetXY(mPos, out int x, out int y))
        {
            HandlePlayerInput(player, x, y);
        }
    }
    public void HandlePlayerInput(Player player, int x, int y)
    {
        //safty check
        if (player != activePlayer) return;
        //calculate results
        StartCoroutine(CalculateInputResult(player, x, y));

    }
    private void Update()
    {
        //turn delay counter
        timeSinceTurnEnded += Time.deltaTime;
    }
    private void HandlePlayerTechLost()
    {
        Debug.Log($"{activePlayer} has no more moves!");
        //Disable player 
        activePlayer.OnNoMovesLeft -= HandlePlayerTechLost;
        activePlayer.DisableControl();
        activePlayer = null;

        DecisionUI.Instance.ShowMessage("Game Over", "Restart?", OnDecisionMade);
    }
    /// <summary>
    /// Decision UI callback
    /// </summary>
    /// <param name="restart"></param>
    private void OnDecisionMade(bool restart)
    {
        if (restart)
            SceneHandler.Instance.RestartLevel();
        else
            SceneHandler.Instance.LoadScene(0);
    }

    #endregion

    #region Turn Logic Functions
    

    private IEnumerator CalculateInputResult(Player player, int x, int y)
    {
        //Disable current player control and checl input
        activePlayer.DisableControl();
        //Get sekected tile value
        TileSO selectedTile = grid.GetValue(x, y);

        //Move if possible
        if (selectedTile == null && currentActiveTile != null)
        {
            //try moving tile if possible
            if (TryMovingTile(x, y))
            {
                //Tile moved, update neighbors
                TryTurningNeighbors(x, y);
                //Check end game condition
                if (grid.All(IsNotEmpty))
                {
                    Debug.Log("Game Over");
                    DecisionUI.Instance.ShowMessage("Game Over", "Restart?", OnDecisionMade);
                    yield break;
                }
                //Switch players
                yield return StartCoroutine(SetPlayerTurn());
            }
        }
        else if (selectedTile != null)
        {
            if (selectedTile.owner == player)
            {
                selectTileX = x;
                selectTileY = y;
                currentActiveTile = selectedTile;
            }

        }
        //Reenable controls
        activePlayer.EnableControl();

    }

    /// <summary>
    /// If possible clone to jump tile in the desired location
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool TryMovingTile(int x, int y)
    {
        if(grid.GetValue(x, y) == null)
        {
            float desiredDistance = Vector3.Distance(new Vector2(x, y), new Vector2(selectTileX, selectTileY));
            if(desiredDistance < 2)
            {
                grid.SetValue(x, y, activePlayer.tile);
                return true;
            }
            else if (desiredDistance < 3)
            {
                grid.SetValue(selectTileX, selectTileY, null);
                grid.SetValue(x, y, activePlayer.tile);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Iterate a 3x3 square around (x,y) and look for oppsite player's tiles
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void TryTurningNeighbors(int x, int y)
    {
        int xMin = x - 1 < 0 ? 0 : x - 1;
        int xMax = x + 1 > gameGrid.width ? gameGrid.width : x + 1;
        int yMin = y - 1 < 0 ? 0 : y - 1;
        int yMax = y + 1 > gameGrid.height ? gameGrid.height : y + 1;

        TileSO currentTile = grid.GetValue(x, y);

        for (int i = xMin; i <= xMax; i++)
        {
            for (int j = yMin; j <= yMax; j++)
            {
                if (i == x && j == y) continue;
                TileSO tile = grid.GetValue(i, j);
                if (tile == null) continue;
                if (tile.owner == currentTile.owner) continue;

                grid.SetValue(i, j, currentTile);

            }
        }
    }
    #endregion

    #region Grid Functions
    /// <summary>
    /// Predicate for grid testing
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    private bool IsNotEmpty(TileSO element)
    {
        return element != null;
    }

    /// <summary>
    /// Check if current grid contain any tiles belong to the active player
    /// </summary>
    /// <param name="tileToCheck"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private bool HasAnyTiles(TileSO tileToCheck)
    {
        if (tileToCheck == null)
            return false;
        if (tileToCheck.owner == activePlayer)
            return true;
        return false;

    }
    #endregion
}



using System;

using UnityEngine;

public class PlayerFactory
{
    public static int playerIndex = 0;
    private TileSO[] tiles;
    public PlayerFactory()
    {
        tiles = Resources.LoadAll<TileSO>("Tiles");
        playerIndex = 0;
    }
    public Player CreatePlayer(PlayerType type)
    {
        GameObject playerObject = new GameObject("Player");
        Player p;
        switch (type)
        {
            default:
            case PlayerType.Human:
                p = playerObject.AddComponent<HumanPlayer>();
                break;
            case PlayerType.AI:
                p = playerObject.AddComponent<AiPlayer>();
                break;
        }
        p.SetTile(tiles[playerIndex++]);
        return p;
    }
}


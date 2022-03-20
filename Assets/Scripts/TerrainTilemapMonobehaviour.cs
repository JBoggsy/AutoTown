using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainTilemapMonobehaviour : MonoBehaviour
{
    public Tile[] Tiles;

    public void Start()
    {
        Tilemap tile_map = gameObject.GetComponent<Tilemap>();
        
        int width = 20;
        int height = 20;
        int radius = 5;
        Tile tile;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int location = new Vector3Int(x - width / 2, y - height / 2, 0);

                if (location.magnitude < radius)
                {
                    tile = Tiles[1];
                }
                else
                {
                    tile = Tiles[5];
                }

                tile_map.SetTile(location, tile);
            }
        }
    }
}

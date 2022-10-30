using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainTilemapMonobehaviour : MonoBehaviour
{
    public Tile GrassTile;
    public Tile DirtTile;
    public Tile RockTile;
    public Tile IceTile;
    public Tile WaterShallowTile;
    public Tile WaterDeepTile;

    private TownSceneManager TownSceneManagerObject;
    private Tilemap TilemapGameObject;
    private RegionModel Region;
    private bool NeedsRefresh = true;

    public void Start()
    {
        TownSceneManagerObject = TownSceneManager.Instance;
        Region = TownSceneManagerObject.Region;
        TilemapGameObject = gameObject.GetComponent<Tilemap>();
    }

    public void Update()
    {
        if (NeedsRefresh) { Refresh(); }
    }

    public void Refresh()
    {
        Vector3Int tile_coordinate;
        print(Region);
        for (int y=0; y<Region.Height; y++)
        {
            for (int x=0; x<Region.Width; x++)
            {
                tile_coordinate = new Vector3Int(x, y, 0);
                switch (Region.GetTerrainAt(tile_coordinate))
                {
                    case TerrainType.Grass:
                        TilemapGameObject.SetTile(tile_coordinate, GrassTile);
                        break;
                    case TerrainType.Dirt:
                        TilemapGameObject.SetTile(tile_coordinate, DirtTile);
                        break;
                    case TerrainType.Rock:
                        TilemapGameObject.SetTile(tile_coordinate, RockTile);
                        break;
                    case TerrainType.Ice:
                        TilemapGameObject.SetTile(tile_coordinate, IceTile);
                        break;
                    case TerrainType.Water_Shallow:
                        TilemapGameObject.SetTile(tile_coordinate, WaterShallowTile);
                        break;
                    case TerrainType.Water_Deep:
                        TilemapGameObject.SetTile(tile_coordinate, WaterDeepTile);
                        break;
                }
            }
        }

        NeedsRefresh = false;
    }
}

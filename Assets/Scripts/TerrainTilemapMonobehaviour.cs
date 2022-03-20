using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainTilemapMonobehaviour : MonoBehaviour
{
    public Tile[] Tiles;

    private TownSceneManager TownSceneManagerObject;
    private Tilemap TilemapGameObject;
    private bool NeedsRefresh = true;

    public void Start()
    {
        TownSceneManagerObject = TownSceneManager.Instance;
        TilemapGameObject = gameObject.GetComponent<Tilemap>();
    }

    public void Update()
    {
        if (NeedsRefresh) { Refresh(); }
    }

    public void Refresh()
    {
    }
}

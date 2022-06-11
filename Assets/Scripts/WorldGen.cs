using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGen
{
    public struct ResourceDeposit
    {
        public int Quantity;
        public Vector2Int Location;
        public ResourceDepositType Type;

        public ResourceDeposit(int quantity, Vector2Int location, ResourceDepositType type)
        {
            Quantity = quantity;
            Location = location;
            Type = type;
        }
    }

    public static void Create(int width, int height, int seed, out TerrainType[,] terrain, out List<ResourceDeposit> deposits)
    {
        deposits = new List<ResourceDeposit>();

        Random.InitState(seed);
        terrain = new TerrainType[width, height];


        float island_radius = Mathf.Min(width / 2, height / 2);
        float clearing_altitude = 0.5f;
        int min_resources = 100;
        int max_resources = 1000;
        float scale = 1.0f;
        float resouce_scarcity = 0.6f;

        List<(float freq, float mag)> fourier_series_topogrophy = new List<(float, float)>()
        {
            (0.100f * scale, 0.1f),
            (0.050f * scale, 0.2f),
            (0.010f * scale, 0.4f),
            (0.005f * scale, 0.2f)
        };

        List<(float freq, float mag)> fourier_series_resources = new List<(float, float)>()
        {
            (0.20f * scale, 0.1f),
            (0.10f * scale, 0.2f),
            (0.05f * scale, 0.3f)
        };

        float offset_topogrophy = Random.Range(0f, 10000f);
        float offset_trees = Random.Range(0f, 10000f);
        float offset_rocks = Random.Range(0f, 10000f);

        float altitude_ocean = 0.35f;
        float altitude_water = 0.40f;
        float altitude_grass = 0.65f;
        float altitude_rocks = 1.00f;

        AnimationCurve island_dropoff = new AnimationCurve();
        island_dropoff.AddKey(new Keyframe(0.0f, altitude_rocks));
        island_dropoff.AddKey(new Keyframe(0.7f, altitude_rocks));
        island_dropoff.AddKey(new Keyframe(1.0f, altitude_ocean));

        AnimationCurve center_clearing = new AnimationCurve();
        center_clearing.AddKey(new Keyframe(0.00f, 1.0f));
        center_clearing.AddKey(new Keyframe(0.10f, 1.0f));
        center_clearing.AddKey(new Keyframe(0.20f, 0.0f));
        center_clearing.AddKey(new Keyframe(1.00f, 0.0f));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float altitude = Util.PerlinNoiseMultisample(x, y, fourier_series_topogrophy, offset_topogrophy);
                float distance = (new Vector3Int(x - width / 2, y - height / 2, 0)).magnitude;
                altitude *= island_dropoff.Evaluate(distance / island_radius);
                float clearing_weight = center_clearing.Evaluate(distance / island_radius);
                altitude = Mathf.Lerp(altitude, clearing_altitude, clearing_weight);

                TerrainType terrain_type;
                if (altitude < altitude_ocean)
                {
                    terrain_type = TerrainType.Water_Deep;
                }
                else if (altitude < altitude_water)
                {
                    terrain_type = TerrainType.Water_Shallow;
                }
                else if (altitude < altitude_grass)
                {
                    terrain_type = TerrainType.Grass;
                }
                else
                {
                    terrain_type = TerrainType.Rock;
                }

                terrain[x, y] = terrain_type;

                if (terrain_type != TerrainType.Grass) { continue; }

                float rocks = Util.PerlinNoiseMultisample(x, y, fourier_series_resources, offset_rocks);
                float trees = Util.PerlinNoiseMultisample(x, y, fourier_series_resources, offset_trees);
                rocks = Mathf.Lerp(rocks, 0, clearing_weight);
                trees = Mathf.Lerp(trees, 0, clearing_weight);

                ResourceDepositType resource_type;
                float richness;
                if (rocks > resouce_scarcity)
                {
                    resource_type = ResourceDepositType.Rock;
                    richness = rocks;
                }
                else if (trees > resouce_scarcity)
                {
                    resource_type = ResourceDepositType.Tree;
                    richness = trees;
                }
                else
                {
                    continue;
                }

                richness = (richness - resouce_scarcity) / (1 - resouce_scarcity);
                int quantity = (int)(richness * (max_resources - min_resources)) + min_resources;

                deposits.Add(new ResourceDeposit(quantity, new Vector2Int(y, x), resource_type));
            }
        }
    }
}

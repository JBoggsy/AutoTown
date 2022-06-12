using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGen
{
    public class FourierSeries : List<(float, float)>
    {
        public List<(float Freq, float Mag)> Components;
    }

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

    public struct Parameters
    {
        public int Width;
        public int Height;

        public float Scale;
        public float ResourceScarcity;
        public float ClearingAltitude;
        public int MinResources;
        public int MaxResources;

        public float AltitudeOcean;
        public float AltitudeShallows;
        public float AltitudeGrass;
        public float AltitudeMountain;

        public FourierSeries TopogrophyProfile;
        public FourierSeries ResourceProfile;

        public AnimationCurve IslandDropoff;
        public AnimationCurve CenterClearing;

        public static Parameters Default()
        {
            Parameters p = new Parameters();
            
            p.Width = 200;
            p.Height = 200;

            p.Scale = 1.0f;
            p.ResourceScarcity = 0.6f;
            p.ClearingAltitude = 0.5f;
            p.MinResources = 100;
            p.MaxResources = 1000;

            p.AltitudeOcean = 0.35f;
            p.AltitudeShallows = 0.40f;
            p.AltitudeGrass = 0.65f;
            p.AltitudeMountain = 1.00f;

            p.TopogrophyProfile = new FourierSeries
            {
                (0.100f * p.Scale, 0.1f),
                (0.050f * p.Scale, 0.2f),
                (0.010f * p.Scale, 0.4f),
                (0.005f * p.Scale, 0.2f)
            };
            p.ResourceProfile = new FourierSeries
            {
                (0.20f * p.Scale, 0.1f),
                (0.10f * p.Scale, 0.2f),
                (0.05f * p.Scale, 0.3f)
            };

            p.IslandDropoff = new AnimationCurve(
                new Keyframe(0.0f, p.AltitudeMountain),
                new Keyframe(0.7f, p.AltitudeMountain),
                new Keyframe(1.0f, p.AltitudeOcean)
            );
            p.CenterClearing = new AnimationCurve(
                new Keyframe(0.00f, 1.0f),
                new Keyframe(0.10f, 1.0f),
                new Keyframe(0.20f, 0.0f),
                new Keyframe(1.00f, 0.0f)
            );

            return p;
        }
    }

    public static void Create(Parameters p, int seed, out TerrainType[,] terrain, out List<ResourceDeposit> deposits)
    {
        terrain = new TerrainType[p.Width, p.Height];
        deposits = new List<ResourceDeposit>();

        Random.InitState(seed);

        float island_radius = Mathf.Min(terrain.GetLength(0) / 2, terrain.GetLength(1) / 2);

        float offset_topogrophy = Random.Range(0f, 10000f);
        float offset_trees = Random.Range(0f, 10000f);
        float offset_rocks = Random.Range(0f, 10000f);

        for (int x = 0; x < terrain.GetLength(0); x++)
        {
            for (int y = 0; y < terrain.GetLength(1); y++)
            {
                float altitude = Util.PerlinNoiseMultisample(x, y, p.TopogrophyProfile, offset_topogrophy);
                float distance = (new Vector3Int(x - terrain.GetLength(0) / 2, y - terrain.GetLength(1) / 2, 0)).magnitude;
                float clearing_weight = p.CenterClearing.Evaluate(distance / island_radius);
                altitude *= p.IslandDropoff.Evaluate(distance / island_radius);
                altitude = Mathf.Lerp(altitude, p.ClearingAltitude, clearing_weight);

                TerrainType terrain_type;
                if (altitude < p.AltitudeOcean)
                {
                    terrain_type = TerrainType.Water_Deep;
                }
                else if (altitude < p.AltitudeShallows)
                {
                    terrain_type = TerrainType.Water_Shallow;
                }
                else if (altitude < p.AltitudeGrass)
                {
                    terrain_type = TerrainType.Grass;
                }
                else
                {
                    terrain_type = TerrainType.Rock;
                }

                terrain[x, y] = terrain_type;

                if (terrain_type != TerrainType.Grass) { continue; }

                float rocks = Util.PerlinNoiseMultisample(x, y, p.ResourceProfile, offset_rocks);
                float trees = Util.PerlinNoiseMultisample(x, y, p.ResourceProfile, offset_trees);
                rocks = Mathf.Lerp(rocks, 0, clearing_weight);
                trees = Mathf.Lerp(trees, 0, clearing_weight);

                ResourceDepositType resource_type;
                float richness;
                if (rocks > p.ResourceScarcity)
                {
                    resource_type = ResourceDepositType.Rock;
                    richness = rocks;
                }
                else if (trees > p.ResourceScarcity)
                {
                    resource_type = ResourceDepositType.Tree;
                    richness = trees;
                }
                else
                {
                    continue;
                }

                richness = (richness - p.ResourceScarcity) / (1 - p.ResourceScarcity);
                int quantity = (int)(richness * (p.MaxResources - p.MinResources)) + p.MinResources;

                deposits.Add(new ResourceDeposit(quantity, new Vector2Int(y, x), resource_type));
            }
        }
    }
}

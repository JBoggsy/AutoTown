using UnityEngine;

public class Percept
{
    public float Health { get; set; }
    public Vector3Int Position { get; set; }
    public RegionModel Region { get; set; }

    public Percept(AgentEntity agent)
    {
        this.Health = agent.Health;
        this.Position = agent.Position;
    }
}


public class ManualControlPercept : Percept
{
    public ManualControlPercept(AgentEntity agent) : base(agent)
    {
        this.Health = agent.Health;
        this.Position = agent.Position;

    }
}
public abstract class AgentControlInterface
{
    protected AgentEntity agentEntity;

    public AgentControlInterface(AgentEntity agentEntity)
    {
        this.agentEntity = agentEntity;
    }

    public abstract Action DecideNextAction(Percept percept);
}
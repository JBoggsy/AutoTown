using UnityEngine;

/**
 * Super-class for all entities that exist in the world, including resource deposits, items, people, etc.
 */
public abstract class WorldEntity
{
    protected const bool is_obstacle = true;
    public Vector3Int Position { get; protected set; }
    public float Health { get; protected set; }
    protected float MaxHealth { get; set; }

    public bool IsObstacle() { return is_obstacle; }

    /// <summary>
    /// Apply raw damage dealt to the world entity. Input is the unmodified damage dealt, the
    /// concrete class implementing this method should handle applying modifications such as 
    /// resistances or weaknesses.
    /// </summary>
    /// <param name="damage">Unmodified raw damage dealt</param>
    /// <returns>True if the damage dealt is greater than the remaining health pool of the entity</returns>
    public abstract bool ApplyDamage(float damage);

    /// <summary>
    /// Apply raw healing to the world entity. Input is the unmodified health returned, and the 
    /// concrete class implementing this method should handle applying modifications.
    /// </summary>
    /// <param name="damage">Unmodified raw damage dealt</param>
    /// <returns>True if the entity is restored to full health</returns>
    public abstract bool ApplyHeal(float heal);

    public abstract void Destroy(); 
}

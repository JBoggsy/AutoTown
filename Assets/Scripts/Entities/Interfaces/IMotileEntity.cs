using UnityEngine;

public interface IMotileEntity
{
    public bool Move(Vector3Int direction);
    public bool Translate(Vector3Int translation);
}
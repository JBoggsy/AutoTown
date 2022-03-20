using UnityEngine;

public class TownSceneManager : MonoBehaviour
{
    public static TownSceneManager Instance;
    public RegionModel Region { get; private set; }

    public void Awake()
    {
        Region = new RegionModel(50, 50);
        Instance = this;
    }
}

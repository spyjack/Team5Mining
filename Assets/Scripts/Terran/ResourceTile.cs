using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile", menuName = "Terrain Tiles/Tile", order = 51)]
public class ResourceTile : Tile
{
    //[SerializeField]
    //Vector3Int tileTransform = new Vector3Int();

    [SerializeField]
    int health = 1;

    [SerializeField]
    ResourceId resourceType = ResourceId.Dirt;

    [SerializeField]
    int hardness = 0;

    [SerializeField]
    bool isDestructible = true;

    [SerializeField]
    GameObject destructionOverlay = null;

    public int Health
    {
        get { return health; }
    }

    public int Hardness
    {
        get { return hardness; }
    }

    public ResourceId Resource
    {
        get { return resourceType; }
        set { resourceType = value; }
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        //tileTransform = position;
        //Debug.Log(tileTransform);
        if (destructionOverlay != null)
        {
            go = destructionOverlay;
        }
        return base.StartUp(position, tilemap, go);
    }

    public void TakeDamage(int damage)
    {
        if (isDestructible)
        {
            health -= damage;
            if (health <= 0)
            {
                Debug.Log("died");
            }
        }
    }

    public void SetHealth(int _health)
    {
        health = _health;
    }
}

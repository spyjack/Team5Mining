using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile", menuName = "Terrain Tiles/Tile", order = 51)]
public class ResourceTile : Tile
{
    [SerializeField]
    Vector3Int tileTransform = new Vector3Int();

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

    [SerializeField]
    GameObject glowObject = null;

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
        tileTransform = position;
        //Debug.Log(tileTransform);
        if (destructionOverlay != null)
        {
            go = destructionOverlay;
        }
        if (glowObject != null)
        {
            Debug.Log("Spawn Light");
            Vector3 spawnPos = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tilemap>().CellToWorld(position);
            spawnPos.z -= 3;
            Instantiate(glowObject, spawnPos, Quaternion.identity);
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

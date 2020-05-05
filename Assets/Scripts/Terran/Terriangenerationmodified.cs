using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Terriangenerationmodified : MonoBehaviour
{
    [SerializeField]
    List<TerrainProbabilities> terrainInits = new List<TerrainProbabilities>();

    private int count = 0;

    private int[,] terrainMap;
    private int[,] ironMap;
    private int[,] copperMap;
    private int[,] goldMap;
    private int[,] crystalMap;
    private int[,] leadMap;
    private int[,] aluminumMap;
    private int[,] coalMap;
    private int[,] hardstoneMap;
    public Vector3Int tmpSize;
    public Tilemap topMap;
    public Tilemap botMap;
    public TerrainTile topTile;
    public TerrainTile coalTile;
    public AnimatedTile botTile;

    [SerializeField]
    private List<ResourceTile> terrainTilesList;

    [SerializeField]
    Sprite testSprite = null;

    int width;
    int height;

    //public GameObject loadingScreen;


    public void Start()
    {
        doSim();
    }


    public void doSim()
    {
        clearMap(false);
        width = tmpSize.x;
        height = tmpSize.y;

        int ironAmount = 0;
        int coalAmount = 0;
        int copperAmount = 0;
        int goldAmount = 0;
        int crystalsAmount = 0;
        int aluminumAmount = 0;
        int leadAmount = 0;

        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            initPos(terrainInits[0], terrainMap);
        }
        if (ironMap == null)
        {
            ironMap = new int[width, height];
            initPos(terrainInits[1], ironMap);
        }
        if (copperMap == null)
        {
            copperMap = new int[width, height];
            initPos(terrainInits[2], copperMap);
        }
        if (goldMap == null)
        {
            goldMap = new int[width, height];
            initPos(terrainInits[3], goldMap);
        }
        if (crystalMap == null)
        {
            crystalMap = new int[width, height];
            initPos(terrainInits[6], crystalMap);
        }
        if (leadMap == null)
        {
            leadMap = new int[width, height];
            initPos(terrainInits[4], leadMap);
        }
        if (aluminumMap == null)
        {
            aluminumMap = new int[width, height];
            initPos(terrainInits[5], aluminumMap);
        }
        if (coalMap == null)
        {
            coalMap = new int[width, height];
            initPos(terrainInits[7], coalMap);
        }

        foreach (TerrainProbabilities _terrainProbs in terrainInits)
        {
            for (int i = 0; i < _terrainProbs.iniChance; i++)
            {
                terrainMap = genTilePos(terrainMap, terrainInits[0]);
                ironMap = genTilePos(ironMap, terrainInits[1]);
                copperMap = genTilePos(copperMap, terrainInits[2]);
                goldMap = genTilePos(goldMap, terrainInits[3]);
                coalMap = genTilePos(coalMap, terrainInits[7]);
                crystalMap = genTilePos(crystalMap, terrainInits[6]);
                aluminumMap = genTilePos(aluminumMap, terrainInits[5]);
                leadMap = genTilePos(leadMap, terrainInits[4]);
            }
        }
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (terrainMap[x, y] == 1 && y < Random.Range(15,25))
                {//You were missing these curly braces, so the if statement only set the top tile as it was the first below the if statement.
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[0]));
                    
                }else if (terrainMap[x, y] == 1)
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[1]));
                }
                if (coalMap[x, y] == 0 && y > Random.Range(10, 25))
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[8]));
                    coalAmount++;
                }
                if (ironMap[x,y] == 1 && coalMap[x, y] == 1 && y > 20)
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[2]));
                    ironAmount++;
                }
                if (copperMap[x, y] == 1 && ironMap[x, y] == 0 && coalMap[x, y] == 1 && y > Random.Range(15, 35))
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[3]));
                    copperAmount++;
                }
                if (goldMap[x, y] == 1 && y > Random.Range(45, 55))
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[4]));
                    goldAmount++;
                }
                if (leadMap[x, y] == 1 && y > Random.Range(55, 65))
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[7]));
                    leadAmount++;
                }
                if (aluminumMap[x, y] == 1 && y > Random.Range(85, 95))
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[6]));
                    aluminumAmount++;
                }
                if (crystalMap[x, y] == 1 && y > Random.Range(100, 105))
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[5]));
                    crystalsAmount++;
                }
                if (y >= Random.Range(height - 5, height - 1))
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), CreateTile(terrainTilesList[9]));

                botMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), botTile);
                
            }
        }

        Debug.Log("Generated " + coalAmount + " Coal");
        Debug.Log("Generated " + ironAmount + " Iron Ore");
        Debug.Log("Generated " + copperAmount + " Copper Ore");
        Debug.Log("Generated " + goldAmount + " Gold Ore");
        Debug.Log("Generated " + crystalsAmount + " Rare Crystals");
        Debug.Log("Generated " + aluminumAmount + " Aluminum Ore");
        Debug.Log("Generated " + leadAmount + " Lead Ore");
    }

    public ResourceTile CreateTile(ResourceTile _baseTile)
    {
        ResourceTile _newTile = ScriptableObject.CreateInstance<ResourceTile>();
        _newTile.sprite = _baseTile.sprite;
        _newTile.SetHealth(_baseTile.Health);
        _newTile.Resource = _baseTile.Resource;
        _newTile.glowObject = _baseTile.glowObject;

        return _newTile;
    }

    public void initPos(TerrainProbabilities _terrainInits, int[,] _map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Random.Range(1, 101) < _terrainInits.iniChance)
                {
                    _map[x, y] = 1;
                }else
                {
                    _map[x, y] = 0;
                }
            }

        }

    }


    public int[,] genTilePos(int[,] oldMap, TerrainProbabilities _terrainInits)
    {
        int[,] newMap = new int[width, height];
        int neighb;
        BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                neighb = 0;
                foreach (var b in myB.allPositionsWithin)
                {
                    if (b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y + b.y < height)
                    {
                        neighb += oldMap[x + b.x, y + b.y];
                    }
                    else
                    {
                        neighb++;
                    }
                }

                if (oldMap[x, y] == 1)
                {
                    if (neighb < _terrainInits.deathLimit) newMap[x, y] = 0;

                    else
                    {
                        newMap[x, y] = 1;

                    }
                }

                if (oldMap[x, y] == 0)
                {
                    if (neighb > _terrainInits.birthLimit) newMap[x, y] = 1;

                    else
                    {
                        newMap[x, y] = 0;
                        
                    }
                }

            }

        }


        
        return newMap;
    }

    public void clearMap(bool complete)
    {

        topMap.ClearAllTiles();
        botMap.ClearAllTiles();
        if (complete)
        {
            terrainMap = null;
        }


    }



}

[System.Serializable]
public struct TerrainProbabilities
{
    public ResourceId resourceType;


    [Range(0, 100)]
    public int iniChance;
    [Range(1, 16)]
    public int birthLimit;
    [Range(1, 16)]
    public int deathLimit;

    [Range(1, 10)]
    public int iterations;
}

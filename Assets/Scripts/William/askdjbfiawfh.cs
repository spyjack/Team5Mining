using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class askdjbfiawfh : MonoBehaviour
{
    public Tilemap tilemap;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cube")
        {
            tilemap.SetTile(new Vector3Int(-7, 3, 0), null);
            print("HIT THE GROUND");
        }
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in collision.contacts)
        {
            Debug.Log(hit.point);
            hitPosition.x = hit.point.x - 0.1f;
            hitPosition.y = hit.point.y - 0.1f;
            tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        }
    }
}

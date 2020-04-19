using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ShipDrilling : MonoBehaviour
{
    public Tilemap tilemap;
    /*public void OnTriggerEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in collision.contacts)
        {
            Debug.Log(hit.point);
            hitPosition.x = hit.point.x - 0.1f;
            hitPosition.y = hit.point.y - 0.1f;
            tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        }
    }
    */
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in collision.contacts)
        {
            Debug.Log(hit.point);
            hitPosition.x = hit.point.x - 0f;
            hitPosition.y = hit.point.y - 0f;
            tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        }
    }

    /*
public void OnTriggerEnter2D(Collision2D coll)
{
    Vector3 hitPosition = Vector3.zero;
    RaycastHit hit;
    Ray drillingRay = new Ray(transform.position, Vector3.down);
    if (Physics.Raycast(drillingRay, out hit, 100))
    {
        if (hit.collider.tag == "Ground")
        {
            Debug.Log(hit.point);
            hitPosition.x = hit.point.x - 0.1f;
            hitPosition.y = hit.point.y - 0.1f;
            tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        }
    }

}
*/

    public void FixedUpdate()
    {
        Vector2 hitPos = Vector2.zero;
        RaycastHit hit;
        Ray drillingRay = new Ray(transform.position, Vector2.right);
        Debug.DrawLine(transform.position, Vector2.right);
        if(Physics.Raycast(drillingRay,out hit,1 ))
        {
            if(hit.collider.tag == "Ground")
            {
                tilemap.SetTile(tilemap.WorldToCell(hitPos), null);
            }
        }
    }
}

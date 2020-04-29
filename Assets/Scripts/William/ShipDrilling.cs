using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ShipDrilling : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject drillingParticles;
    public Vector3 drillBitPos;
    public GameObject drill;

    [SerializeField]
    public CircleCollider2D drillCollider = null;

    [SerializeField]
    VehicleClass vehicleMain = null;

    [SerializeField]
    VehicleMovement vehicleMover = null;

    //Added object pooling as it currently instantiates waaay too many particles - Terran
    [SerializeField]
    List<GameObject> drillParticlesPooled = new List<GameObject>();

    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in collision.contacts)
        {
            Debug.Log(hit.point);

            hitPosition.x = hit.point.x;
            hitPosition.y = hit.point.y;

            hitPosition.x = hit.point.x;
            hitPosition.y = hit.point.y;

            ResourceTile _tile = tilemap.GetTile<ResourceTile>(tilemap.WorldToCell(hitPosition));
            _tile.TakeDamage(1);
            print("Tile Health " + _tile.Health);
            
            //tilemap.GetTile<ResourceTile>(tilemap.WorldToCell(hitPosition)).TakeDamage(5);
            //tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);

            //Code for Particle system spawning.
            CreateParticle(this.transform.position);
            drill.transform.parent = drill.transform;
            //Consume a fuel with every block mined
            vehicleMain.UseFuel(-1);
            //Add a resource
            vehicleMain.Inventory.AddRandomResource(1);
        }
        GameObject.FindObjectOfType<PlayerController>().dirtyNav = true;
    }

    

    void CreateParticle(Vector2 position)
    {
        GameObject particle = GetPooledParticle();
        particle.SetActive(true);
        particle.transform.position = position;
        particle.GetComponent<ParticleSystem>().Play();
    }

    GameObject GetPooledParticle()
    {
        for (int i = 0; i < drillParticlesPooled.Count; i++)
        {
            if (!drillParticlesPooled[i].activeInHierarchy)
            {
                return drillParticlesPooled[i];
            }
        }
        //If the loop doesn't return an object, then it'll continue down here and create a new particle
        GameObject newParticle = Instantiate(drillingParticles, transform.position, Quaternion.identity);
        newParticle.SetActive(false);
        drillParticlesPooled.Add(newParticle);
        StopCoroutine(DisablePooledParticles());
        StartCoroutine(DisablePooledParticles());
        return newParticle;
    }

    IEnumerator DisablePooledParticles()
    {
        while(true)
        {
            for (int i = 0; i < drillParticlesPooled.Count; i++)
            {
                if (drillParticlesPooled[i].activeInHierarchy && drillParticlesPooled[i].GetComponent<ParticleSystem>().isStopped)
                {
                    drillParticlesPooled[i].SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

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

    /*public void FixedUpdate()
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
    */
}

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
    LayerMask miningLayerMask = new LayerMask();

    [SerializeField]
    public CircleCollider2D drillCollider = null;

    [SerializeField]
    VehicleClass vehicleMain = null;

    [SerializeField]
    VehicleMovement vehicleMover = null;

    [SerializeField]
    bool drillActive = false;

    public bool ActiveDrill
    {
        get { return drillActive; }
    }

    //Added object pooling as it currently instantiates waaay too many particles - Terran
    [SerializeField]
    List<GameObject> drillParticlesPooled = new List<GameObject>();

    private void OnEnable()
    {
        if (tilemap == null)
            FindTilemap();
    }

    public void FindTilemap()
    {
        foreach (Tilemap map in FindObjectsOfType<Tilemap>())
        {
            if (map.transform.tag == "Ground")
            {
                tilemap = map;
                return;
            }
        }
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!drillActive && other.tag == "Ground" && vehicleMain.Inventory.GetFuelAmount() > 0)
        {
            StartCoroutine(DrillRepeated());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground" && vehicleMover.GetTarget(0).z != 1)
        {
            drillActive = false;
            vehicleMain.Audio.StopAudio(VehicleSounds.Drilling);
            StopCoroutine(DrillRepeated());
        }
    }

    IEnumerator DrillRepeated()
    {
        if (drillActive || !vehicleMover.IsMining)
        {
            vehicleMain.Audio.StopAudio(VehicleSounds.Drilling);
            yield break;
        }
        int _hardnessCount = 0;
        while (true)
        {
            float gasLeft = vehicleMain.Inventory.GetFuelAmount();
            if (!drillActive && vehicleMover.IsMining)
            {
                drillActive = true;
            }
            else if (!vehicleMover.IsMining || gasLeft <= 0)
            {
                vehicleMain.Audio.StopAudio(VehicleSounds.Drilling);
                drillActive = false;
                yield break;
            }
            _hardnessCount = 0;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, drillCollider.radius, transform.forward, drillCollider.radius, miningLayerMask);
            if (hits.Length > 0)
            {
                print(hits[0].transform.gameObject.layer);
                bool hasChanged = false;
                foreach (RaycastHit2D hit in hits)
                {
                    //print("DrillRepeated");
                    Vector2 hitPosition = new Vector2();

                    hitPosition.x = hit.point.x;
                    hitPosition.y = hit.point.y;

                    hitPosition.x = hit.point.x;
                    hitPosition.y = hit.point.y;

                    if (tilemap.GetTile(tilemap.WorldToCell(hitPosition)) != null)
                    {
                        ResourceTile _tile = tilemap.GetTile<ResourceTile>(tilemap.WorldToCell(hitPosition));
                        if (_tile.Hardness <= vehicleMain.DrillTier)
                        {
                            int damage = 5;
                            float collectionBonus = 1;
                            vehicleMain.Audio.PlayAudio(VehicleSounds.Drilling);
                            WorkerBase _drillOperater = vehicleMain.GetWorker(WorkStation.Drill);
                            if (_drillOperater != null)
                            {
                                damage += _drillOperater.Operating;
                                collectionBonus = (float)_drillOperater.Operating * 0.25f;
                            }
                            if (vehicleMain.GetWorker(WorkStation.Spare) != null)
                                damage += Mathf.RoundToInt((float)vehicleMain.GetWorker(WorkStation.Spare).Operating / 2f);
                            _tile.TakeDamage(damage);
                            //print("Tile Health " + _tile.Health);
                            if (_tile.Health <= 0)
                            {
                                //Add a resource
                                vehicleMain.Inventory.AddResource(_tile.Resource, (1 * vehicleMain.DrillEfficiency) + collectionBonus);
                                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                                hasChanged = true;
                            }
                        }
                        else
                        {
                            _hardnessCount++;
                        }
                        vehicleMain.UseFuel(-1 * vehicleMain.DrillFuelEfficiency);
                    }

                    //Code for Particle system spawning.
                    //CreateParticle(this.transform.position);
                    drill.transform.parent = drill.transform;
                    //Consume a fuel with every block mined

                    if (_hardnessCount >= hits.Length)
                    {
                        Debug.LogWarning("Soil is too hard");
                        vehicleMain.Audio.StopAudio(VehicleSounds.Drilling);
                        drillActive = false;
                        yield break;
                    }
                }
                if (hasChanged)
                    GameObject.FindObjectOfType<PlayerController>().dirtyNav = true;
            }
            else
            {
                vehicleMain.Audio.StopAudio(VehicleSounds.Drilling);
                drillActive = false;
                yield break;
            }
            yield return new WaitForSeconds(vehicleMain.DrillSpeed);
        }
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
        while (true)
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
}
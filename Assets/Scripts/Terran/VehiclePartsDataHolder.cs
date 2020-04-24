using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehiclePartsDataHolder : MonoBehaviour
{
    [Header("Cabin Parts")]
    [SerializeField]
    List<PartCabin> partsCabinList = new List<PartCabin>();

    [Header("Drill Parts")]
    [SerializeField]
    List<PartDrill> partsDrillList = new List<PartDrill>();

    [Header("Engine Parts")]
    [SerializeField]
    List<PartEngine> partsEngineList = new List<PartEngine>();

    [Header("Wheel Parts")]
    [SerializeField]
    List<PartWheel> partsWheelList = new List<PartWheel>();

    [Header("Upgrade Parts")]
    [SerializeField]
    List<PartUpgrade> partsUpgradeList = new List<PartUpgrade>();

    public PartBase GetRandomPart()
    {
        int randPartList = Random.Range(0, 4);
        switch(randPartList)
        {
            case 0:
                return partsCabinList[Random.Range(0, partsCabinList.Count)];
            case 1:
                return partsDrillList[Random.Range(0, partsCabinList.Count)];
            case 2:
                return partsEngineList[Random.Range(0, partsCabinList.Count)];
            case 3:
                return partsWheelList[Random.Range(0, partsCabinList.Count)];
            case 4:
                return partsUpgradeList[Random.Range(0, partsCabinList.Count)];
            default:
                return partsCabinList[Random.Range(0, partsCabinList.Count)];
        }

    }
}

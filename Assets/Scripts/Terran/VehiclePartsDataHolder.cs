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

    [Header("Vehicle Base")]
    [SerializeField]
    List<PartBody> vehicleBaseParts = new List<PartBody>();

    [Header("Ship Names")]
    [SerializeField]
    List<string> shipNames = new List<string>();

    public List<PartBody> BodyTypeList
    {
        get { return vehicleBaseParts; }
    }
    public List<PartCabin> CabinList
    {
        get { return partsCabinList; }
    }
    public List<PartDrill> DrillList
    {
        get { return partsDrillList; }
    }
    public List<PartEngine> EngineList
    {
        get { return partsEngineList; }
    }
    public List<PartWheel> WheelsList
    {
        get { return partsWheelList; }
    }
    public List<PartUpgrade> UpgradesList
    {
        get { return partsUpgradeList; }
    }
    public List<string> ShipNames
    {
        get { return shipNames; }
    }

    private void Start()
    {
        WorkerDataHolder.ReadString("ShipNames.txt", shipNames);
    }

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

    public void GetRandomPart(TierProbability _tierChances, out PartBody _part)
    {
        int _tierProb = _tierChances.GetRandomTier();
        switch (_tierProb)
        {
            case 1:
                _part = vehicleBaseParts[0];
                return;
            case 2:
                _part = vehicleBaseParts[1];
                return;
            case 3:
                _part = vehicleBaseParts[2];
                return;
        }
        _part = null;
    }

    public void GetRandomPart(TierProbability _tierChances, out PartCabin _part)
    {
        int _tierProb = _tierChances.GetRandomTier();
        switch (_tierProb)
        {
            case 1:
                _part = partsCabinList[0];
                return;
            case 2:
                _part = partsCabinList[0];
                return;
            case 3:
                _part = partsCabinList[0];
                return;
        }
        _part = null;
    }

    public void GetRandomPart(TierProbability _tierChances, out PartDrill _part)
    {
        int _tierProb = _tierChances.GetRandomTier();
        switch (_tierProb)
        {
            case 1:
                _part = partsDrillList[0];
                return;
            case 2:
                _part = partsDrillList[1];
                return;
            case 3:
                _part = partsDrillList[2];
                return;
        }
        _part = null;
    }

    public void GetRandomPart(TierProbability _tierChances, out PartEngine _part)
    {
        int _tierProb = _tierChances.GetRandomTier();
        switch (_tierProb)
        {
            case 1:
                _part = partsEngineList[0];
                return;
            case 2:
                _part = partsEngineList[0];
                return;
            case 3:
                _part = partsEngineList[0];
                return;
        }
        _part = null;
    }

    public void GetRandomPart(TierProbability _tierChances, out PartWheel _part)
    {
        int _tierProb = _tierChances.GetRandomTier();
        switch (_tierProb)
        {
            case 1:
                _part = partsWheelList[0];
                return;
            case 2:
                _part = partsWheelList[0];
                return;
            case 3:
                _part = partsWheelList[0];
                return;
        }
        _part = null;
    }

    public void GetRandomPart(TierProbability _tierChances, out PartUpgrade _part)
    {
        int _tierProb = _tierChances.GetRandomTier();
        switch (_tierProb)
        {
            case 1:
                _part = partsUpgradeList[0];
                return;
            case 2:
                _part = partsUpgradeList[0];
                return;
            case 3:
                _part = partsUpgradeList[0];
                return;
        }
        _part = null;
    }
}

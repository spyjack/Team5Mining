using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    PlayerController player = null;

    [SerializeField]
    List<VehicleClass> dockedShips = new List<VehicleClass>();

    [Header("Part Market")]
    [SerializeField]
    Transform vehicleCardHolder = null;

    [SerializeField]
    GameObject vehicleCardPrefab = null;

    [SerializeField]
    List<Transform> partInfoItems = new List<Transform>();

    [SerializeField]
    GameObject partItemPrefab = null;

    [SerializeField]
    GameObject partInfoPrefab = null;

    [SerializeField]
    GameObject vehicleStatPrefab = null;

    [SerializeField]
    GameObject partInfoHolder = null;

    [SerializeField]
    GameObject partOverviewObj = null;

    [SerializeField]
    List<PartItemConnector> partItemsList = new List<PartItemConnector>();

    int selectedPart = -1;

    [SerializeField]
    GameObject partItemHolder1 = null;

    [SerializeField]
    GameObject partItemHolder2 = null;

    [SerializeField]
    Text partCostText = null;

    [SerializeField]
    Button partBuyButton = null;

    [SerializeField]
    VehiclePartsDataHolder partDataHolder = null;

    [SerializeField]
    Transform shipSpawnPosition = null;

    [SerializeField]
    GameObject vehiclePrefab = null;

    [Header("Worker Store")]
    [SerializeField]
    List<WorkerCardConnector> workerConnectors = new List<WorkerCardConnector>();

    [SerializeField]
    GameObject workerCardPrefab = null;

    [SerializeField]
    Transform workerCardGroup = null;

    [SerializeField]
    WorkerDataHolder workerInfoHolder = null;

    [Header("Store Values")]
    [SerializeField]
    List<ResourceSaleItem> resourceSaleItems = new List<ResourceSaleItem>();

    [Header("Resources Store Variables")]
    [SerializeField]
    VehicleClass selectedVehicle = null;

    [SerializeField]
    int selectedVehicleIndex = -1;

    [SerializeField]
    GameObject shipSelectorPrefab = null;

    [SerializeField]
    GameObject resourceListPrefab = null;

    [Header("Resources Store UI")]

    [SerializeField]
    private List<ShipSelector> shipSelectors = new List<ShipSelector>();

    [SerializeField]
    private List<ResourceListConnector> resourceListItems = new List<ResourceListConnector>();

    [SerializeField]
    private Text currentCapacityText = null;

    [SerializeField]
    private Text maxCapacityText = null;

    [SerializeField]
    private GameObject dockedShipsGroup = null;

    [SerializeField]
    private GameObject resourceListGroup = null;

    [SerializeField]
    private Text helpResourcesText = null;
    [Header("Store General UI")]
    [SerializeField]
    private GameObject marketTab = null;

    [SerializeField]
    private GameObject barracksTab = null;

    [SerializeField]
    private GameObject resourcesTab = null;

    [SerializeField]
    private GameObject openShopButton = null;

    [SerializeField]
    private GameObject closeShopButton = null;

    [SerializeField]
    private GameObject shopObject = null;

    [SerializeField]
    private TierProbability tierProbability = new TierProbability();

    public VehicleClass SelectedShip
    {
        get { return selectedVehicle; }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Vehicle")
        {
            DockShip(other.transform.GetComponent<VehicleClass>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Vehicle")
        {
            UndockShip(other.transform.GetComponent<VehicleClass>());
        }
    }

    public void OnMarketTab()
    {
        if (!marketTab.activeInHierarchy)
        {
            marketTab.SetActive(true);
            resourcesTab.SetActive(false);
            barracksTab.SetActive(false);
            PopulatePartsList();
            selectedPart = -1;
            partOverviewObj.SetActive(false);
            PopulateVehicleCard();
        }
    }

    public void OnBarracksTab()
    {
        if (!barracksTab.activeInHierarchy)
        {
            barracksTab.SetActive(true);
            resourcesTab.SetActive(false);
            marketTab.SetActive(false);
            PopulateWorkerList();
        }
    }

    public void OnResourcesTab()
    {
        if (!resourcesTab.activeInHierarchy)
        {
            resourcesTab.SetActive(true);
            barracksTab.SetActive(false);
            marketTab.SetActive(false);
        }

    }

    public void OnOpenShop()
    {
        openShopButton.SetActive(false);
        closeShopButton.SetActive(true);
        OnMarketTab();
        shopObject.SetActive(true);
        
    }

    public void OnCloseShop()
    {
        openShopButton.SetActive(true);
        closeShopButton.SetActive(false);
        shopObject.SetActive(false);
    }

    public void SelectShipTab(VehicleClass _vehicle)
    {
        if (shipSelectors.Count > 0)
        shipSelectors[selectedVehicleIndex].transform.localScale = new Vector3(1, 1, 1);
        for (int i = 0; i < shipSelectors.Count; i++)
        {
            if (shipSelectors[i].Vehicle == _vehicle)
            {
                selectedVehicleIndex = i;
                shipSelectors[i].transform.localScale = new Vector3(1, 1.1f, 1);
                break;
            }
        }
        selectedVehicle = _vehicle;
        UpdateCapacity();
        CreateResourceList();
    }

    public void SellResource(ResourceId _resource, float _amount)
    {
        if (selectedVehicle.Inventory.CheckForResource(_resource))
        {
            _amount = Mathf.Min(selectedVehicle.Inventory.GetResourceAmount(_resource), _amount);
            player.Money += (int)CalculateValue(_resource, _amount);
            selectedVehicle.Inventory.RemoveResource(_resource, _amount);
            UpdateCapacity();
        }
    }

    public void BuyResource(ResourceId _resource, float _amount)
    {
        if (selectedVehicle.Inventory.UsedCapacity < selectedVehicle.Inventory.Capacity && player.Money - (int)CalculateValue(_resource, _amount, false) >= 0)
        {
            player.Money -= (int)CalculateValue(_resource, _amount, false);
            selectedVehicle.Inventory.AddResource(_resource, _amount);
            UpdateCapacity();
            //print("Added " + _resource + " x" + _amount);
        }
    }

    public void ToggleStore()
    {
        if (shopObject.activeInHierarchy)
        {
            shopObject.SetActive(false);
        } else
        {
            shopObject.SetActive(true);
        }
    }

    public void PurchaseVehicle(VehicleCardConnector _vehicleCard)
    {
        if (player.Money >= _vehicleCard.vehicle.GetCost(_vehicleCard.costMultiplier) && !_vehicleCard.isPurchased)
        {
            player.Money -= _vehicleCard.vehicle.GetCost(_vehicleCard.costMultiplier);
            _vehicleCard.isPurchased = true;
            _vehicleCard.purchaseVehicleButton.interactable = false;
            ColorBlock buttonColors = new ColorBlock();
            buttonColors.normalColor = Color.red;
            buttonColors.disabledColor = Color.red;
            buttonColors.colorMultiplier = 1;
            _vehicleCard.purchaseVehicleButton.colors = buttonColors;
            _vehicleCard.soldOutOverlay.SetActive(true);

            GameObject _newVehicle = Instantiate(vehiclePrefab, shipSpawnPosition.position, Quaternion.identity);
            _newVehicle.GetComponent<VehicleClass>().CreateSelf(_vehicleCard.vehicle);
            player.AddShip(_newVehicle.transform);
        }
    }

    public void PopulateVehicleCard()
    {
        VehicleCardConnector[] vehicleCards = vehicleCardHolder.GetComponentsInChildren<VehicleCardConnector>();
        if (vehicleCards.Length < 1)
        {
            NewVehicleCard(tierProbability);
        }else if (vehicleCards.Length == 1)
        {
            if (vehicleCards[0].isPurchased)
            {
                Destroy(vehicleCards[0].gameObject);
                NewVehicleCard(tierProbability);
            }
        }
    }

    public void AddPartAttribute(string _attributeName, string _attributeValue)
    {
        PartAttributeConnector _newAttribute = Instantiate(partInfoPrefab.transform, partInfoHolder.transform).GetComponent<PartAttributeConnector>();
        _newAttribute.attributeName.text = _attributeName;
        _newAttribute.attributeData.text = _attributeValue;
        partInfoItems.Add(_newAttribute.transform);
    }

    public void ClearPartAttributes()
    {
        for (int i = partInfoItems.Count; i > 0; i--)
        {
            Destroy(partInfoItems[i-1].gameObject);
            partInfoItems.RemoveAt(i-1);
        }
    }

    //If you select an object, turn on the overview and set the correct part. If it is selected already, hide the overview
    public void SelectPart(PartItemConnector _part)
    {
        if (partItemsList.Count > 0 && selectedPart >= 0)
        {
            if (partItemsList[selectedPart] == _part)
            {
                selectedPart = -1;
                partOverviewObj.SetActive(false);
                return;
            }
        }

        for (int i = 0; i < partItemsList.Count; i++)
        {
            if (partItemsList[i] == _part)
            {
                selectedPart = i;
                partOverviewObj.SetActive(true);
                partCostText.text = "Cost:\n" + "$" + _part.partItem.Cost;

                if (_part.isPurchased)
                {
                    partBuyButton.interactable = false;
                    ColorBlock colors = new ColorBlock();
                    colors.normalColor = Color.red;
                    colors.disabledColor = Color.red;
                    colors.colorMultiplier = 1;
                    partBuyButton.colors = colors;
                    partBuyButton.transform.GetComponentInChildren<Text>().text = "Purchased";
                    return;

                } else if (_part.partItem.Cost > player.Money)
                {
                    ColorBlock colors = new ColorBlock();
                    colors.normalColor = Color.green;
                    colors.disabledColor = Color.gray;
                    colors.colorMultiplier = 1;
                    partBuyButton.colors = colors;
                    partBuyButton.interactable = false;
                    partBuyButton.transform.GetComponentInChildren<Text>().text = "Too Expensive";
                }
                else if (!partBuyButton.interactable)
                {
                    partBuyButton.interactable = true;
                    ColorBlock colors = new ColorBlock();
                    colors.normalColor = Color.green;
                    colors.disabledColor = Color.gray;
                    colors.colorMultiplier = 1;
                    partBuyButton.colors = colors;
                    partBuyButton.transform.GetComponentInChildren<Text>().text = "Purchase";
                }

                partBuyButton.onClick.RemoveAllListeners();
                partBuyButton.onClick.AddListener(delegate { PurchasePart(_part); });
                return;
            }
        }
        selectedPart = -1;
        partOverviewObj.SetActive(false);
    }

    public void PurchasePart(PartItemConnector _part)
    {
        if (player.Money - _part.partItem.Cost >= 0 && !_part.isPurchased)
        {
            player.Money -= _part.partItem.Cost;
            _part.isPurchased = true;
            player.AddPart(_part.partItem);

            _part.purchasedOverlay.SetActive(true);
            _part.partIcon.color = Color.black;

            partBuyButton.interactable = false;
            ColorBlock colors = new ColorBlock();
            colors.normalColor = Color.red;
            colors.disabledColor = Color.red;
            colors.colorMultiplier = 1;
            partBuyButton.colors = colors;
            partBuyButton.transform.GetComponentInChildren<Text>().text = "Purchased";
        }
    }

    public void PopulateWorkerList()
    {
        for(int i = workerConnectors.Count; i > 0; i--)
        {
            if (workerConnectors[i-1].isRecruited)
            {
                WorkerCardConnector _workerCard = workerConnectors[i-1];
                workerConnectors.Remove(_workerCard);
                _workerCard.gameObject.SetActive(false);
                Destroy(_workerCard.gameObject);
            }
        }
        for (int c = 3 - workerConnectors.Count; c > 0; c--)
        {
            NewWorkerCard(NewWorker());
        }
    }

    public void RecruitWorker(WorkerCardConnector workerConnector)
    {
        workerConnector.workerImageBackground.color = Color.gray;
        workerConnector.workerImage.color = Color.black;
        workerConnector.recruitedOverlay.SetActive(true);
        ColorBlock buttonColors = new ColorBlock();
        buttonColors.normalColor = Color.red;
        buttonColors.disabledColor = Color.red;
        buttonColors.colorMultiplier = 1;
        workerConnector.recruitButton.colors = buttonColors;
        workerConnector.recruitButton.interactable = false;
        workerConnector.isRecruited = true;

        player.Money -= (int)workerConnector.worker.Cost;
        player.AddWorker(workerConnector.worker);
    }

    void NewWorkerCard(WorkerBase _worker)
    {
        WorkerCardConnector newWorkerCard = Instantiate(workerCardPrefab, workerCardGroup).GetComponent<WorkerCardConnector>();
        newWorkerCard.worker = _worker;
        newWorkerCard.workerNameText.text = _worker.WorkerName;
        newWorkerCard.motorSkillText.text = _worker.Motorskills.ToString();
        newWorkerCard.engineerSkillText.text = _worker.Engineering.ToString();
        newWorkerCard.operationSkillText.text = _worker.Operating.ToString();
        newWorkerCard.workerImage.sprite = _worker.Portrait;
        newWorkerCard.shopMain = this;

        newWorkerCard.costText.text = "Cost: $" + _worker.Cost;

        workerConnectors.Add(newWorkerCard);
    }

    WorkerBase NewWorker()
    {
        WorkerBase newWorker = ScriptableObject.CreateInstance<WorkerBase>();
        newWorker.Motorskills = UnityEngine.Random.Range(1, 11);
        newWorker.Engineering = UnityEngine.Random.Range(1, 11);
        newWorker.Operating = UnityEngine.Random.Range(1, 11);
        int rand = UnityEngine.Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                newWorker.WorkerName = workerInfoHolder.GetName(Gender.Male);
                newWorker.Gender = Gender.Male;
                newWorker.Portrait = workerInfoHolder.GetPortrait(Gender.Male);
                break;
            case 1:
                newWorker.WorkerName = workerInfoHolder.GetName(Gender.Female);
                newWorker.Gender = Gender.Female;
                newWorker.Portrait = workerInfoHolder.GetPortrait(Gender.Female);
                break;
            case 2:
                newWorker.WorkerName = workerInfoHolder.GetName(Gender.Other);
                newWorker.Gender = Gender.Other;
                newWorker.Portrait = workerInfoHolder.GetPortrait(Gender.Other);
                break;
        }

        newWorker.name = newWorker.WorkerName + " Asset";
        newWorker.Cost = GetWorkerCost(newWorker);

        return newWorker;
    }

    float GetWorkerCost(WorkerBase _worker)
    {
        return (_worker.Motorskills * _worker.Engineering * _worker.Operating) * UnityEngine.Random.Range(2,6);
    }

    void NewVehicleCard(TierProbability _tierChances)
    {
        VehicleCardConnector _newVehicleCard = Instantiate(vehicleCardPrefab, vehicleCardHolder).GetComponent<VehicleCardConnector>();
        _newVehicleCard.shopMain = this;
        _newVehicleCard.vehicle = _newVehicleCard.gameObject.AddComponent<VehicleClass>();
        _newVehicleCard.vehicle.ShipName = partDataHolder.ShipNames[UnityEngine.Random.Range(0, partDataHolder.ShipNames.Count)];

        partDataHolder.GetRandomPart(_tierChances, out PartBody _body);
        _newVehicleCard.vehicle.InstallPart(_body);

        partDataHolder.GetRandomPart(_tierChances, out PartEngine _engine);
        _newVehicleCard.vehicle.InstallPart(_engine);

        partDataHolder.GetRandomPart(_tierChances, out PartWheel _wheels);
        _newVehicleCard.vehicle.InstallPart(_wheels);

        PartCabin _cabin = null;
        PartDrill _drill = null;

        //Cabin is nonessential so it is a random chance
        _newVehicleCard.vehicleVisualParts[2].sprite = null;
        _newVehicleCard.vehicleVisualParts[2].color = new Color(0, 0, 0, 0);
        if (UnityEngine.Random.Range(0, 10) > 4)
        {
            partDataHolder.GetRandomPart(_tierChances, out _cabin);
            _newVehicleCard.vehicle.InstallPart(_cabin);
            _newVehicleCard.vehicleVisualParts[2].sprite = _cabin.Image;
            _newVehicleCard.vehicleVisualParts[2].color = Color.white;
        }

        //Drill is nonessential so it is a random chance
        _newVehicleCard.vehicleVisualParts[5].sprite = null;
        _newVehicleCard.vehicleVisualParts[5].color = new Color(0, 0, 0, 0);
        if (UnityEngine.Random.Range(0, 10) > 5)
        {
            partDataHolder.GetRandomPart(_tierChances, out _drill);
            _newVehicleCard.vehicle.InstallPart(_drill);
            _newVehicleCard.vehicleVisualParts[5].sprite = _drill.Image;
            _newVehicleCard.vehicleVisualParts[5].color = Color.white;
        }

        _newVehicleCard.shipNameText.text = _newVehicleCard.vehicle.ShipName;
        _newVehicleCard.costMultiplier = UnityEngine.Random.Range(1, 3);
        _newVehicleCard.vehicleCostText.text = "Cost: $" + _newVehicleCard.vehicle.GetCost(_newVehicleCard.costMultiplier);
        _newVehicleCard.vehicle.Cost = _newVehicleCard.vehicle.GetCost(_newVehicleCard.costMultiplier);

        _newVehicleCard.vehicleVisualParts[0].sprite = _body.Image;
        _newVehicleCard.vehicleVisualParts[1].sprite = _body.WindowSprite;
        //_newVehicleCard.vehicleVisualParts[2].sprite = _cabin.Image;
        //_newVehicleCard.vehicleVisualParts[3].sprite = _engine.Image;
        _newVehicleCard.vehicleVisualParts[4].sprite = _wheels.Image;

        //Stat text
        Text statText = Instantiate(vehicleStatPrefab, _newVehicleCard.shipStatList.transform).GetComponentInChildren<Text>();
        statText.text = "Hull: " + _body.PartName;
        statText = Instantiate(vehicleStatPrefab, _newVehicleCard.shipStatList.transform).GetComponentInChildren<Text>();
        if (_cabin != null)
        {
            statText.text = "Observatory: " + _cabin.PartName;
        }
        else
        {
            statText.text = "Observatory: None";
        }
            statText = Instantiate(vehicleStatPrefab, _newVehicleCard.shipStatList.transform).GetComponentInChildren<Text>();
            statText.text = "Engine: " + _engine.PartName;
            statText = Instantiate(vehicleStatPrefab, _newVehicleCard.shipStatList.transform).GetComponentInChildren<Text>();
        if (_drill != null)
        {
            statText.text = "Drilling Module: " + _drill.PartName;
        }
        else
        {
            statText.text = "Drilling Module: None";
        }
            statText = Instantiate(vehicleStatPrefab, _newVehicleCard.shipStatList.transform).GetComponentInChildren<Text>();
            statText.text = "Movement Module: " + _wheels.PartName;
        }

    void PopulatePartsList()
    {
        int neededItems = 6;
        for(int i = partItemsList.Count; i > 0; i--)
        {
            neededItems--;
            if (partItemsList[i-1].isPurchased)
            {
                neededItems++;
                PartItemConnector _part = partItemsList[i - 1];
                partItemsList.Remove(_part);
                _part.gameObject.SetActive(false);
                Destroy(_part.gameObject);
            }
        }
        for (int n = 0; n < neededItems; n++)
        {
            partItemsList.Add(NewPartCard());
        }
    }

    //Spawns a new Spare Part item card
    PartItemConnector NewPartCard()
    {
        Transform spawnPos = partItemHolder1.transform;
        if (partItemHolder1.gameObject.GetComponentsInChildren<PartItemConnector>().Length < 3)
        {
            spawnPos = partItemHolder1.transform;
            //print(partItemHolder1.gameObject.GetComponentsInChildren<PartItemConnector>().Length);
        }else
        {
            spawnPos = partItemHolder2.transform;
        }

        PartItemConnector newPartCard = Instantiate(partItemPrefab, spawnPos).GetComponent<PartItemConnector>();
        newPartCard.shopMain = this;
        newPartCard.purchaseItemButton = partBuyButton;

        newPartCard.partItem = partDataHolder.GetRandomPart();

        return newPartCard;
    }

    void AddShipSelector(VehicleClass _vehicle)
    {
        GameObject newSelector = Instantiate(shipSelectorPrefab, dockedShipsGroup.transform);
        ShipSelector selector = newSelector.GetComponent<ShipSelector>();
        selector.Vehicle = _vehicle;
        selector.Shop = this;
        shipSelectors.Add(selector);
    }

    void DockShip(VehicleClass _vehicle)
    {
        dockedShips.Add(_vehicle);

        if (dockedShips.Count == 1)
            SelectShipTab(_vehicle);

        helpResourcesText.gameObject.SetActive(false);

        foreach (ShipSelector sellector in shipSelectors)
        {
            if (sellector.Vehicle == _vehicle)
            {
                sellector.gameObject.SetActive(true);
                return;
            }
        }
        AddShipSelector(_vehicle);
    }

    void UndockShip(VehicleClass _vehicle)
    {
        dockedShips.Remove(_vehicle);
        foreach (ShipSelector selector in shipSelectors)
        {
            if (selectedVehicle == _vehicle)
                selectedVehicle = null;

            if (selector.Vehicle == _vehicle)
            {
                selector.gameObject.SetActive(false);
                DisableResourceList();
            }
        }
        if (dockedShips.Count <= 0)
        {
            currentCapacityText.text = "0";
            maxCapacityText.text = "0";
            helpResourcesText.gameObject.SetActive(true);
        }
        else
        {
            SelectShipTab(dockedShips[0]);
        }
    }

    void CreateResourceList()
    {
        foreach (ResourceId resourceId in Enum.GetValues(typeof(ResourceId)))
        {
            bool addResource = true;
            foreach (ResourceListConnector connector in resourceListItems)
            {
                if (connector.resourceId == resourceId)
                {
                    addResource = false;
                    RefreshResource(resourceId, connector);
                    break;
                }
            }
            if (addResource)
                AddNewResource(resourceId);
        }
    }

    void DisableResourceList()
    {
        foreach (ResourceListConnector connector in resourceListItems)
        {
            connector.gameObject.SetActive(false);
        }
    }

    public void RefreshResource(ResourceId _resource, ResourceListConnector _connector)
    {
        if (selectedVehicle.Inventory.CheckForResource(_resource) || _resource == ResourceId.Fuel || _resource == ResourceId.Rations)
        {
            _connector.gameObject.SetActive(true);            

            _connector.resourceName.text = selectedVehicle.Inventory.GetResourceName(_resource);
            _connector.resourceQuantity.text = selectedVehicle.Inventory.GetResourceAmount(_resource).ToString("F2");
            _connector.resourceCapacity.text = selectedVehicle.Inventory.GetResourceCapacity(_resource).ToString("F2");
            _connector.resourceValue.text = "$" + CalculateValue(_resource);
            _connector.shopMain = this;

            if (_resource == ResourceId.Fuel || _resource == ResourceId.Rations)
            {
                _connector.buyOneButton.SetActive(true);
            }
        }
        else if (!selectedVehicle.Inventory.CheckForResource(_resource))
        {
            _connector.gameObject.SetActive(false);
        }
    }

    void AddNewResource(ResourceId _resource)
    {
        if (selectedVehicle.Inventory.CheckForResource(_resource) || _resource == ResourceId.Fuel || _resource == ResourceId.Rations)
        {
            GameObject newListObject = Instantiate(resourceListPrefab, resourceListGroup.transform);
            ResourceListConnector connector = newListObject.GetComponent<ResourceListConnector>();
            resourceListItems.Add(connector);

            connector.resourceId = _resource;
            connector.resourceName.text = selectedVehicle.Inventory.GetResourceName(_resource);
            connector.resourceQuantity.text = selectedVehicle.Inventory.GetResourceAmount(_resource).ToString("F2");
            connector.resourceCapacity.text = selectedVehicle.Inventory.GetResourceCapacity(_resource).ToString("F2");
            connector.resourceValue.text = "$" + CalculateValue(_resource).ToString("F2");
            connector.shopMain = this;

            if (_resource == ResourceId.Fuel || _resource == ResourceId.Rations)
            {
                connector.buyOneButton.SetActive(true);
            }
        }
    }

    void UpdateCapacity()
    {
        currentCapacityText.text = selectedVehicle.Inventory.UsedCapacity.ToString("F2");
        if (selectedVehicle.Inventory.UsedCapacity >= selectedVehicle.Inventory.Capacity * 0.9f)
        {
            currentCapacityText.color = Color.red;
        }else if (selectedVehicle.Inventory.UsedCapacity >= selectedVehicle.Inventory.Capacity * 0.6f)
        {
            currentCapacityText.color = Color.yellow;
        }else
        {
            currentCapacityText.color = Color.white;
        }
        maxCapacityText.text = selectedVehicle.Inventory.Capacity.ToString("F2");
    }

    float CalculateValue(ResourceId _resource)
    {
        for (int i = 0; i < resourceSaleItems.Count; i++)
        {
            if (resourceSaleItems[i].resource == _resource)
            {
                return (resourceSaleItems[i].salePricerPerUnit * selectedVehicle.Inventory.GetResourceAmount(_resource)) * resourceSaleItems[i].saleModifier;
            }
        }
        return 0f;
    }

    float CalculateValue(ResourceId _resource, float _amount)
    {
        for (int i = 0; i < resourceSaleItems.Count; i++)
        {
            if (resourceSaleItems[i].resource == _resource)
            {
                return (resourceSaleItems[i].salePricerPerUnit * _amount) * resourceSaleItems[i].saleModifier;
            }
        }
        return 0f;
    }

    float CalculateValue(ResourceId _resource, float _amount, bool isSelling = false)
    {
        for (int i = 0; i < resourceSaleItems.Count; i++)
        {
            if (resourceSaleItems[i].resource == _resource)
            {
                return (resourceSaleItems[i].buyPricerPerUnit * _amount) * resourceSaleItems[i].saleModifier;
            }
        }
        return 0f;
    }

    
}

[System.Serializable]
public struct ResourceSaleItem
{
    public ResourceId resource;
    public float salePricerPerUnit;
    public float buyPricerPerUnit;
    public float saleModifier;
}

[System.Serializable]
public struct TierProbability
{
    public float tier1Chance;
    public float tier2Chance;
    public float tier3Chance;

    public TierProbability(float _tier1Chance, float _tier2Chance, float _tier3Chance)
    {
        tier1Chance = _tier1Chance;
        tier2Chance = _tier2Chance;
        tier3Chance = _tier3Chance;
    }

    public int GetRandomTier()
    {
        int randChance = UnityEngine.Random.Range(0, 101);
        Debug.Log("Random Chance was " + randChance);
        if (randChance <= GetChance(1))
        {
            return 1;
        }
        else if (randChance > GetChance(1) && randChance <= (GetChance(1) + GetChance(2)))
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    public float GetChance(int tierIndex)
    {
        if (tierIndex == 0)
        {
            return ((tier1Chance + tier2Chance + tier3Chance) / (tier1Chance + tier2Chance + tier3Chance)) * 100;
        }else if (tierIndex == 1)
        {
            return (tier1Chance/ (tier1Chance + tier2Chance + tier3Chance)) * 100;
        }else if (tierIndex == 2)
        {
            return (tier2Chance / (tier1Chance + tier2Chance + tier3Chance)) * 100;
        }else if (tierIndex == 3)
        {
            return (tier3Chance / (tier1Chance + tier2Chance + tier3Chance)) * 100;
        }else
        {
            Debug.LogWarning("Wrong Tier Index");
            return 0f;
        }
    }
}

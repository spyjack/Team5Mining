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
    int selectedVehicleIndex = 0;

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

    [Header("Store General UI")]
    [SerializeField]
    private GameObject marketTab = null;

    [SerializeField]
    private GameObject barracksTab = null;

    [SerializeField]
    private GameObject resourcesTab = null;

    [SerializeField]
    private GameObject shopObject = null;

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

    public void SelectShipTab(VehicleClass _vehicle)
    {
        for (int i = 0; i < shipSelectors.Count; i++)
        {
            if (shipSelectors[i].Vehicle == _vehicle)
            {
                selectedVehicleIndex = i;
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

    public void GenerateVehicle()
    {

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
        }else
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

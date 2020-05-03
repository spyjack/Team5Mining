using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryBarConnector : MonoBehaviour
{
    [SerializeField]
    int currentItems = 0;
    [SerializeField]
    int maxItems = 6;

    [SerializeField]
    ContentType contentType = ContentType.Part;

    [SerializeField]
    List<InventoryBarContent> invContent = new List<InventoryBarContent>();

    [SerializeField]
    Transform itemContentHolder = null;

    [SerializeField]
    Text barTitleText = null;

    [Header("Item Prefabs")]
    [SerializeField]
    GameObject partItemPrefab = null;

    [SerializeField]
    GameObject workerItemPrefab = null;

    [SerializeField]
    GameObject sellMenu = null;

    [SerializeField]
    Text sellMenuDescText = null;

    [SerializeField]
    Text sellMenuCostText = null;

    [SerializeField]
    ShipEditor shipEditor = null;

    [SerializeField]
    EventSystem eventSystem = null;

    [SerializeField]
    GraphicRaycaster gfxRaycast = null;

    InventoryBarContent selectedBarContent = null;

    public int ItemCount
    {
        get { return currentItems; }
    }

    public int MaxItems
    {
        get { return maxItems; }
    }

    public ShipEditor Editor
    {
        get { return shipEditor; }
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetItems(contentType);
        sellMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PointerEventData _pointerEvenData = new PointerEventData(eventSystem);
            _pointerEvenData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gfxRaycast.Raycast(_pointerEvenData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.tag == "InvSlot")
                {
                    InventoryBarContent _resultContent = result.gameObject.GetComponentInParent<InventoryBarContent>();
                    if (_resultContent.type == contentType && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                    {
                        selectedBarContent = _resultContent;
                        sellMenu.SetActive(true);
                        if (contentType == ContentType.Part)
                        {
                            sellMenuDescText.text = "Would you like to <b>Sell</b> your " + selectedBarContent.partContent.PartName + "?";
                            sellMenuCostText.text = "+$" + selectedBarContent.partContent.Cost * 0.75f;
                        }
                        else
                        {
                            sellMenuDescText.text = "Would you like to <b>Fire</b> " + selectedBarContent.workerContent.WorkerName + "?";
                            sellMenuCostText.text = "";
                        }
                    }else if (_resultContent.type == contentType && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                    {
                        selectedBarContent = _resultContent;
                        SellItem();
                    }
                }
            }
        }
    }

    public void SellItem()
    {
        for (int i = 0; i < invContent.Count; i++)
        {
            if (invContent[i] == selectedBarContent)
            {
                PlayerController _player = FindObjectOfType<PlayerController>();
                if (selectedBarContent.type == ContentType.Part)
                {
                    _player.Money += selectedBarContent.partContent.Cost * 0.75f;
                    _player.RemovePart(selectedBarContent.partContent);
                }
                else
                {
                    _player.RemoveWorker(selectedBarContent.workerContent);
                }

                
                sellMenu.SetActive(false);
                return;
            }
        }
    }

    public void CancelSell()
    {
        sellMenu.SetActive(false);
    }

    public void ResetItems(ContentType _contentType)
    {
        GameObject itemPrefab = null;
        switch (_contentType)
        {
            case ContentType.Worker:
                itemPrefab = workerItemPrefab;
                barTitleText.text = "Recruited Workers";
                break;
            case ContentType.Part:
                itemPrefab = partItemPrefab;
                barTitleText.text = "Spare Parts";
                break;
        }
        int curCount = invContent.Count;
        for (int i = maxItems-1; i >= 0; i--)
        {
            
            if (curCount > i)
            {
                GameObject _obj = invContent[i].gameObject;
                invContent.RemoveAt(i);
                _obj.SetActive(false);
                Destroy(_obj);
            }
            invContent.Add(Instantiate(itemPrefab, itemContentHolder).GetComponent<InventoryBarContent>());
            
        }
    }

    public void AddItem(WorkerBase _worker)
    {
        if (currentItems >= maxItems)
            return;

        foreach (InventoryBarContent item in invContent)
        {
            if (!item.HasContent)
            {
                item.AddContent(_worker);
                item.shipEditor = shipEditor;
                currentItems++;
                return;
            }
        }

    }

    public void AddItem(PartBase _part)
    {
        if (currentItems >= maxItems)
            return;

        foreach (InventoryBarContent item in invContent)
        {
            if (!item.HasContent)
            {
                item.AddContent(_part);
                item.shipEditor = shipEditor;
                currentItems++;
                return;
            }
        }

    }

    public void RemoveItem(PartBase _part)
    {
        if (currentItems <= 0)
            return;

        foreach (InventoryBarContent item in invContent)
        {
            if (item.partContent == _part)
            {
                item.RemoveContent();
                currentItems--;
                return;
            }
        }
    }

    public void RemoveItem(WorkerBase _worker)
    {
        if (currentItems <= 0)
            return;

        foreach (InventoryBarContent item in invContent)
        {
            if (item.workerContent == _worker)
            {
                print("Removing worker " + currentItems);
                item.RemoveContent();
                currentItems--;
                print("Remaining Items " + currentItems);
                return;
            }
        }
    }
}

public enum ContentType
{
    Worker,
    Part
}

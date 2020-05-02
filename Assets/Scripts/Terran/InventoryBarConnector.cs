using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    ShipEditor shipEditor = null;

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
                item.RemoveContent();
                currentItems--;
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

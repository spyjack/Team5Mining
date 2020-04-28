using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBarContent : MonoBehaviour
{
    public ContentType type = ContentType.Part;
    [SerializeField]
    bool hasContent = false;

    public Image contentIcon = null;
    public Button contentButton = null;

    public GameObject emptyContentIcon = null;
    public GameObject contentMain = null;

    public WorkerBase workerContent = null;
    public PartBase partContent = null;

    public ShipEditor shipEditor = null;

    bool isSelected = false;
    bool isHeld = false;

    public bool HasContent
    {
        get { return hasContent; }
    }

    private void Start()
    {
        if (workerContent != null)
        {
            AddContent(workerContent);
        }else if (partContent != null)
        {
            AddContent(partContent);
        }else
        {
            RemoveContent();
        }
    }

    public void AddContent(WorkerBase _worker)
    {
        workerContent = _worker;
        partContent = null;
        type = ContentType.Worker;
        hasContent = true;
        contentIcon.sprite = workerContent.Portrait;
        emptyContentIcon.SetActive(false);
        contentMain.SetActive(true);
        contentButton.onClick.AddListener(OnPickUp);
    }

    public void AddContent(PartBase _part)
    {
        partContent = _part;
        workerContent = null;
        type = ContentType.Part;
        hasContent = true;
        contentIcon.sprite = partContent.Icon;
        emptyContentIcon.SetActive(false);
        contentMain.SetActive(true);
        contentButton.onClick.AddListener(OnPickUp);
    }

    public void RemoveContent()
    {
        workerContent = null;
        partContent = null;
        hasContent = false;
        contentIcon.sprite = null;
        emptyContentIcon.SetActive(true);
        contentMain.SetActive(false);
    }

    public void OnDrop()
    {
        isHeld = false;
        emptyContentIcon.SetActive(false);
        contentMain.SetActive(true);
    }

    public void OnPickUp()
    {
        if (hasContent)
        {
            shipEditor.Hold(this);
            isHeld = true;
            emptyContentIcon.SetActive(true);
            contentMain.SetActive(false);
            
        }
    }
}

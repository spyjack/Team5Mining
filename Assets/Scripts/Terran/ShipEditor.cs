using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipEditor : MonoBehaviour
{
    [SerializeField]
    List<EditorTabConnector> editorTabs = new List<EditorTabConnector>();

    [SerializeField]
    ShopController shopMain = null;

    [SerializeField]
    GameObject editorTabPrefab = null;

    [SerializeField]
    GameObject editorMenu = null;

    [SerializeField]
    GameObject editorTabHolder = null;

    [SerializeField]
    InventoryBarContent heldItem = null;

    [SerializeField]
    VehicleClass selectedShip = null;

    [SerializeField]
    Transform heldPreview = null;

    [SerializeField]
    Image heldPreviewThumbnail = null;

    [SerializeField]
    GraphicRaycaster gfxRaycast = null;

    [SerializeField]
    EventSystem eventSystem = null;

    [SerializeField]
    Text helpText = null;

    [SerializeField]
    GameObject openMenuButton = null;

    [SerializeField]
    GameObject closeMenuButton = null;

    int openEditorIndex = 0;



    public VehicleClass ActiveShip
    {
        get { return selectedShip; }
    }

    public InventoryBarContent Holding
    {
        get { return heldItem; }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        heldPreview.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData _pointerEvenData = new PointerEventData(eventSystem);
            _pointerEvenData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gfxRaycast.Raycast(_pointerEvenData, results);
            foreach (RaycastResult result in results)
            {
                if (heldItem != null && result.gameObject.tag == "PartComponent" && heldItem.type == ContentType.Part)
                {
                    
                    VehicleEditorComponent _component = result.gameObject.GetComponentInParent<VehicleEditorComponent>();
                    if (heldItem.partContent is PartDrill && _component.partType == PartType.Drill)
                    {
                        _component.AddPart(heldItem.partContent);
                        Drop();
                        editorTabs[openEditorIndex].FinalizeChanges();
                        return;
                    }
                    else if (heldItem.partContent is PartCabin && _component.partType == PartType.Cabin)
                    {
                        _component.AddPart(heldItem.partContent);
                        Drop();
                        editorTabs[openEditorIndex].FinalizeChanges();
                        return;
                    }
                    else if (heldItem.partContent is PartEngine && _component.partType == PartType.Engine)
                    {
                        _component.AddPart(heldItem.partContent);
                        Drop();
                        editorTabs[openEditorIndex].FinalizeChanges();
                        return;
                    }
                    else if (heldItem.partContent is PartWheel && _component.partType == PartType.Wheels)
                    {
                        _component.AddPart(heldItem.partContent);
                        Drop();
                        editorTabs[openEditorIndex].FinalizeChanges();
                        return;
                    }
                    else if (heldItem.partContent is PartUpgrade && _component.partType == PartType.Upgrade)
                    {
                        _component.AddPart(heldItem.partContent);
                        Drop();
                        editorTabs[openEditorIndex].FinalizeChanges();
                        return;
                    }else
                    {
                        Drop();
                    }
                    

                }
                else
                {
                    Drop();
                }
                    
            }
        }else if (Input.GetMouseButtonDown(1))
        {
            PointerEventData _pointerEvenData = new PointerEventData(eventSystem);
            _pointerEvenData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gfxRaycast.Raycast(_pointerEvenData, results);
            foreach (RaycastResult result in results)
            {
                if (heldItem == null && result.gameObject.tag == "PartComponent")
                {
                    PlayerController _player = FindObjectOfType<PlayerController>();
                    if (_player.PartsInventory.ItemCount < _player.PartsInventory.MaxItems)
                    {
                        VehicleEditorComponent editorComponent = result.gameObject.GetComponentInParent<VehicleEditorComponent>();
                        _player.AddPart(editorComponent.installedPart);
                        editorComponent.RemovePart();
                        editorTabs[openEditorIndex].FinalizeChanges();
                        return;
                    }
                }
                else
                {
                    Drop();
                }
            }
        }
    }

    public void Hold(InventoryBarContent _holding)
    {
        if (heldItem != null)
            heldItem.OnDrop();

        heldItem = _holding;
        heldPreview.gameObject.SetActive(true);
        heldPreviewThumbnail.sprite = heldItem.contentIcon.sprite;
    }

    public void Drop()
    {
        if (heldItem != null)
        {
            heldItem.OnDrop();
            heldItem = null;
            heldPreview.gameObject.SetActive(false);
        }
    }

    public void CloseMenu()
    {
        editorMenu.SetActive(false);
        openMenuButton.SetActive(true);
        closeMenuButton.SetActive(false);
    }

    public void OpenMenu()
    {
        editorMenu.SetActive(true);
        openMenuButton.SetActive(false);
        closeMenuButton.SetActive(true);
        helpText.gameObject.SetActive(false);
        if (shopMain.DockedShips.Count <= 0)
        {
            GoToShipEditor(0);
            editorTabs[0].gameObject.SetActive(false);
            helpText.gameObject.SetActive(true);
        }else if (!editorTabs[openEditorIndex].gameObject.activeSelf)
        {
            GoToShipEditor(openEditorIndex);
        }
    }

    public void GoToShipEditor(VehicleClass _vehicle)
    {
        for (int i = 0; i < editorTabs.Count; i++)
        {
            if (editorTabs[i].vehicle == _vehicle)
            {
                selectedShip = _vehicle;
                GoToShipEditor(i);
                break;
            }else if (editorTabs[i].gameObject.activeSelf)
            {
                editorTabs[i].gameObject.SetActive(false);
            }
        }
    }

    public void GoToShipEditor(int index)
    {
        print("Changing Ship Editor");
        editorTabs[openEditorIndex].transform.gameObject.SetActive(false);
        editorTabs[index].transform.gameObject.SetActive(true);
        editorTabs[index].RefreshComponents();
        helpText.gameObject.SetActive(false);
        openEditorIndex = index;
    }

    public void GoToNextShipEditor()
    {
        if (shopMain.DockedShips.Count > 1)
        {
            for (int i = openEditorIndex + 1; i < editorTabs.Count; i++)
            {
                if (shopMain.IsDocked(editorTabs[i].vehicle))
                {
                    GoToShipEditor(i);
                    return;
                }
            }
            
            GoToPrevShipEditor();
        }
    }

    public void GoToPrevShipEditor()
    {
        if (shopMain.DockedShips.Count > 1)
        {
            for (int i = openEditorIndex - 1; i >= 0; i--)
            {
                if (shopMain.IsDocked(editorTabs[i].vehicle))
                {
                    GoToShipEditor(i);
                    break;
                }
            }


        }
    }

    public void CreateShipEditor(VehicleClass _vehicle)
    {
        for (int i = 0; i < editorTabs.Count; i++)
        {
            if (editorTabs[i].vehicle == _vehicle)
            {
                break;
            }
        }
        
        EditorTabConnector _newEditorTab = Instantiate(editorTabPrefab, editorTabHolder.transform).GetComponent<EditorTabConnector>();
        _newEditorTab.vehicle = _vehicle;
        editorTabs.Add(_newEditorTab);
        if (shopMain.DockedShips.Count > 1)
        {
            _newEditorTab.transform.gameObject.SetActive(false);
        }else
        {
            GoToShipEditor(0);
        }
    }

    public void CloseTab()
    {
        editorTabs[openEditorIndex].gameObject.SetActive(false);
        openEditorIndex = 0;
        helpText.gameObject.SetActive(true);
    }

    public void RefreshTabs()
    {
        if (editorTabs.Count > 0 && !editorTabs[openEditorIndex].gameObject.activeInHierarchy)
        {
            GoToShipEditor(openEditorIndex);
        }
    }

}

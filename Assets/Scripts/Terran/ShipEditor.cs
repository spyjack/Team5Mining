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

    [SerializeField]
    UIAudio uiAudio = null;

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
                LeftClickActions(result);    
            }
        }else if (Input.GetMouseButtonDown(1))
        {
            RightClickActions();
        }
    }

    void LeftClickActions(RaycastResult result)
    {
        if (heldItem != null && result.gameObject.tag == "PartComponent" && heldItem.type == ContentType.Part)
        {

            VehicleEditorComponent _component = result.gameObject.GetComponentInParent<VehicleEditorComponent>();
            if (heldItem.partContent is PartDrill && _component.partType == PartType.Drill)
            {
                uiAudio.PlaySound(UIsounds.Upgrade);
                _component.AddPart(heldItem.partContent);
                Drop();
                editorTabs[openEditorIndex].FinalizeChanges();
                return;
            }
            else if (heldItem.partContent is PartCabin && _component.partType == PartType.Cabin)
            {
                uiAudio.PlaySound(UIsounds.Upgrade);
                _component.AddPart(heldItem.partContent);
                Drop();
                editorTabs[openEditorIndex].FinalizeChanges();
                return;
            }
            else if (heldItem.partContent is PartEngine && _component.partType == PartType.Engine)
            {
                uiAudio.PlaySound(UIsounds.Upgrade);
                _component.AddPart(heldItem.partContent);
                Drop();
                editorTabs[openEditorIndex].FinalizeChanges();
                return;
            }
            else if (heldItem.partContent is PartWheel && _component.partType == PartType.Wheels)
            {
                uiAudio.PlaySound(UIsounds.Upgrade);
                _component.AddPart(heldItem.partContent);
                Drop();
                editorTabs[openEditorIndex].FinalizeChanges();
                return;
            }
            else if (heldItem.partContent is PartUpgrade && _component.partType == PartType.Upgrade)
            {
                uiAudio.PlaySound(UIsounds.Upgrade);
                _component.AddPart(heldItem.partContent);
                Drop();
                editorTabs[openEditorIndex].FinalizeChanges();
                return;
            }
            else
            {
                Drop();
            }


        }
        else if (heldItem != null && result.gameObject.tag == "WorkerComponent" && heldItem.type == ContentType.Worker)
        {
            EditorWorkerComponent _component = result.gameObject.GetComponentInParent<EditorWorkerComponent>();
            editorTabs[openEditorIndex].vehicle.GetPart(out PartCabin _cabin);
            bool canEquipAuxillary = _cabin != null || _component.station == WorkStation.Cabin;
            if (editorTabs[openEditorIndex].vehicle.CrewCount < editorTabs[openEditorIndex].vehicle.CrewCountMax && canEquipAuxillary)
            {
                _component.AddWorker(heldItem.workerContent);
                Drop();
                editorTabs[openEditorIndex].EquipWorker(_component.station);
            }
            else { Drop(); }
        }
        else
        {
            Drop();
        }
    }

    void RightClickActions()
    {
        PointerEventData _pointerEvenData = new PointerEventData(eventSystem);
        _pointerEvenData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gfxRaycast.Raycast(_pointerEvenData, results);
        PlayerController _player = FindObjectOfType<PlayerController>();
        foreach (RaycastResult result in results)
        {
            if (heldItem == null && result.gameObject.tag == "PartComponent")
            {
                VehicleEditorComponent editorComponent = result.gameObject.GetComponentInParent<VehicleEditorComponent>();
                editorTabs[openEditorIndex].vehicle.GetPart(out PartCabin _cabin);
                if (_player.PartsInventory.ItemCount < _player.PartsInventory.MaxItems && editorComponent.partType != PartType.Cabin)
                {
                    _player.AddPart(editorComponent.installedPart);
                    editorComponent.RemovePart();
                    editorTabs[openEditorIndex].FinalizeChanges();
                    uiAudio.PlaySound(UIsounds.Upgrade);
                    return;
                }
                else if (_player.PartsInventory.ItemCount < _player.PartsInventory.MaxItems && editorComponent.partType == PartType.Cabin && _cabin != null)
                {
                    int cabinmates = 0;
                    for (int c = 1; c < editorTabs[openEditorIndex].vehicle.Crew.Length; c++)
                    {
                        if (editorTabs[openEditorIndex].vehicle.Crew[c] != null)
                            cabinmates++;
                    }
                    print("Cabin mates " + cabinmates);
                    if (_player.WorkersInventory.ItemCount + cabinmates < _player.WorkersInventory.MaxItems)
                    {
                        for (int c = 1; c < editorTabs[openEditorIndex].vehicle.Crew.Length; c++)
                        {
                            if (editorTabs[openEditorIndex].vehicle.Crew[c] != null)
                            {
                                _player.AddWorker(editorTabs[openEditorIndex].vehicle.Crew[c]);
                                editorTabs[openEditorIndex].vehicle.AssignWorker(c, null);
                            }
                        }
                        _player.AddPart(editorComponent.installedPart);
                        editorComponent.RemovePart();
                        editorTabs[openEditorIndex].FinalizeChanges();
                        uiAudio.PlaySound(UIsounds.Upgrade);
                        return;
                    }
                    else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        for (int c = 1; c < selectedShip.Crew.Length; c++)
                        {
                            if (selectedShip.Crew[c] != null)
                            {
                                editorTabs[openEditorIndex].vehicle.AssignWorker(c, null);
                            }
                        }
                        _player.AddPart(editorComponent.installedPart);
                        editorComponent.RemovePart();
                        editorTabs[openEditorIndex].FinalizeChanges();
                        uiAudio.PlaySound(UIsounds.Upgrade);
                        return;
                    }
                }
            }
            else if (heldItem == null && result.gameObject.tag == "WorkerComponent")
            {
                EditorWorkerComponent editorWorkerComponent = result.gameObject.GetComponentInParent<EditorWorkerComponent>();
                if (_player.WorkersInventory.ItemCount < _player.WorkersInventory.MaxItems && editorWorkerComponent.assignedWorker != null)
                {
                    _player.AddWorker(editorTabs[openEditorIndex].vehicle.GetWorker(editorWorkerComponent.station));
                    editorWorkerComponent.RemoveWorker();
                    editorTabs[openEditorIndex].EquipWorker(editorWorkerComponent.station);
                    return;
                }
            }
            else
            {
                Drop();
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
            editorTabs[openEditorIndex].gameObject.SetActive(false);
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
            }else if (editorTabs[i].gameObject.activeSelf)
            {
                editorTabs[i].gameObject.SetActive(false);
            }
        }
    }

    public void GoToShipEditor(int index)
    {
        editorTabs[openEditorIndex].transform.gameObject.SetActive(false);
        editorTabs[index].transform.gameObject.SetActive(true);
        editorTabs[index].RefreshComponents();
        helpText.gameObject.SetActive(false);
        openEditorIndex = index;
        selectedShip = editorTabs[index].vehicle;
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
        if (shopMain.DockedShips.Count != 1)
        {
            _newEditorTab.transform.gameObject.SetActive(false);
        }else if (shopMain.DockedShips.Count == 1)
        {
            GoToShipEditor(editorTabs.Count-1);
        }
    }

    public void CloseTab()
    {
        editorTabs[openEditorIndex].gameObject.SetActive(false);
        helpText.gameObject.SetActive(true);
    }

    public void RefreshTabs()
    {
        if (editorTabs.Count > 0 && !editorTabs[openEditorIndex].gameObject.activeSelf)
        {
            GoToNextShipEditor();
        }
    }

}

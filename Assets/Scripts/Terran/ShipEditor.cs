using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipEditor : MonoBehaviour
{
    [SerializeField]
    Button closeMenuButton = null;

    [SerializeField]
    GameObject EditorMenu = null;

    [SerializeField]
    InventoryBarContent heldItem = null;

    [SerializeField]
    Transform selectedShip = null;

    [SerializeField]
    Transform heldPreview = null;

    [SerializeField]
    Image heldPreviewThumbnail = null;

    [SerializeField]
    GraphicRaycaster gfxRaycast = null;

    [SerializeField]
    EventSystem eventSystem = null;

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
                if (result.gameObject.tag != "stuff")
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
        EditorMenu.SetActive(false);
    }


}

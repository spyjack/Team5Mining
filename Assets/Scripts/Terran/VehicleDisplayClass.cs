using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDisplayClass : MonoBehaviour
{
    [Header("Vehicle Connectors")]
    [SerializeField]
    VehicleClass vehicle = null;

    [SerializeField]
    VehicleMovement vehicleMover = null;

    [SerializeField]
    ShipDrilling vehicleDriller = null;

    [Header("Vehicle Parts")]
    [SerializeField]
    SpriteRenderer drillRenderer = null;

    Sprite drillSpriteAlt = null;

    [SerializeField]
    Transform drillTransform = null;

    [SerializeField]
    SpriteRenderer cabinRenderer = null;

    [SerializeField]
    SpriteRenderer bodyRenderer = null;

    [SerializeField]
    SpriteRenderer windowRenderer = null;

    [SerializeField]
    SpriteRenderer wheelRenderer = null;

    Sprite wheelSpriteAlt = null;

    [SerializeField]
    GameObject headlights = null;

    [Header("Vehicle UI")]
    [SerializeField]
    GameObject uiHolder = null;

    [SerializeField]
    GameObject uiPrefab = null;

    [SerializeField]
    ShipUIConnector uiConnector = null;


    bool isDrillAnimating = false;
    bool isWheelAnimating = false;
    // Start is called before the first frame update
    void Start()
    {
        uiHolder = GameObject.Find("ShipUI");
        ReDraw();
        StartCoroutine(LightCheck());
    }

    // Update is called once per frame
    void Update()
    {
        if (vehicleDriller.ActiveDrill && !isDrillAnimating)
        {
            isDrillAnimating = true;
            StartCoroutine(AnimateDrill());
        }else if (!vehicleDriller.ActiveDrill && isDrillAnimating)
        {
            isDrillAnimating = false;
            StopCoroutine(AnimateDrill());
        }

        if(vehicleMover.Speed > 10 && !isWheelAnimating)
        {
            isWheelAnimating = true;
            StartCoroutine(AnimateWheels());
        }else if (vehicleMover.Speed < 5 && isWheelAnimating)
        {
            isWheelAnimating = false;
            StopCoroutine(AnimateWheels());
        }

        if (Vector2.Distance(drillTransform.position, vehicleMover.DrillPoint) > 0.1f)
        {
            drillTransform.position = Vector2.Lerp(drillTransform.position, vehicleMover.DrillPoint, Time.deltaTime * 3f);
        }

        if (drillTransform.position.x > transform.position.x && transform.localScale.x > -1)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }else if (drillTransform.position.x < transform.position.x && transform.localScale.x < 1)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void AlternateDrillSprite()
    {
        if (drillSpriteAlt == null)
            return;

        Sprite prevSprite = drillRenderer.sprite;
        drillRenderer.sprite = drillSpriteAlt;
        drillSpriteAlt = prevSprite;
    }

    public void AlternateWheelSprite()
    {
        if (wheelSpriteAlt == null)
            return;

        Sprite prevSprite = wheelRenderer.sprite;
        wheelRenderer.sprite = wheelSpriteAlt;
        wheelSpriteAlt = prevSprite;
    }

    public void DoNothing()
    {
        //Do Nothing
    }

    public void ReDraw()
    {
        vehicle.GetPart(out PartBody _body);
        if (_body != null)
        {
            bodyRenderer.sprite = _body.Image;
            windowRenderer.sprite = _body.WindowSprite;
        }
        else { bodyRenderer.sprite = null; windowRenderer.sprite = null; }

        vehicle.GetPart(out PartCabin _cabin);
        if (_cabin != null)
        { cabinRenderer.sprite = _cabin.Image; }
        else { cabinRenderer.sprite = null; }

        vehicle.GetPart(out PartDrill _drill);
        if (_drill != null)
        { drillRenderer.sprite = _drill.Image; drillSpriteAlt = _drill.SecondaryImage; }
        else { drillRenderer.sprite = null; }

        vehicle.GetPart(out PartWheel _wheels);
        if (_wheels != null)
        { wheelRenderer.sprite = _wheels.Image; wheelSpriteAlt = _wheels.SecondaryImage; }
        else { wheelRenderer.sprite = null; }

        RefreshUI();
    }

    public void SetUI(bool active)
    {
        uiConnector.gameObject.SetActive(active);
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (uiHolder == null)
            uiHolder = GameObject.Find("ShipUI");

        if (uiConnector == null)
            uiConnector = Instantiate(uiPrefab, uiHolder.transform).GetComponent<ShipUIConnector>();

        uiConnector.RefreshUI(vehicle);
    }

    public void RefreshUiInv()
    {
        if (uiConnector == null)
            return;

        uiConnector.RefreshInventoryUI(vehicle);
    }

    IEnumerator AnimateDrill()
    {
        while(true)
        {
            if (!isDrillAnimating)
                yield break;

            AlternateDrillSprite();
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator AnimateWheels()
    {
        while (true)
        {
            if (!isWheelAnimating)
                yield break;

            AlternateWheelSprite();
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator LightCheck()
    {
        while(true)
        {
            if (this.transform.position.y > 17 && headlights.activeSelf)
            {
                headlights.SetActive(false);
            }else if (this.transform.position.y <= 17 && !headlights.activeSelf)
            {
                headlights.SetActive(true);
            }
            yield return new WaitForSeconds(2f);
        }
    }
}

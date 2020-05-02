using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleEditorComponent : MonoBehaviour
{
    public PartType partType = PartType.Drill;

    public Text titleText = null;

    public PartBase installedPart = null;

    public Image icon = null;

    public EditorTabConnector tabConnector = null;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        switch (partType)
        {
            case PartType.Drill:
                if (installedPart != null && installedPart is PartDrill)
                {
                    PartDrill drill = (PartDrill)installedPart;
                    titleText.text = drill.PartName;
                    icon.sprite = drill.Icon;
                    icon.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    titleText.text = "Drill";
                    icon.sprite = null;
                    icon.color = new Color(1, 1, 1, 0);
                }
                break;
            case PartType.Cabin:
                if (installedPart != null && installedPart is PartCabin)
                {
                    PartCabin cabin = (PartCabin)installedPart;
                    titleText.text = cabin.PartName;
                    icon.sprite = cabin.Icon;
                    icon.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    titleText.text = "Cabin";
                    icon.sprite = null;
                    icon.color = new Color(1, 1, 1, 0);
                }
                break;
            case PartType.Engine:
                if (installedPart != null && installedPart is PartEngine)
                {
                    PartEngine engine = (PartEngine)installedPart;
                    titleText.text = engine.PartName;
                    icon.sprite = engine.Icon;
                    icon.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    titleText.text = "Engine";
                    icon.sprite = null;
                    icon.color = new Color(1, 1, 1, 0);
                }
                break;
            case PartType.Wheels:
                if (installedPart != null && installedPart is PartWheel)
                {
                    PartWheel wheel = (PartWheel)installedPart;
                    titleText.text = wheel.PartName;
                    icon.sprite = wheel.Icon;
                    icon.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    titleText.text = "Wheels";
                    icon.sprite = null;
                    icon.color = new Color(1, 1, 1, 0);
                }
                break;
            case PartType.Upgrade:
                if (installedPart != null && installedPart is PartUpgrade)
                {
                    PartUpgrade upgrade = (PartUpgrade)installedPart;
                    titleText.text = upgrade.PartName;
                    icon.sprite = upgrade.Icon;
                    icon.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    titleText.text = "Equipment";
                    icon.sprite = null;
                    icon.color = new Color(1, 1, 1, 0);
                }
                break;
        }
    }

    public void InstallPart(VehicleClass _vehicle)
    {
        switch (partType)
        {
            case PartType.Drill:
                _vehicle.InstallPart((PartDrill)installedPart);
                break;
            case PartType.Cabin:
                _vehicle.InstallPart((PartCabin)installedPart);
                break;
            case PartType.Engine:
                _vehicle.InstallPart((PartEngine)installedPart);
                break;
            case PartType.Wheels:
                _vehicle.InstallPart((PartWheel)installedPart);
                break;
            case PartType.Upgrade:
                _vehicle.InstallPart((PartUpgrade)installedPart);
                break;
        }
    }

    public void AddPart(PartBase _part)
    {
        installedPart = _part;
        if (_part != null)
        {
            titleText.text = _part.PartName;
            icon.sprite = _part.Icon;
            icon.color = new Color(1, 1, 1, 1);
        }
    }

    public void AddPart(PartDrill _part)
    {
        installedPart = _part;
        if (_part != null)
        {
            titleText.text = _part.PartName;
            icon.sprite = _part.Icon;
            icon.color = new Color(1, 1, 1, 1);
        }
    }

    public void AddPart(PartCabin _part)
    {
        installedPart = _part;
        if (_part != null)
        {
            titleText.text = _part.PartName;
            icon.sprite = _part.Icon;
            icon.color = new Color(1, 1, 1, 1);
        }
    }

    public void AddPart(PartEngine _part)
    {
        installedPart = _part;
        if (_part != null)
        {
            titleText.text = _part.PartName;
            icon.sprite = _part.Icon;
            icon.color = new Color(1, 1, 1, 1);
        }
    }

    public void AddPart(PartWheel _part)
    {
        installedPart = _part;
        if (_part != null)
        {
            titleText.text = _part.PartName;
            icon.sprite = _part.Icon;
            icon.color = new Color(1, 1, 1, 1);
        }
    }

    public void AddPart(PartUpgrade _part)
    {
        installedPart = _part;
        if (_part != null)
        {
            titleText.text = _part.PartName;
            icon.sprite = _part.Icon;
            icon.color = new Color(1, 1, 1, 1);
        }
    }
}

public enum PartType
{
    Drill,
    Cabin,
    Engine,
    Wheels,
    Upgrade
}

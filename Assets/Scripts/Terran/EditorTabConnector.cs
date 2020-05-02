using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorTabConnector : MonoBehaviour
{
    public List<VehicleEditorComponent> editorComponents = new List<VehicleEditorComponent>();

    public Text shipNameText = null;

    public InputField nameInput = null;

    public VehicleClass vehicle = null;

    private void Start()
    {
        RefreshComponents();
    }

    public void FinalizeChanges()
    {
        PlayerController _player = FindObjectOfType<PlayerController>();
        foreach (VehicleEditorComponent _editorComponent in editorComponents)
        {
            switch (_editorComponent.partType)
            {
                case PartType.Drill:
                    vehicle.GetPart(out PartDrill _drill);
                    if (_drill == null || _drill != (PartDrill)_editorComponent.installedPart)
                    {
                        if (_editorComponent.installedPart != null) //if the part isn't just being removed, take it away from the player, then add the old part to the players inv
                        {
                            _player.RemovePart(_editorComponent.installedPart);
                            _player.AddPart(_drill);
                        }
                        vehicle.InstallPart((PartDrill)_editorComponent.installedPart);
                    }
                    break;
                case PartType.Cabin:
                    vehicle.GetPart(out PartCabin _cabin);
                    if (_cabin == null || _cabin != (PartCabin)_editorComponent.installedPart)
                    {
                        if (_editorComponent.installedPart != null) //if the part isn't just being removed, take it away from the player, then add the old part to the players inv
                        {
                            _player.RemovePart(_editorComponent.installedPart);
                            _player.AddPart(_cabin);
                        }
                        vehicle.InstallPart((PartCabin)_editorComponent.installedPart);
                    }
                    break;
                case PartType.Engine:
                    vehicle.GetPart(out PartEngine _engine);
                    if (_engine == null || _engine != (PartEngine)_editorComponent.installedPart)
                    {
                        if (_editorComponent.installedPart != null) //if the part isn't just being removed, take it away from the player, then add the old part to the players inv
                        {
                            _player.RemovePart(_editorComponent.installedPart);
                            _player.AddPart(_engine);
                        }
                        vehicle.InstallPart((PartEngine)_editorComponent.installedPart);
                    }
                    break;
                case PartType.Wheels:
                    vehicle.GetPart(out PartWheel _wheels);
                    if (_wheels == null || _wheels != (PartWheel)_editorComponent.installedPart)
                    {
                        if (_editorComponent.installedPart != null) //if the part isn't just being removed, take it away from the player, then add the old part to the players inv
                        {
                            _player.RemovePart(_editorComponent.installedPart);
                            _player.AddPart(_wheels);
                        }
                        vehicle.InstallPart((PartWheel)_editorComponent.installedPart);
                    }
                    break;
                case PartType.Upgrade:
                    vehicle.GetPart(out PartUpgrade _upgrade);
                    if (_upgrade == null || _upgrade != (PartUpgrade)_editorComponent.installedPart)
                    {
                        if (_editorComponent.installedPart != null) //if the part isn't just being removed, take it away from the player, then add the old part to the players inv
                        {
                            _player.RemovePart(_editorComponent.installedPart);
                            _player.AddPart(_upgrade);
                        }
                        vehicle.InstallPart((PartUpgrade)_editorComponent.installedPart);
                    }
                    break;
            }
        }
    }

    public void FinalizeChanges(PartDrill _drillPart)
    {
        PlayerController _player = FindObjectOfType<PlayerController>();
        vehicle.GetPart(out PartDrill _drill);
        if (_drill == null || _drill != _drillPart)
        {
            if (_drillPart != null) //if the part isn't just being removed, take it away from the player, then add the old part to the players inv
            {
                _player.RemovePart(_drillPart);
                _player.AddPart(_drill);
            }
            vehicle.InstallPart(_drillPart);
        }
    }


    public void RefreshComponents()
    {
        for (int i = 0; i < editorComponents.Count; i++)
        {
            if (editorComponents[i].partType == PartType.Drill)
            {
                vehicle.GetPart(out PartDrill _drill);
                editorComponents[i].AddPart(_drill);
            }else if (editorComponents[i].partType == PartType.Cabin)
            {
                vehicle.GetPart(out PartCabin _cabin);
                editorComponents[i].AddPart(_cabin);
            }
            else if (editorComponents[i].partType == PartType.Engine)
            {
                vehicle.GetPart(out PartEngine _engine);
                editorComponents[i].AddPart(_engine);
            }
            else if (editorComponents[i].partType == PartType.Wheels)
            {
                vehicle.GetPart(out PartWheel _wheel);
                editorComponents[i].AddPart(_wheel);
            }
            else if (editorComponents[i].partType == PartType.Upgrade)
            {
                vehicle.GetPart(out PartUpgrade _upgrade);
                editorComponents[i].AddPart(_upgrade);
            }
            editorComponents[i].SetUp();
            shipNameText.text = vehicle.ShipName;
        }
    }

    public void ChangeShipName()
    {
        //if you're changing the name already and called, set name
        if (nameInput.interactable)
        {
            nameInput.interactable = false;
            vehicle.ShipName = nameInput.text;
            FindObjectOfType<ShopController>().RefreshSelectorTabNames();
        }else
        {
            nameInput.interactable = true;
        }
    }
}
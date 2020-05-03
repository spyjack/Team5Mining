using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorTabConnector : MonoBehaviour
{
    public List<VehicleEditorComponent> editorComponents = new List<VehicleEditorComponent>();

    public List<EditorWorkerComponent> editorWorkerComponents = new List<EditorWorkerComponent>();

    public Text shipNameText = null;

    public InputField nameInput = null;

    public VehicleClass vehicle = null;

    private void Start()
    {
        RefreshComponents();
        RefreshWorkerComponents();
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
                            _player.AddPart(_drill);
                            _player.RemovePart(_editorComponent.installedPart);
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
                            _player.AddPart(_cabin);
                            _player.RemovePart(_editorComponent.installedPart);
                        }
                        vehicle.InstallPart((PartCabin)_editorComponent.installedPart);
                        RefreshWorkerComponents();
                    }
                    break;
                case PartType.Engine:
                    vehicle.GetPart(out PartEngine _engine);
                    if (_engine == null || _engine != (PartEngine)_editorComponent.installedPart)
                    {
                        if (_editorComponent.installedPart != null) //if the part isn't just being removed, take it away from the player, then add the old part to the players inv
                        {
                            _player.AddPart(_engine);
                            _player.RemovePart(_editorComponent.installedPart);
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
                            _player.AddPart(_wheels);
                            _player.RemovePart(_editorComponent.installedPart);
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
                            _player.AddPart(_upgrade);
                            _player.RemovePart(_editorComponent.installedPart);
                        }
                        vehicle.InstallPart((PartUpgrade)_editorComponent.installedPart);
                    }
                    break;
            }
        }
        vehicle.VehicleGraphics.ReDraw();
    }

    public void FinalizeChanges(PartDrill _drillPart)
    {
        PlayerController _player = FindObjectOfType<PlayerController>();
        vehicle.GetPart(out PartDrill _drill);
        if (_drill == null || _drill != _drillPart)
        {
            if (_drillPart != null) //if the part isn't just being removed, take it away from the player, then add the old part to the players inv
            {
                _player.AddPart(_drill);
                _player.RemovePart(_drillPart);
            }
            vehicle.InstallPart(_drillPart);
        }
        vehicle.VehicleGraphics.ReDraw();
    }

    public void EquipWorker(int _stationIndex)
    {
        PlayerController _player = FindObjectOfType<PlayerController>();
        if (editorWorkerComponents[_stationIndex].assignedWorker != null)
        {
            _player.AddWorker(vehicle.Crew[_stationIndex]);
            _player.RemoveWorker(editorWorkerComponents[_stationIndex].assignedWorker);
        }
        vehicle.AssignWorker(_stationIndex, editorWorkerComponents[_stationIndex].assignedWorker);
    }

    public void EquipWorker(WorkStation _station)
    {
        PlayerController _player = FindObjectOfType<PlayerController>();
        switch (_station)
        {
            
            case WorkStation.Cabin:
                if (editorWorkerComponents[0].assignedWorker != null)
                {
                    _player.AddWorker(vehicle.GetWorker(_station));
                    _player.RemoveWorker(editorWorkerComponents[0].assignedWorker);
                }
                vehicle.AssignWorker(_station, editorWorkerComponents[0].assignedWorker);
                return;
            case WorkStation.Engine:
                if (editorWorkerComponents[1].assignedWorker != null)
                {
                    _player.AddWorker(vehicle.GetWorker(_station));
                    _player.RemoveWorker(editorWorkerComponents[1].assignedWorker);
                }
                vehicle.AssignWorker(_station, editorWorkerComponents[1].assignedWorker);
                return;
            case WorkStation.Drill:
                if (editorWorkerComponents[2].assignedWorker != null)
                {
                    _player.AddWorker(vehicle.GetWorker(_station));
                    _player.RemoveWorker(editorWorkerComponents[2].assignedWorker);
                }
                vehicle.AssignWorker(_station, editorWorkerComponents[2].assignedWorker);
                return;
            case WorkStation.Spare:
                if (editorWorkerComponents[3].assignedWorker != null)
                {
                    _player.AddWorker(vehicle.GetWorker(_station));
                    _player.RemoveWorker(editorWorkerComponents[3].assignedWorker);
                }
                vehicle.AssignWorker(_station, editorWorkerComponents[3].assignedWorker);
                return;
            default:
                if (editorWorkerComponents[0].assignedWorker != null)
                {
                    _player.AddWorker(vehicle.GetWorker(_station));
                    _player.RemoveWorker(editorWorkerComponents[0].assignedWorker);
                }
                vehicle.AssignWorker(_station, editorWorkerComponents[0].assignedWorker);
                return;
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

    public void RefreshWorkerComponents()
    {
        vehicle.GetPart(out PartCabin _cabin);
        int maxComponents = 1;
        if (_cabin != null)
        {
            maxComponents += _cabin.CrewCapacity;
            print("Crew Capacity: " + maxComponents);
        }
        for (int i = 0; i < editorWorkerComponents.Count; i++)
        {
            if (i < maxComponents)
            {
                editorWorkerComponents[i].AddWorker(vehicle.Crew[i]);
            }
            if (_cabin == null && i > 0)
            {
                editorWorkerComponents[i].gameObject.SetActive(false);

            }else if (_cabin != null && !editorWorkerComponents[i].gameObject.activeSelf)
            {
                editorWorkerComponents[i].gameObject.SetActive(true);
                editorWorkerComponents[i].SetUp();
            }
        }
    }

    public void ChangeShipName()
    {
        //if you're changing the name already and called, set name
        if (nameInput.interactable)
        {
            nameInput.interactable = false;
            vehicle.ShipName = nameInput.text;
            vehicle.VehicleGraphics.RefreshUI();
            FindObjectOfType<ShopController>().RefreshSelectorTabNames();
        }else
        {
            nameInput.interactable = true;
        }
    }
}
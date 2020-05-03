using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorWorkerComponent : MonoBehaviour
{
    public WorkStation station = WorkStation.None;

    public Text titleText = null;

    public Text statsText = null;

    public Text nameText = null;

    public WorkerBase assignedWorker = null;

    public Image icon = null;

    public EditorTabConnector tabConnector = null;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        if (assignedWorker == null)
        {
            nameText.text = "Empty";
            statsText.text = "";
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
            return;
        }
        nameText.text = assignedWorker.WorkerName;
        statsText.text = "Mo: " + assignedWorker.Motorskills + " Op: " + assignedWorker.Operating + " En: " + assignedWorker.Engineering;
        icon.sprite = assignedWorker.Portrait;
        icon.color = new Color(1, 1, 1, 1);
    }

    public void AssignWorker(VehicleClass _vehicle)
    {
        _vehicle.AssignWorker(station, assignedWorker);
    }

    public void RemoveWorker()
    {
        assignedWorker = null;
        nameText.text = "Empty";
        statsText.text = "";
        icon.sprite = null;
        icon.color = new Color(1, 1, 1, 0);
    }

    public void AddWorker(WorkerBase _worker)
    {
        assignedWorker = _worker;
        SetUp();
    }
}

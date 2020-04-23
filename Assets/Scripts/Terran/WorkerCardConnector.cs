using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerCardConnector : MonoBehaviour
{
    public Text workerNameText = null;

    public Image workerImage = null;

    public Image workerImageBackground = null;

    public Text healthSkillText = null;

    public Text motorSkillText = null;

    public Text engineerSkillText = null;

    public Text operationSkillText = null;

    public Text costText = null;

    public Button recruitButton = null;

    public GameObject recruitedOverlay = null;

    public WorkerBase worker = null;

    public bool isRecruited = false;
}

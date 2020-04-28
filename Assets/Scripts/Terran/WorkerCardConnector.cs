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

    public ShopController shopMain = null;

    public PlayerController playerMain = null;

    private void Start()
    {
        SetUpButton();
        playerMain = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (playerMain.Money - worker.Cost < 0)
        {
            recruitButton.transform.GetComponentInChildren<Text>().text = "Too Expensive";
            recruitButton.interactable = false;
        }else if (playerMain.GetMaxWorkers() <= playerMain.GetWorkersCount())
        {
            recruitButton.transform.GetComponentInChildren<Text>().text = "No Room";
            recruitButton.interactable = false;
        }
        else if (playerMain.Money - worker.Cost >= 0 && !isRecruited)
        {
            recruitButton.transform.GetComponentInChildren<Text>().text = "Recruit";
            recruitButton.interactable = true;
        }
    }

    void SetUpButton()
    {
        recruitButton.onClick.AddListener(OnClickRecruit);
    }

    void OnClickRecruit()
    {
        shopMain.RecruitWorker(this);
    }
}

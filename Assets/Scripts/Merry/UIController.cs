using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIController : MonoBehaviour
{
    [Header("InGame")]//////////////////
    public Text lvText;

    public GameObject leavePanel;
    public GameObject shopPanel;
    public GameObject menuPanel;

    public AudioClip inGameMus;


    //    public Text[] statsText;
    //public GameObject statsButton;

    [Header("Shop")]//////////////////////
    public Text ShopCashText;
    public Text ShopText;
    public Text yourText;

    public AudioClip inShopMus;

    public GameObject[] Tab;
    public GameObject[] whichDrillSh;
    public GameObject[] whichBaseSh;
    public GameObject[] whichWheelsSh;
    public GameObject[] whichObSh;
    /*wheelsPanel;
public GameObject basePanel;
public GameObject drillPanel;
public GameObject obPanel;*/

    [Header("Stats")]
    public GameObject[] workerStatsPanel = new GameObject[3];
    public Text[] statsText;
    public Text DrilSatsText;

    public GameObject[] whichDrillSt;
    public GameObject[] whichBaseSt;
    public GameObject[] whichWheelsSt;
    public GameObject[] whichObSt;
    public Image[] Portrits;
    public Sprite[] portritFile;

    [Header("Misk")]
    public int level;
    private int statscount;
    public AudioClip buttonClick;


    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        statscount = 1;
        leavePanel.SetActive(false);
        shopPanel.SetActive(false);
        menuPanel.SetActive(false);
        Debug.Log("Check1");
        lvText.text = "Lv: " + PlayerStats.arc.lv;
        Debug.Log("Check2");
/*        workerStatsPanel[0].SetActive(false);
        workerStatsPanel[1].SetActive(false);
        workerStatsPanel[2].SetActive(false);
        */
    }

    // Update is called once per frame
    void Update()
    {
        //nMouseOver();
    }

    // leval
    

        public void GetLevel(int lv)
    {
        level = lv;
    }

    public void LvUpButtom()
    {
        level = PlayerStats.arc.lv;
       // level = playerStats.arc.playerLevel;
        if(level <=2)
        {
            //PlayerStats.instance.UpdateLevel(true);
           PlayerStats.arc.lv++;
           lvText.text = "Lv: " + PlayerStats.arc.lv;
        }
        else
        {
            
        }
    }
    public void LvDownButtom()
    {

        level = PlayerStats.arc.lv;
        if (level >= 1)
        {
            PlayerStats.arc.lv--;
            lvText.text = "Lv: " + PlayerStats.arc.lv;
        }
        else
        {
        }
    }
    private void LevelText()
    {

    }
    

    //worker stats panel
    /*

    public void statsShowButton()
    {
        Debug.Log("Clicked");
        statscount++;
        if(statscount == 2)
        {
            Debug.Log("2");

            level = PlayerStats.arc.lv;
            for (int i = 0; i < level; i++)
            {
                workerStatsPanel[i].SetActive(true);
            }
        }
        if (statscount == 3)
        {
            Debug.Log("3");

            
            for (int i = 0; i < 3; i++)
            {
                workerStatsPanel[i].SetActive(false);
            }
            statscount = 1;
        }
    }  
*/

    //leave Panel -- DONE

    public void OnLeaveButton()
    {
        leavePanel.SetActive(true);
        AudioManager.am.Play("Button");
    }
    public void QuitButton()
    {
        Application.Quit();
        AudioManager.am.Play("Button");
    }
    public void DontQuitButton()
    {
        leavePanel.SetActive(false);
        AudioManager.am.Play("Button");
    }


    //menu panel
    public void goToMenu()
    {
        menuPanel.SetActive(true);

        PlayerStats.arc.WorkTotal();
        DrilSatsText.text = "" + PlayerStats.arc.ShipName + "'s Work: " + PlayerStats.arc.drilWork + " \n" + "Money: " + PlayerStats.arc.Cash;

        Debug.Log("Now Name");
        for(int u = 0; u < 3; u++)
        {
            Portrits[u].enabled = false;
            statsText[u].text = "";
        }

        //string statText = "";
        for (int i = 0; i < PlayerStats.arc.lv; i++)
        {
            // Debug.Log("" + i );
            Portrits[i].enabled = true;
            Portrits[i].sprite = portritFile[PlayerStats.arc.workersImage[i]];
            statsText[i].text = "Name: " + PlayerStats.arc.Name[i] + "\n" + "Work Contribut: " + PlayerStats.arc.Work[i];
        }
        SetStatIm();
        Debug.Log("x");
        AudioManager.am.Play("Button");
    }
    public void leaveToMenu()
    {
        AudioManager.am.Play("Button");
        menuPanel.SetActive(false);
    }

    private void SetStatIm()
    {
        for(int i = 0; i < 3; i++)
        {
            whichBaseSt[i].SetActive(false);
            whichWheelsSt[i].SetActive(false);
            whichDrillSt[i].SetActive(false);
            whichObSt[i].SetActive(false);
        }
        /**/
        whichBaseSt[PlayerStats.arc.whichImage[1]].SetActive(true);
        whichWheelsSt[PlayerStats.arc.whichImage[0]].SetActive(true);
        whichDrillSt[PlayerStats.arc.whichImage[2]].SetActive(true);
        whichObSt[PlayerStats.arc.whichImage[3]].SetActive(true);

        Debug.Log("S");
    }


    //shop panel - figure buttns,
    public void goToShopButton()
    {
        string what = "" + PlayerStats.arc.ShipName;
        shopPanel.SetActive(true);
        ShopCashText.text = "Your Cash $" + PlayerStats.arc.Cash;
        ShopText.text = "Everything cost $25.";
        yourText.text = "Your ship " + what + " is at Lv - " + PlayerStats.arc.lv;
        ShopImage();
        AudioManager.am.Play("Button");
    }

    public void leaveShopButton()
    {
        shopPanel.SetActive(false);
        AudioManager.am.Play("Button");
    }
    public void MoreCash()
    {
        float num = 50;
        PlayerStats.arc.Cash += num;
        ShopCashText.text = "Your Cash $" + PlayerStats.arc.Cash;
    }

    private void ShopImage()
    {
        for (int i = 0; i < 3; i++)
        {
            whichBaseSh[i].SetActive(false);
            whichWheelsSh[i].SetActive(false);
            whichDrillSh[i].SetActive(false);
            whichObSh[i].SetActive(false);
        }
        /**/
        whichBaseSh[PlayerStats.arc.whichImage[1]].SetActive(true);
        whichWheelsSh[PlayerStats.arc.whichImage[0]].SetActive(true);
        whichDrillSh[PlayerStats.arc.whichImage[2]].SetActive(true);
        whichObSh[PlayerStats.arc.whichImage[3]].SetActive(true);
    }

    //Wheels Buttons
    public void TabToWheels()
    {
        for (int i = 0; i < 4; i++)
        {
            Tab[i].SetActive(false);
        }
        Tab[0].SetActive(true);
        AudioManager.am.Play("Button");
    }
    public void W0Button()
    {
        CanBuy(1, 0);
    }
    public void W1Button()
    {
        CanBuy(1, 1);
    }
    public void W2Button()
    {
        CanBuy(1, 2);
    }

    //Base Buttons
    public void TabToBase()
    {
        for (int i = 0; i < 4; i++)
        {
            Tab[i].SetActive(false);
        }
        Tab[1].SetActive(true);
        AudioManager.am.Play("Button");
    }
    public void Base0Button()
    {
        CanBuy(4, 0);
    }
    public void Base1Button()
    {
        CanBuy(4, 1);
    }
    public void Base2Button()
    {
        CanBuy(4, 2);
    }

    //Drill Buttons
    public void TabToDrill()
    {
        for (int i = 0; i < 4; i++)
        {
            Tab[i].SetActive(false);
        }
        Tab[2].SetActive(true);
    }
    public void Drill0Button()
    {
        CanBuy(2, 0);
    }
    public void Drill1Button()
    {
        CanBuy(2, 1);
    }
    public void Drill2Button()
    {
        CanBuy(2, 2);
    }

    //Observitory Buttons
    public void TabToOb()
    {
        for (int i = 0; i < 4; i++)
        {
            Tab[i].SetActive(false);
        }
        Tab[3].SetActive(true);
        AudioManager.am.Play("Button");
    }
    public void Ob0Button()
    {
        CanBuy(3, 0);
    }
    public void Ob1Button()
    {
        CanBuy(3, 1);
    }
    public void Ob2Button()
    {
        CanBuy(3, 2);
    }

    //Can buy?

    private void CanBuy(int what, int lv)
    {
        int check = 25;
        if(check <= PlayerStats.arc.Cash)
        {
            //Wheels
            if (what == 1 && lv == 0)
            {
                PlayerStats.arc.ChangeWeels(0);
                PlayerStats.arc.whichImage[0] = 0;
            }
            if (what == 1 && lv == 1)
            {
                PlayerStats.arc.ChangeWeels(1);
                PlayerStats.arc.whichImage[1] = 0;
            }
            if (what == 1 && lv == 2)
            {
                PlayerStats.arc.ChangeWeels(2);
                PlayerStats.arc.whichImage[2] = 0;
            }
            //Drill
            if (what == 2 && lv == 0)
            {
                PlayerStats.arc.whichImage[2] = 0;
            }
            if (what == 2 && lv == 1)
            {
                PlayerStats.arc.whichImage[2] = 1;
            }
            if (what == 2 && lv == 2)
            {
                PlayerStats.arc.whichImage[2] = 2;
            }
            //Observitory
            if (what == 3 && lv == 0)
            {
                PlayerStats.arc.ChangeObservitory(0);
                PlayerStats.arc.whichImage[3] = 0;
            }
            if (what == 3 && lv == 1)
            {
                PlayerStats.arc.ChangeObservitory(1);
                PlayerStats.arc.whichImage[3] = 1;
            }
            if (what == 3 && lv == 2)
            {
                PlayerStats.arc.ChangeObservitory(2);
                PlayerStats.arc.whichImage[3] = 2;
            }
            //Base
            if (what == 4 && lv == 0)
            {
                PlayerStats.arc.ChangeBase(0);
                PlayerStats.arc.whichImage[1] = 0;
            }
            if (what == 4 && lv == 1)
            {
                PlayerStats.arc.ChangeBase(1);
                PlayerStats.arc.whichImage[1] = 1;
            }
            if (what == 4 && lv == 2)
            {
                PlayerStats.arc.ChangeBase(2);
                PlayerStats.arc.whichImage[1] = 2;
            }

            PlayerStats.arc.Cash -= 25;
            ShopCashText.text = "Your Cash $" + PlayerStats.arc.Cash;
            ShopText.text = "Everything cost $25.";
            ShopImage();
            AudioManager.am.Play("Button");
        }
        else
        {
            ShopText.text = "You don't have enoughfe money. Everything cost $25.";
        }
    }
}

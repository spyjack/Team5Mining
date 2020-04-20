using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats arc;
    //public Sprite[] portritFile;

    public int lv;

    public float Cash = 100;

    public float drilWork;

    //public bool[] Wheels = new bool[3] {true, false, false };
    ////public bool[] Drill = new bool[3] { true, false, false };
    public GameObject[] Base = new GameObject[3];
    public GameObject[] Observitory = new GameObject[3];

    public int[] whichImage = new int[4]; //wheels, base, drill, Ob.

    public string ShipName = "Rusty";
    public string[] Name = new string[3] { "Sam", "Dean", "Cass"};
    public int[] Work = new int[3] { 4, 4, 4};
    public int[] workersImage = new int[3] {1, 6, 7 };
    public Animator anWheel;
    public Animator anDrill;
    public int movenum;
    [SerializeField] private KeyCode moveKey;

    private void Awake()
    {
        // Wheel.SetInteger("Wheel", 0);
        arc = this;
        lv = 1;
    }
    public void Start()
    {
        anDrill.SetInteger("whichDrill", 0);
        anDrill.SetBool("isDigging", true);
        anWheel.SetInteger("whichWheele", 0);
        anWheel.SetBool("isMoving", true);
        UpdatePlayerSprite();
    }
    private void Update()
    {
        if (Input.GetKeyDown(moveKey))
        {
            Move();
        }
    }

    public void UpdatePlayerSprite()
    {
        for(int i = 0; i < 3; i++)
        {
            Base[i].SetActive(false);
            Observitory[i].SetActive(false);
        }
        Base[whichImage[1]].SetActive(true);
        Observitory[whichImage[3]].SetActive(true);
    }

    public void WorkTotal()
    {
        if(lv == 1)
        {
            drilWork = 2;
            for (int i = 0; i > lv; i++)
            {
                drilWork += Work[i];
            }
        }
        if (lv == 2)
        {
            drilWork = 4;
            for (int i = 0; i > lv; i++)
            {
                drilWork += Work[i];
            }
        }
        if (lv == 3)
        {
            drilWork = 6;
            for (int i = 0; i > lv; i++)
            {
            drilWork += Work[i];
            }
        }


    }

    public void ChangeWeels(int num)
    {
        anWheel.SetInteger("whichWheele", num);
       /* if (num == 0)
        {
            anWheel.SetInteger("whichWheele", 0);
            
            Wheels[0] = true;
            Wheels[1] = false;
            Wheels[2] = false;
        }

        if (num == 1)
        {
            anWheel.SetInteger("whichWheele", 1);
          
            Wheels[0] = false;
            Wheels[1] = true;
            Wheels[2] = false;
        }

        if (num == 2)
        {
            anWheel.SetInteger("whichWheele", 2);
            
            Wheels[0] = false;
            Wheels[1] = false;
            Wheels[2] = true;
        }*/
    }

    public void ChangeBase(int num)
    {
        if(num == 0)
        {

            Base[0].SetActive(true);
            Base[1].SetActive(false);
            Base[2].SetActive(false);
        }
        if (num == 1)
        {
            Base[0].SetActive(false);
            Base[1].SetActive(true);
            Base[2].SetActive(false);
        }
        if (num == 2)
        {
            Base[0].SetActive(false);
            Base[1].SetActive(false);
            Base[2].SetActive(true);
        }
    }

    private void Move()
    {
        movenum++;
        if (movenum == 1)
        {
            anWheel.SetBool("isGoinh", true);
        }
        if (movenum == 2)
        {
            anWheel.SetBool("isGoing", false);
            movenum = 0;
        }
    }

    public void ChangeDrill(int num)
    {
        if (num == 0)
        {
            anDrill.SetInteger("whichDrill", 0);
            /*
            Drill[0] = true;
            Drill[1] = false;
            Drill[2] = false;*/
        }
        if (num == 1)
        {
            anDrill.SetInteger("whichDrill", 0);
            /*
            Drill[0] = false;
            Drill[1] = true;
            Drill[2] = false;*/
        }
        if (num == 2)
        {
            anDrill.SetInteger("whichDrill", 0);
            /*
            Drill[0] = false;
            Drill[1] = false;
            Drill[2] = true;*/
        }
    }

    public void ChangeObservitory(int num)
    {
        if (num == 0)
        {
            Observitory[0].SetActive(true);
            Observitory[1].SetActive(false);
            Observitory[2].SetActive(false);
        }
        if (num == 1)
        {
            Observitory[0].SetActive(false);
            Observitory[1].SetActive(true);
            Observitory[2].SetActive(false);
        }
        if (num == 2)
        {
            Observitory[0].SetActive(false);
            Observitory[1].SetActive(false);
            Observitory[2].SetActive(true);
        }
    }
}

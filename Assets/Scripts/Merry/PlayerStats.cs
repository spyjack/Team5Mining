using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats arc;

    public int lv;

    public float Cash = 100;

    public float drilWork;

    public bool[] Wheels = new bool[3] {true, false, false };
    public bool[] Drill = new bool[3] { true, false, false };
    public GameObject[] Base = new GameObject[3];
    public GameObject[] Observitory = new GameObject[3];

    public int[] whichImage = new int[4]; //wheels, base, drill, Ob.

    public string ShipName = "Rusty";
    public string[] Name = new string[3] { "Sam", "Dean", "Cass"};
    public int[] Work = new int[3] { 4, 4, 4};

    public Animator Wheel;
    public Animator anDrill;
    public int num;
    [SerializeField] private KeyCode moveKey;

    private void Awake()
    {
        // Wheel.SetInteger("Wheel", 0);
        anDrill.SetInteger("whichDrill", 0);
        anDrill.SetBool("isDigging", true);
        arc = this;
        lv = 1;
    }
    public void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(moveKey))
        {
            Move();
        }
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
         if(num == 0)
        {
            Wheel.SetInteger("Wheel", 0);
            Wheels[0] = true;
            Wheels[1] = false;
            Wheels[2] = false;
        }

        if (num == 1)
        {
            Wheel.SetInteger("Wheel", 1);
            Wheels[0] = false;
            Wheels[1] = true;
            Wheels[2] = false;
        }

        if (num == 2)
        {
            Wheel.SetInteger("Wheel", 3);
            Wheels[0] = false;
            Wheels[1] = false;
            Wheels[2] = true;
        }
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
        num++;
        if (num == 1)
        {
            Wheel.SetBool("Moveing", false);
        }
        if (num == 2)
        {
            Wheel.SetBool("Moveing", true);
            num = 0;
        }
    }

    public void ChangeDrill(int num)
    {
        if (num == 0)
        {
            Drill[0] = true;
            Drill[1] = false;
            Drill[2] = false;
        }
        if (num == 1)
        {
            Drill[0] = false;
            Drill[1] = true;
            Drill[2] = false;
        }
        if (num == 2)
        {
            Drill[0] = false;
            Drill[1] = false;
            Drill[2] = true;
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

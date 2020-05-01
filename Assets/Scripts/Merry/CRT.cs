using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CRT : MonoBehaviour
{
    public Image Im_0;

    //public GameObject SpawnPoint;


    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("CRT"))
        {
            Debug.Log("Collided");
            Instantiate(Im_0, new Vector2(0.0f, -872.0f), Quaternion.identity);
            other.gameObject.SetActive(false);
        }
    }
}

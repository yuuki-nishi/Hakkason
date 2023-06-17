using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Slider hPSlider;
    public int damage = 10;

    private bool _hitPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("LeftShift");
            if(_hitPlayer)
            {
                hPSlider.value -= damage;
                if(hPSlider.value <= 0)
                {
                    hPSlider.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }

    //Playerと接触しているか
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("HIT");
        if(collider.gameObject.tag == "Player")
        {
            _hitPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            _hitPlayer = false;
        }
    }
}

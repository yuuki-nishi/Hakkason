using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPGauge : MonoBehaviour
{
    public float maxHP = 100;
    public float damage = 10;
    
    private Vector3 position;
    private float initialX;
    private Vector3 localScale;
    private float maxScale;
    private float hpPercent = 1;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        initialX = position.x;
        localScale = transform.localScale;
        maxScale = localScale.x;
        Debug.Log($"{initialX} {maxScale}");
    }

    // Update is called once per frame
    void Update()
    {
        void Attacked()
        {
            hpPercent -= damage / maxHP;
            position.x = initialX - (1 - hpPercent) * maxScale / 2;
            transform.position = position;
            localScale.x = hpPercent * maxScale;
            transform.localScale = localScale;
            Debug.Log($"{position.x} {localScale.x}");
            if(localScale.x <= 0)
            {
                if(gameObject.tag == "Player"){
                    //GameOver
                }
                else
                {
                    //Killed
                    Debug.Log("Killed");
                    Destroy(gameObject);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Attacked();
        }
        
    }
}

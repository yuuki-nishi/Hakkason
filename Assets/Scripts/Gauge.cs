using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    public float damage = 10;
    public float cure = 30;
    
    public Slider _slider;
    private float maxValue = 100;
    private float value;

    // Start is called before the first frame update
    void Start()
    {
        value = maxValue;
        _slider.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        void Gauge(float dmg)
        {
            value -= dmg;
            if(value > maxValue)
            {
                value = maxValue;
            }
            if(value <= 0)
            {
                value = 0;
                _slider.gameObject.SetActive(false);
            }
            _slider.value = value;
            Debug.Log(value);
        }

        void Damage(){ Gauge(damage); }
        void Cure(){ Gauge(-cure); }

        //動作確認
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Damage();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Cure();
        }
    }
}

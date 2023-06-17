using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    public float breath = 1f;
    private float _timer;
    Gauge _gauge;

    // Start is called before the first frame update
    void Start()
    {
        _gauge = GetComponent<Gauge>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > 3f)
        {
            _timer = 0f;
            _gauge.Damage(breath);
        } 
    }
}

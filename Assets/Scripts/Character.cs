using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public virtual class Character
{
    public Vector2 Location;
    public float HP = 100;
    public float MP = 100;
    protected Vector2 initloc = {0,0};


    Character(Vector2 initloc = this.initloc){
        this.Location = initloc;
    }
    public void move(Vector2 targetloc){
        this.Location = targetloc;
    }
}
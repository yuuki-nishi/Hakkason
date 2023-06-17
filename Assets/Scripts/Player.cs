using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player
{
    public Vector2 Location;
    public float HP = 100;
    public float MP = 100;


    Player(int xloc=0,int yloc=0){
        Vector2 vec = Vector2.zero;
        vec.x = xloc;
        vec.y = yloc;
        Debug.Assert(xloc % 2==0);
        Debug.Assert(yloc % 2==0);
        this.Location = vec;
    }
    
}
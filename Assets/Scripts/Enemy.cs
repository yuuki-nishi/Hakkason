using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Character;
public class Enemy : Character.Character
{
    
    public float HP = 50;
    public float MP = 30;
    public void randommove(){

    }
    public Context attacked(Player player){
        return Context.None;
    }
    public Context attacktoplayer(Player player){
        return Context.None;
    }
}
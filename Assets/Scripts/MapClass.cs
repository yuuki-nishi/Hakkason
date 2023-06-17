using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClass
{
    int Xsize = 3;
    int Ysize = 2;
    Maptime [,] MapData = new Maptile[this.Xsize,this.Ysize]{ { Maptile.Floor, Maptile.Floor, Maptile.Floor },
                                  { Maptile.Floor, Maptile.Wall, Maptile.Ladder }};
    public bool isFloor(int x,int y){
        Maptile attr = this.MapData[y][x];
        return attr == Maptile.Fllor;
    }
}

enum Maptile
{
    Floor,
    Wall,
    Ladder,

}
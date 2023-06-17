using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class MapClass
{
    int Xsize = 3;//奇数
    int Ysize = 3;
    Enums.Maptile [,] MapData = new Enums.Maptile[3,3]{ { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Floor },
                                  { Enums.Maptile.Floor, Enums.Maptile.Wall, Enums.Maptile.Ladder },
                                  { Enums.Maptile.Floor, Enums.Maptile.Wall, Enums.Maptile.Floor }};
    public bool isMovable(int x,int y){
        Enums.Maptile attr = this.MapData[y,x];
        return attr == Enums.Maptile.Floor;
    }
}

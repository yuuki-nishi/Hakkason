using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class MapClass
{
    public int Xsize = 3;//奇数
    public int Ysize = 3;
    public Enums.Maptile [,] MapData = new Enums.Maptile[3,3]{ { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Floor },
                                  { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Ladder },
                                  { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Floor }};
    public bool isMovable(int x,int y){
        Debug.Assert(x>=0);
        Debug.Assert(y>=0);
        Debug.Assert(x<this.Xsize);
        Debug.Assert(y<this.Ysize);
        Enums.Maptile attr = this.MapData[y,x];
        return attr == Enums.Maptile.Floor;
    }
}

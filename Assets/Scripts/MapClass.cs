using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class MapClass
{
    public int Xsize;//奇数
    public int Ysize;
    private MapCreater mapcreater = new MapCreater();
    public Enums.Maptile [,] MapData = new Enums.Maptile[3,3]{ { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Wall },
                                  { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Ladder },
                                  { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Floor }};
    public MapClass(){
        this.mapcreater.CreateMap(10,10,3);
        this.Xsize = mapcreater.width;
        this.Ysize = mapcreater.height;
    }
    public bool isMovable(int x,int y){
        Debug.Assert(x>=0);
        Debug.Assert(y>=0);
        Debug.Assert(x<this.Xsize);
        Debug.Assert(y<this.Ysize);
        Enums.Maptile attr = this.mapcreater.Get(x,y);
        return this.mapcreater.isFloor(x,y);
    }
    public Enums.Maptile Get(int x,int y){
        return this.mapcreater.Get(x,y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClass
{
    int Xsize = 3;
    int Ysize = 2;
    Vector2 MapData = new Vector2(this.Xsize,this.Ysize){ { Maptile.Floor, Maptile.Floor, Maptile.Floor },
                                  { Maptile.Floor, Maptile.Wall, Maptile.Ladder }};
}

enum Maptile
{
    Floor,
    Wall,
    Ladder,

}
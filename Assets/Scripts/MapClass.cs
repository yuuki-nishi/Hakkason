using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class MapClass
{
    public int Xsize;//奇数
    public int Ysize;
    public readonly int startx,starty;
    public List<Enemy> enemies;
    public List<Senbei> senbeis;
    private MapCreater mapcreater = new MapCreater();
    public Enums.Maptile [,] MapData = new Enums.Maptile[3,3]{ { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Wall },
                                  { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Ladder },
                                  { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Floor }};
    public MapClass(){
        var startxy= this.mapcreater.CreateMap(10,10,3);
        this.Xsize = mapcreater.width;
        this.Ysize = mapcreater.height;
        this.startx = startxy.Item1;
        this.starty = startxy.Item2;
        List<Vector2> inititems = this.mapcreater.GetItemPosition();
        List<Vector2> initenemies = this.mapcreater.GetEnemyPosition();
        List<Enemy> enemies = new List<Enemy>();
        List<Senbei> senbeis = new List<Senbei>();
        foreach(Vector2 v in inititems){
            Senbei s = new Senbei();
            s.Location = v;
            senbeis.Add(s);
        }
        foreach(Vector2 v in initenemies){
            Enemy s = new Enemy();
            s.Location = v;
            enemies.Add(s);
        }
        this.senbeis = senbeis;
        this.enemies = enemies;
    }
    public bool isMovable(int x,int y){
        Debug.Assert(x>=0);
        Debug.Assert(y>=0);
        Debug.Assert(x<this.Xsize);
        Debug.Assert(y<this.Ysize);
        Enums.Maptile attr = this.mapcreater.GetTile(x,y);
        return this.mapcreater.isFloor(x,y);
    }
    public Enums.Maptile Get(int x,int y){
        return this.mapcreater.GetTile(x,y);
    }

}

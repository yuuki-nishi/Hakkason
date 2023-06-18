using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class MapClass
{
    public int Xsize;//奇数
    public int Ysize;
    public readonly int startx,starty;
    public List<GameObject> enemies;
    public List<GameObject> senbeis;
    private MapCreater mapcreater = new MapCreater();
    public Enums.Maptile [,] MapData = new Enums.Maptile[3,3]{ { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Wall },
                                  { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Ladder },
                                  { Enums.Maptile.Floor, Enums.Maptile.Floor, Enums.Maptile.Floor }};
    public MapClass(){
        var startxy= this.mapcreater.CreateMap(50,50,5);
        this.Xsize = mapcreater.width;
        this.Ysize = mapcreater.height;
        this.startx = startxy.Item1;
        this.starty = startxy.Item2;
        List<Vector2> inititems = this.mapcreater.GetItemPosition();
        List<Vector2> initenemies = this.mapcreater.GetEnemyPosition();
        List<GameObject> enemies = new List<GameObject>();
        List<GameObject> senbeis = new List<GameObject>();
        float z = -9;
        foreach(Vector2 v in inititems){
            GameObject o = new GameObject();
            o.AddComponent<Senbei>();
            o.GetComponent<Senbei>().Location = v;
            Vector3 fortransform = new Vector3(v.x,v.y,z);
            o.transform.position= fortransform;
            o.AddComponent<SpriteRenderer>();
            senbeis.Add(o);
        }
        foreach(Vector2 v in initenemies){
            GameObject o = new GameObject();
            o.AddComponent<Enemy>();
            o.GetComponent<Enemy>().Location = v;
            Vector3 fortransform = new Vector3(v.x,v.y,z);
            o.transform.position= fortransform;
            o.AddComponent<SpriteRenderer>();
            enemies.Add(o);
        }
        this.senbeis = senbeis;
        this.enemies = enemies;
        Debug.Log("item eny log");
        Debug.Log(this.enemies.Count);
        Debug.Log(this.senbeis.Count);
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

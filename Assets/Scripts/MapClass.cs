using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClass
{
    const int RangeSizeMin = 50;
    const int RangeWidthMin = 7;

    const int RoomSizeMin = 25;
    const int roomWidthMin = 4;
    
    int height, width;

    Maptile[,] map;

    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    // 標準偏差つけるなどしてもいいかも？
    int roomCutRandom(int Min, int Max) {
        return Random.Range(Min, Max);
    }

    (int minX, int minY, int maxX, int maxY) roomSizeRandom((int minX, int minY, int maxX, int maxY) RoomRange) {
        return RoomRange;
    }

    public (int X, int Y) CreateMap (int Height, int Width, int Rooms){

        height = Height;
        width = Width;
        map = new Maptile[Height, Width];

        for (int i = 0; i < Height; i++ ) {
            for (int j = 0; j < Width; j++ ) {
                map[i, j] = Maptile.Wall;
            } 
        }
        
        // 部屋、道の分割   どちらも 閉開空間 でもつ [minX, maxX) 
        List<(int minX, int minY, int maxX, int maxY)> room = new List<(int minX, int minY, int maxX, int maxY)>();
        List<(int minX, int minY, int maxX, int maxY)> road = new List<(int minX, int minY, int maxX, int maxY)>();
        room.Add((0, 0, Height, Width));
        
        while (room.Count < Rooms) {

            room.Sort((a,b)=> (b.maxY-b.minY)*(b.maxX-b.minX) - (a.maxY-a.minY)*(a.maxX-a.minX));
            
            bool change = false;

            for (int roomIndex = 0; roomIndex < room.Count; roomIndex++ ) {
                int minX = room[roomIndex].minX;
                int minY = room[roomIndex].minY;
                int maxX = room[roomIndex].maxX;
                int maxY = room[roomIndex].maxY;

                if (maxX - minX < RangeWidthMin * 2 + 1 && maxY - minY < RangeWidthMin * 2 + 1) continue;

                if (maxY - minY < maxX - minX) {

                    if ((maxY - minY) * ((maxX - minX - 1) / 2) < RangeSizeMin ) continue;

                    int roadX = roomCutRandom(minX + RangeWidthMin, maxX - RangeWidthMin);

                    room.Add((minX, minY, roadX, maxY));
                    road.Add((roadX, minY, roadX + 1,maxY));
                    room.Add((roadX + 1, minY, maxX, maxY));
                    room.RemoveAt(roomIndex);
                    change = true;
                    break;
                } else {

                    if ((maxX - minX) * ((maxY - minY - 1) / 2) < RangeSizeMin ) continue;

                    int roadY = roomCutRandom(minY + RangeWidthMin, maxY - RangeWidthMin);

                    room.Add((minX, minY, maxX, roadY));
                    road.Add((minX, roadY, maxX, roadY + 1));
                    room.Add((minX, roadY + 1, maxX, maxY));
                    room.RemoveAt(roomIndex);
                    change = true;
                    break;
                }

            }

            if(change == false) {
                break;
            }
        }

        //部屋、道の情報からmapの作成をする

        for (int i = 0; i < room.Count; i++ ) {
            (int minX, int minY, int maxX, int maxY) RoomSize = roomSizeRandom(room[i]);

            for (int x = RoomSize.minX; x < RoomSize.maxX; x++ ) {
                for (int y = RoomSize.minY; y < RoomSize.maxY; y++ ) {
                    map[x, y] = Maptile.Floor;
                }
            }

            int roadNum = Random.Range(2,4);

            for (int j = 0; j < roadNum; j++ ) {
                
            }
        }

        // 道の建設

        string res = "";
        for (int i = 0; i < height; i++ ) {
            for (int j = 0; j < width; j++) {
                res += map[i,j]==Maptile.Wall?'.':'#';
            }
            res += "\n";
        }
        Debug.Log(res);
    }
    
    public bool IsOutOfRange(int X, int Y) {
        return X < 0 || height <= X || Y < 0 || width <= Y;
    }
    
    public int Get (int X, int Y) {
        if (IsOutOfRange(X, Y)) return Wall;
        return map[X, Y];
    }
    
    public bool isFloor(int X,int Y){
        return Get(X, Y) == Maptile.Floor;
    }
    
}

enum Maptile
{
    Floor,
    Wall,
    Ladder,
    Start,
}
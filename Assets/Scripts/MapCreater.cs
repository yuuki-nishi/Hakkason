using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maptile = Enums.Maptile;
// enum Maptile
// {
//     Floor,
//     Wall,
//     Ladder,
// }

class Deque<T>
{
    T[] buf;
    int offset, count, capacity;
    public int Count { get { return count; } }
    public Deque(int cap) { buf = new T[capacity = cap]; }
    public Deque() { buf = new T[capacity = 16]; }
    private int getIndex(int index)
    {
        var ret = index + offset;
        if (ret >= capacity)
            ret -= capacity;
        return ret;
    }
    public void PushFront(T item)
    {
        if (count == capacity) Extend();
        if (--offset < 0) offset += buf.Length;
        buf[offset] = item;
        ++count;
    }
    public T PopFront()
    {
        --count;
        var ret = buf[offset++];
        if (offset >= capacity) offset -= capacity;
        return ret;
    }
    public void PushBack(T item)
    {
        if (count == capacity) Extend();
        var id = count++ + offset;
        if (id >= capacity) id -= capacity;
        buf[id] = item;
    }
    public T PopBack()
    {
        return buf[getIndex(--count)];
    }
    private void Extend()
    {
        T[] newBuffer = new T[capacity << 1];
        if (offset > capacity - count)
        {
            var len = buf.Length - offset;
            System.Array.Copy(buf, offset, newBuffer, 0, len);
            System.Array.Copy(buf, 0, newBuffer, len, count - len);
        }
        else System.Array.Copy(buf, offset, newBuffer, 0, count);
        buf = newBuffer;
        offset = 0;
        capacity <<= 1;
    }
}

public class MapCreater
{

    const int RangeSizeMin = 50;
    const int RangeWidthMin = 7;
    const int RoomSizeMin = 16;
    const int roomWidthMin = 4;
    
    public int height, width;

    Maptile[,] map;

    public int EnemyCount;
    List<Vector2> EnemyPosition;

    public int ItemCount;
    List<Vector2> ItemPosition;

    public MapCreater(){
        Random.InitState(System.DateTime.Now.Millisecond);
        EnemyPosition = new List<Vector2>();
        ItemPosition = new List<Vector2>();
    }

    // 標準偏差つけるなどしてもいいかも？
    int roomCutRandom(int Min, int Max) {
        return Random.Range(Min, Max);
    }

    (int minX, int minY, int maxX, int maxY) roomSizeRandom((int minX, int minY, int maxX, int maxY) RoomRange) {
        int nX = Random.Range(RoomRange.minX + 1, RoomRange.minX + (RoomRange.maxX - RoomRange.minX ) / 3 );
        int xX = Random.Range(RoomRange.maxX - (RoomRange.maxX - RoomRange.minX ) / 3, RoomRange.maxX - 1 );
        int nY = Random.Range(RoomRange.minY + 1, RoomRange.minY + (RoomRange.maxY - RoomRange.minY ) / 3 );
        int xY = Random.Range(RoomRange.maxY - (RoomRange.maxY - RoomRange.minY ) / 3, RoomRange.maxY - 1 );

        while(xX - nX < roomWidthMin){
            if(nX == RoomRange.minX + 1) xX++;
            else if(xX == RoomRange.maxX - 1) nX--;
            else if(Random.Range(0,2) == 1) xX++;
            else nX--;
        }
        while(xY - nY < roomWidthMin){
            if(nY == RoomRange.minY + 1) xY++;
            else if(xY == RoomRange.maxY - 1) nY--;
            else if(Random.Range(0,2) == 1) xY++;
            else nY--;
        }

        return (nX, nY, xX, xY);
    }

    void connectAllRoom(int startX, int startY){
        Deque<(int,int)>see = new Deque<(int, int)>();
        int[,] cost = new int[height, width];
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                cost[i,j] = 100000;
            }
        }
        cost[startX,startY] = 0;
        see.PushBack((startX, startY));

        List<int> dir = new List<int>(){0,1,2,3};
        int[] dx = {0,1,0,-1};
        int[] dy = {1,0,-1,0};
        while (see.Count != 0) {
            (int X, int Y) pos = see.PopFront();
            if(cost[pos.X, pos.Y] != 0 && map[pos.X, pos.Y] == Maptile.Floor ){
                int X = pos.X, Y = pos.Y;
                for(int i = 0; i < 4; i++){
                    if(cost[X + dx[dir[i]], Y + dy[dir[i]]] == cost[X, Y]){
                        X = X + dx[dir[i]];
                        Y = Y + dy[dir[i]];
                        map[X, Y] = Maptile.Floor;
                        break;
                    }
                }
                while (cost[X, Y] != 0) {
                    for (int i = 0; i < 4; i++){
                        int p = Random.Range(i,4);
                        (dir[i], dir[p]) = (dir[p], dir[i]);
                    }
                    for(int i = 0; i < 4; i++){
                        if(cost[X + dx[dir[i]], Y + dy[dir[i]]] == cost[X, Y] - 1){
                            X = X + dx[dir[i]];
                            Y = Y + dy[dir[i]];
                            map[X, Y] = Maptile.Floor;
                            break;
                        }
                    }
                }
                connectAllRoom(startX, startY);
                break;
            }

            for (int i = 0; i < 4; i++){
                int p = Random.Range(i,4);
                (dir[i], dir[p]) = (dir[p], dir[i]);
            }
            
            for (int i = 0; i < 4; i++) {
                if(!IsOutOfRange(pos.X + dx[dir[i]], pos.Y + dy[dir[i]])){
                    int nc = cost[pos.X, pos.Y] + (map[pos.X + dx[dir[i]], pos.Y + dy[dir[i]]] == Maptile.Wall?1:0);
                    if(cost[pos.X + dx[dir[i]], pos.Y + dy[dir[i]]] > nc){
                        cost[pos.X + dx[dir[i]], pos.Y + dy[dir[i]]] = nc;
                        if(nc != cost[pos.X, pos.Y]) see.PushBack((pos.X + dx[dir[i]], pos.Y + dy[dir[i]]));
                        else see.PushFront((pos.X + dx[dir[i]], pos.Y + dy[dir[i]]));
                    }
                }
            }
        }
    }

    void deleteWasteRoad(){

        for (int i = 0; i < height; i++) {
            map[i, 0] = Maptile.Wall;
            map[i, width - 1] = Maptile.Wall; 
        }
        for (int i = 0; i < width; i++ ) {
            map[0,i] = Maptile.Wall;
            map[height - 1, i] = Maptile.Wall;
        }

        Queue<(int X,int Y)> see = new Queue<(int X, int Y)>();
        for (int i = 0; i < height; i++){
            for (int j = 0; j < width; j++){
                see.Enqueue((i,j));
            }
        }

        while (see.Count != 0) {
            (int X, int Y)pos = see.Peek();
            see.Dequeue();
            if (GetTile(pos.X,pos.Y) == Maptile.Wall) continue;
            int cnt = 0;
            if (GetTile(pos.X + 1, pos.Y) == Maptile.Wall) cnt++;
            if (GetTile(pos.X - 1, pos.Y) == Maptile.Wall) cnt++;
            if (GetTile(pos.X, pos.Y + 1) == Maptile.Wall) cnt++;
            if (GetTile(pos.X, pos.Y - 1) == Maptile.Wall) cnt++;
            if (cnt == 3) {
                map[pos.X, pos.Y] = Maptile.Wall;
                if (GetTile(pos.X + 1, pos.Y) != Maptile.Wall) see.Enqueue((pos.X + 1, pos.Y));
                if (GetTile(pos.X - 1, pos.Y) != Maptile.Wall) see.Enqueue((pos.X - 1, pos.Y));
                if (GetTile(pos.X, pos.Y + 1) != Maptile.Wall) see.Enqueue((pos.X, pos.Y + 1));
                if (GetTile(pos.X, pos.Y - 1) != Maptile.Wall) see.Enqueue((pos.X, pos.Y - 1));
            }
        }

    }

    // この関数を呼び出すことで 高さがHeight, 横幅がWidth 部屋の数がRooms(mapの大きさが足りないとそれに応じて少なくなります)のマップを生成します
    // 戻り値はこのマップのスタート地点です
    public (int X, int Y) CreateMap (int Height, int Width, int Rooms){

        int startX = 0, startY = 0;
        height = Height;
        width = Width;
        map = new Maptile[Height, Width];
        EnemyPosition.Clear();
        EnemyCount = 0;
        ItemPosition.Clear();
        ItemCount = 0;

        for (int i = 0; i < Height; i++ ) {
            for (int j = 0; j < Width; j++ ) {
                map[i, j] = Maptile.Wall;
            } 
        }
        
        // 部屋、道の分割   どちらも 閉開空間 でもつ [minX, maxX) 
        List<(int minX, int minY, int maxX, int maxY)> room = new List<(int minX, int minY, int maxX, int maxY)>();
        List<(int minX, int minY, int maxX, int maxY)> road = new List<(int minX, int minY, int maxX, int maxY)>();
        room.Add((0, 0, Height, Width));

        //部屋の数が十分になるか部屋を分割できなくなるまで部屋を分けていく
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

        int startRoom = Random.Range(0,room.Count);
        int LadderRoom;
        do{
            LadderRoom = Random.Range(0,room.Count);
        }while(startRoom == LadderRoom && room.Count != 1);


        int[] item = new int[room.Count];
        for (int i = 0; i < room.Count; i++ ) {
            item[i] = 0;
        }
        ItemCount = Random.Range(Mathf.Min(2,room.Count) , room.Count+1);
        for (int i = 0; i < ItemCount; i++)item[i] = 1;

        for (int i = 0; i < room.Count; i++ ) {

            (int minX, int minY, int maxX, int maxY) RoomSize = roomSizeRandom(room[i]);

            for (int x = RoomSize.minX; x < RoomSize.maxX; x++ ) {
                for (int y = RoomSize.minY; y < RoomSize.maxY; y++ ) {
                    map[x, y] = Maptile.Floor;
                }
            }

            if (i == startRoom) {
                startX = Random.Range(RoomSize.minX + 1, RoomSize.maxX - 1);
                startY = Random.Range(RoomSize.minY + 1, RoomSize.maxY - 1);
            }

            else {
                int enemyNum = Random.Range(1,3);
                EnemyCount += enemyNum;
                for (int j = 0; j < enemyNum; j++){
                    Vector2 tmpPosition = new Vector2(Random.Range(RoomSize.minX, RoomSize.maxX), Random.Range(RoomSize.minY, RoomSize.maxY));
                    bool same = false;
                    for (int k = 0; k < EnemyPosition.Count; k++) {
                        if(tmpPosition == EnemyPosition[k])same = true;
                    }
                    if (same){
                        j--;
                    }else{
                        EnemyPosition.Add(tmpPosition);
                    }
                }
            }

            if (item[i] == 1) {
                while(true){
                    bool same = false;
                    Vector2 tmpPosition = new Vector2(Random.Range(RoomSize.minX, RoomSize.maxX), Random.Range(RoomSize.minY, RoomSize.maxY));
                    for (int k = 0; k < EnemyPosition.Count; k++) {
                        if (tmpPosition == EnemyPosition[k])same = true;
                    }
                    if (same == false) {
                        ItemPosition.Add(tmpPosition);
                        break;
                    }
                }
            }

            int roadNum = Random.Range(2,4);

            int[] dx = {0,1,0,-1};
            int[] dy = {1,0,-1,0};
            for (int j = 0; j < roadNum; j++ ) {
                int x = Random.Range(RoomSize.minX, RoomSize.maxX);
                int y = Random.Range(RoomSize.minY, RoomSize.maxY);
                int dir = Random.Range(0, 4);
                while (IsIn(x,y,room[i])) {
                    x += dx[dir];
                    y += dy[dir];
                    if (IsOutOfRange(x,y)) break;
                    map[x, y] = Maptile.Floor;
                }
            }

            if (i == LadderRoom) {
                map[Random.Range(RoomSize.minX + 1, RoomSize.maxX - 1), Random.Range(RoomSize.minY + 1, RoomSize.maxY - 1)] = Maptile.Ladder;
            }

        }

        // 道の建設

        for (int i = 0; i < road.Count; i++ ) {
            List<(int X, int Y)> pos = new List<(int X, int Y)>();
            for (int x = road[i].minX; x < road[i].maxX; x++ ) {
                for (int y = road[i].minY; y < road[i].maxY; y++) {
                    pos.Add((x, y));
                }
            }
            while(pos.Count != 0){
                if(map[pos[pos.Count-1].X,pos[pos.Count-1].Y] == Maptile.Wall){
                    pos.RemoveAt(pos.Count - 1);
                }else{
                    break;
                }
            }
            pos.Reverse();
            while(pos.Count != 0){
                if(map[pos[pos.Count-1].X,pos[pos.Count-1].Y] == Maptile.Wall){
                    pos.RemoveAt(pos.Count - 1);
                }else{
                    break;
                }
            }
            for(int j = 0; j < pos.Count; j++ ) {
                map[pos[j].X, pos[j].Y] = Maptile.Floor;
            }
        }

        connectAllRoom(startX, startY);

        deleteWasteRoad();

        return (startX, startY);
    }

    bool IsIn(int X, int Y, (int minX, int minY, int maxX, int maxY) Range ) {
        return Range.minX <= X && X < Range.maxX && Range.minY <= Y && Y < Range.maxY;
    }
    
    public bool IsOutOfRange(int X, int Y) {
        return X < 0 || height <= X || Y < 0 || width <= Y;
    }
    
    public Maptile GetTile (int X, int Y) {
        if (IsOutOfRange(X, Y)) return Maptile.Wall;
        return map[X, Y];
    }
    
    public bool isFloor(int X,int Y) {
        return GetTile(X, Y) == Maptile.Floor;
    }

    public bool isLadder(int X, int Y) {
        return GetTile(X, Y) == Maptile.Ladder;
    }
    
    public bool isMovable(int X,int Y) {
        return GetTile(X, Y) == Maptile.Floor || GetTile(X, Y) == Maptile.Ladder;
    }

    public List<Vector2> GetEnemyPosition() {
        //Debug.Assert(0 <= Index && Index < EnemyCount);
        return EnemyPosition;
    }

    public List<Vector2> GetItemPosition() {
        //Debug.Assert(0 <= Index && Index < ItemCount);
        return ItemPosition;
    }

}
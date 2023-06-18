using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Character;
public class Enemy : Character.Character
{
        
    public float HP = 50;
    public float MP = 50;

    // GameMaster.MapData
    // GameMaster.playerstate

    int Height,Width;
    int[,] map; 
    List<(int X, int Y)> cantposition;

    public Enemy(){
        Height = GameMaster.MapData.Xsize;
        Width = GameMaster.MapData.Ysize;
        map = new int[Height, Width];
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++ ) {
                map[i,j] = GameMaster.MapData.Get(i,j)==Maptile.Wall?1:0; 
            }
        }

        cantposition = new List<(int X, int Y)>();
    }

    bool canMove (int x,int y) {
        if(map[x, y] == 1) return false;
        for (int i = 0; i < cantposition.Count; i++) {
            if(cantposition[i].X == x && cantposition[i].Y == y){
                return false;
            }
        }
        return true;
    }

    public void randommove(){
        cantposition.Clear();
        cantposition.Add(((int)GameMaster.playerstate.Location.x , (int)GameMaster.playerstate.Location.y));
        for (int i = 0; i < GameMaster.MapData.enemies.Count; i++) {
            (int X,int Y) tmp = (((int)GameMaster.MapData.enemies[i].Location.x , (int)GameMaster.MapData.enemies[i].Location.y ));
            if(tmp.X != (int)Location.x || tmp.Y != (int)Location.y) {
                cantposition.Add(tmp);
            }
        }
        for (int i = 0; i < GameMaster.MapData.senbeis.Count; i++) {
            cantposition.Add(((int)GameMaster.MapData.senbeis[i].Location.x , (int)GameMaster.MapData.senbeis[i].Location.y ));
        }

        int[,]dist = new int[Height, Width];
        for (int i = 0; i < Height; i++ ) {
            for (int j = 0; j < Width; j++ ) {
                dist[i, j] = 1000000;
            }
        }
        Queue<(int X, int Y)> see = new Queue<(int X, int Y)>();
        see.Enqueue(((int)GameMaster.playerstate.Location.x, (int)GameMaster.playerstate.Location.y));
        dist[see.Peek().X, see.Peek().Y] = 0;

        int[] dx = {0,1,0,-1};
        int[] dy = {-1,0,1,0};
        while (see.Count != 0) {
            (int X, int Y) pos = see.Dequeue();
            for (int i = 0; i < 4; i++) {
                if (canMove(pos.X + dx[i] , pos.Y + dy[i] ) && dist[pos.X + dx[i] , pos.Y + dy[i]] > dist[pos.X, pos.Y] + 1 ) {
                    dist[pos.X + dx[i] , pos.Y + dy[i]] = dist[pos.X, pos.Y] + 1;
                    see.Enqueue((pos.X + dx[i] , pos.Y + dy[i]));
                }
            }
        }

        direction = Direction.Down;
        if (dist[(int)Location.x, (int)Location.y] < 20) {
            for (int i = 0; i < 4; i++) {
                if (dist[(int)Location.x + dx[i] , (int)Location.y + dy[i]] == dist[(int)Location.x, (int)Location.y] - 1 ) {
                    Location.x += dx[i];
                    Location.y += dy[i];
                    switch(i){
                        case 0:
                        direction = Direction.Down;
                        break;
                        case 1:
                        direction = Direction.Right;
                        break;
                        case 2:
                        direction = Direction.Up;
                        break;
                        default:
                        direction = Direction.Left;
                        break;
                    }
                    break;
                }
            }
        } 
        else {
            MoveRandom();
        }
    }

    void MoveRandom() {
        List<int> dir = new List<int>();
        for (int i = 0; i < 4; i++)dir.Add(i);
        for (int i = 0; i < 4; i++){
            int p = Random.Range(i,4);
            (dir[i], dir[p]) = (dir[p], dir[i]);
        }
        int[] dx = {0,1,0,-1};
        int[] dy = {-1,0,1,0};

        for (int i = 0; i < 4; i++) {
            if(canMove((int)Location.x + dx[dir[i]], (int)Location.y + dy[dir[i]])){
                Location.x += dx[dir[i]];
                Location.y += dy[dir[i]];
                switch(i){
                    case 0:
                    direction = Direction.Down;
                    break;
                    case 1:
                    direction = Direction.Right;
                    break;
                    case 2:
                    direction = Direction.Up;
                    break;
                    default:
                    direction = Direction.Left;
                    break;
                }
                break;
            }
        }
    }

    public Context attacked(Player player){
        return Context.None;
    }
    public Context attacktoplayer(Player player){
        return Context.None;
    }
}
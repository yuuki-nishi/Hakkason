using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    public MapClass MapData;
    public Turn turn = Turn.Player;
    public Player player;
    void Start()
    {
        this.MapData = new MapClass();
        this.player = new Player();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateState(){

        Vector2 movevec = Vector2.zero;//{0,0}
        //エンターキーが入力された場合「true」
        if (Input.GetKey(KeyCode.Up)){
            movevec += Vector2.up;
        }
        if (Input.GetKey(KeyCode.Down)){
            movevec += Vector2.down;
        }
        if (Input.GetKey(KeyCode.Left)){
            movevec += Vector2.left;
        }
        if (Input.GetKey(KeyCode.Right)){
            movevec += Vector2.right;
        }
        movetargetloc = movevec + this.player.Location;

    }
    void Display(){

    }
}
enum Turn
{
    Player,
    Turn,

}
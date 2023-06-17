using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    public MapClass MapData;
    public Turn turn = Turn.Player;
    public Player player;
    public Enemy[] enemies;
    public int enemynum = 3;
    public int inputkey_cooltime = 30;
    public GameObject Maplayer;
    public Camera camera;

    void Start()
    {
        this.MapData = new MapClass();
        this.player = new Player();
        this.enemies = new Enemy[enemynum];
        for (int i = 0;i < this.enemynum;i++){
            this.enemies[i] = new Enemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputkey_cooltime > 0){
            inputkey_cooltime -= 1;

        }else{
            this.UpdateState();
        }
    }
    void UpdateState(){

        Vector2 movevec = Vector2.zero;//{0,0}
        //エンターキーが入力された場合「true」
        bool inputkeyflag = false;
        if (Input.GetKey(KeyCode.Up)){
            movevec += Vector2.up;
            inputkeyflag = true;
        }
        if (Input.GetKey(KeyCode.Down)){
            movevec += Vector2.down;
            inputkeyflag = true;
        }
        if (Input.GetKey(KeyCode.Left)){
            movevec += Vector2.left;
            inputkeyflag = true;
        }
        if (Input.GetKey(KeyCode.Right)){
            movevec += Vector2.right;
            inputkeyflag = true;
        }
        bool movable = ( this.MapData.isFloor(movevec.x,movevec.y));
        if (inputkey_cooltime & movable){
            //入力があったので、ターン処理を始める
            inputinputkey_cooltime = 30;
            movetargetloc = movevec + this.player.Location;
            this.turn = Turn.Player;
            if (this.MapData.ismovable(movetargetloc)){
                this.move(movetargetloc);
            }else{
                Debug.Log("cannot move to",movetargetloc);
            }
            this.turn = Turn.Enemy;


        }

    }
    void move(Vector2 targetloc){
        this.player.move(targetloc);
    }
    void Display(){

    }
}
enum Turn
{
    Player,
    Turn,

}
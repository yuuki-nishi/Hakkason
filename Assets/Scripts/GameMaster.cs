using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
//using Turn.Turn;
public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    public MapClass MapData;
    public Turn turn = Turn.Player;//外部読み込み用
    public Player player;
    public List<Enemy> enemies;
    public int enemynum = 3;
    public int inputkey_cooltime = 30;
    public GameObject Maplayer;
    [SerializeField] private Camera maincamera;

    void Start()
    {
        this.MapData = new MapClass();
        this.player = new Player();
        this.enemies = new List<Enemy>();
        for (int i = 0;i < this.enemynum;i++){
            this.enemies.Add(new Enemy());
        }
        this.TileMapchips();
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
        if (Input.GetKey(KeyCode.UpArrow)){
            movevec += Vector2.up;
            inputkeyflag = true;
        }
        if (Input.GetKey(KeyCode.DownArrow)){
            movevec += Vector2.down;
            inputkeyflag = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            movevec += Vector2.left;
            inputkeyflag = true;
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            movevec += Vector2.right;
            inputkeyflag = true;
        }
        Vector2 movetargetloc = movevec + this.player.Location;
        //敵の方向に入力したかを見る
        Enemy attackedenemy = null;
        for (int i = 0;i<this.enemynum;i++){
            if (this.enemies[i].Location == movetargetloc){
                attackedenemy = this.enemies[i];
                break;
            }
        }
        //移動先が壁化を見る
        bool movable = ( this.MapData.isMovalbe((int)movevec.x,(int)movevec.y));
        bool turnprocessflag = false;
        if (inputkeyflag & movable){
            //入力があって動けるので、ターン処理を始める
            this.move(movetargetloc);
            turnprocessflag = true;
            this.turn = Turn.Player;
        }else if (inputkeyflag & (attackedenemy != null)){
            turnprocessflag = true;
            //敵に攻撃
            Context context=attackedenemy.attacked(this.player);
            if (context == Context.None){
                //体力消えたとかで消える
                this.enemies.Remove(attackedenemy);
            }
        }else if (inputkeyflag){
            Debug.Log("cannot move to {0}",movetargetloc);

        }

        //turnprocessflagが立つと、敵が行動できる

        if (turnprocessflag){
            inputinputkey_cooltime = 30;
            this.turn=Turn.Enemy;
            foreach (Enemy enemy in this.enemies){
                System.Console.Write("attack ememy {0} ", enemy);
                enemy.attacktoplayer(this.player);
            }
        }

    }
    void move(Vector2 targetloc){
        this.player.move(targetloc);
    }
    void TileMapchips(){
        //タイルを並べる
        this.Maplayer.clear();
        for (int x = - this.MapData.Xsize/2;x <= this.MapData.Xsize/2; x++){
            for (int y = - this.MapData.Ysize/2;y<= this.MapData.Ysize/2; y++){
                Maptile maptile = this.MapData.MapData[y][x];
                this.Maplayer.addchip(x,y,maptile);
            }
        }
    }
    void Display(){
        //カメラを動かす
        this.maincamera.transform.position.x = this.player.Location.x;
        this.maincamera.transform.position.y = this.player.Location.y;
        //キャラの描写のし直し
        
    }
}
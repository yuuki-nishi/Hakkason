using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
//using Turn.Turn;
public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    public static MapClass MapData;//描写用ではなく、なんかデータ格納してるやつ
    public Turn turn = Turn.Player;//外部読み込み用
    [SerializeField] public GameObject player;
    public static Player playerstate;
    //public List<Enemy> enemies;
    public int enemynum = 3;
    private int inputkey_cooltime = 30;
    public Maplayer maplayer=null;
    [SerializeField] public GameObject maplayerobject;//描写用
    [SerializeField] public GameObject enemy;//描写用
    [SerializeField] public GameObject senbei;//描写用
    [SerializeField] private Camera maincamera;//用
    [SerializeField] public Sprite WallSprite;
    [SerializeField] public Sprite FloorSprite;
    [SerializeField] public Sprite LadderSprite;

    void Start()
    {
        MapData = new MapClass();
        // //this.player = new Player();
        // this.enemies = new List<Enemy>();
        // for (int i = 0;i < this.enemynum;i++){
        //     this.enemies.Add(new Enemy());
        // }
        //Debug.Log(this.enemies);
        this.maplayer = new Maplayer(this.maplayerobject,this.WallSprite,this.FloorSprite,this.LadderSprite);
        this.TileMapchips();
        //プレイヤーの初期地点
        Vector2 startvec = new Vector2(MapData.startx,MapData.starty);
        this.player.GetComponent<Player>().Location = startvec;

    }

    // Update is called once per frame
    void Update()
    {
        if (this.inputkey_cooltime > 0){
            this.inputkey_cooltime -= 1;

        }else{
            this.UpdateState();
        }
        playerstate = this.player.GetComponent<Player>();
        this.PlayerDisplay();
        this.EnemyDisplay();
        this.SenbeiDisplay();
        this.Display();
    }
    void UpdateState(){

        Vector2 movevec = Vector2.zero;//{0,0}
        //エンターキーが入力された場合「true」
        bool inputkeyflag = false;
        Enums.Direction playerdir = Enums.Direction.Null;
        if (Input.GetKey(KeyCode.UpArrow)){
            movevec += Vector2.up;
            inputkeyflag = true;
            playerdir = Enums.Direction.Up;
        }
        if (Input.GetKey(KeyCode.DownArrow)){
            movevec += Vector2.down;
            inputkeyflag = true;
            playerdir = Enums.Direction.Down;
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            movevec += Vector2.left;
            inputkeyflag = true;
            playerdir = Enums.Direction.Left;
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            movevec += Vector2.right;
            inputkeyflag = true;
            playerdir = Enums.Direction.Right;
        }
        //向きを変えるだけならばタダ
        if (playerdir != Enums.Direction.Null){
            this.player.GetComponent<Player>().direction = playerdir;

        }else{
            //向きは変わらない
            playerdir = this.player.GetComponent<Player>().direction;
        }
        //
        Vector2 ploc = this.player.GetComponent<Player>().Location;//画面ではなく、抽象的な升目の方
        Vector2 movetargetloc = movevec + ploc;
        //敵の方向に入力したかを見る
        GameObject attackedenemy = null;
        for (int i = 0;i<MapData.enemies.Count;i++){
            if (MapData.enemies[i].GetComponent<Enemy>().Location == movetargetloc){
                attackedenemy = MapData.enemies[i];
                break;
            }
        }
        //移動先が壁化を見る
        //まず画面外の場合
        bool b1 = (int)movetargetloc.x < 0;
        bool b2 = (int)movetargetloc.y < 0;
        bool b3 = (int)movetargetloc.x >= MapData.Xsize;
        bool b4 = (int)movetargetloc.y >= MapData.Ysize;
        bool movable = !(b1|b2|b3|b4);
        //次に、壁の場合
        if (movable){
            movable = movable & MapData.isMovable((int)movetargetloc.x,(int)movetargetloc.y);
        }
        bool turnprocessflag = false;
        if (inputkeyflag & (attackedenemy == null) & movable){
            //入力があって動けるので、ターン処理を始める
            this.move(movetargetloc);
            turnprocessflag = true;
            this.turn = Turn.Player;
        }else if (inputkeyflag & (attackedenemy != null)){
            turnprocessflag = true;
            //敵に攻撃
            Context context=attackedenemy.GetComponent<Enemy>().attacked(this.player.GetComponent<Player>());
            if (context == Context.None){
                //体力消えたとかで消える
                MapData.enemies.Remove(attackedenemy);
            }
        }else if (inputkeyflag){
            Debug.Log("cannot move to "+movetargetloc.x+" "+movetargetloc.y);

        }

        //turnprocessflagが立つと、敵が行動できる

        if (turnprocessflag){
            this.inputkey_cooltime = 30;
            this.turn=Enums.Turn.Enemy;
            foreach (GameObject e in MapData.enemies){
                Enemy enemy = e.GetComponent<Enemy>();
                System.Console.Write("attack ememy {0} ", enemy);
                enemy.attacktoplayer(this.player.GetComponent<Player>());
            }
        }

    }
    void move(Vector2 targetloc){
        //playerの描写処理
        this.player.GetComponent<Player>().move(targetloc);
        float z = this.player.transform.position.z;
        Vector3 forposition_vec = new Vector3(targetloc.x,targetloc.y,z);
        this.player.transform.position = forposition_vec;
        Debug.Log(this.player.GetComponent<SpriteRenderer>().sprite);
        Debug.Log("player move to "+targetloc.x+" "+targetloc.y);
    }
    void TileMapchips(){
        //タイルを並べる
        Debug.Assert(this.maplayer != null);
        this.maplayer.hello();
        this.maplayer.clear();
        for (int x = 0;x < MapData.Xsize; x++){
            for (int y = 0;y< MapData.Ysize; y++){
                //Debug.Log(y);
                Enums.Maptile maptile = MapData.Get(x,y);
                float x_in_screen = x-MapData.Xsize/2;
                float y_in_screen = y-MapData.Ysize/2;
                this.maplayer.addchip(x,y,maptile);
            }
        }
    }
    void EnemyDisplay(){
        
        //enemyの描写処理
        foreach(GameObject e in MapData.enemies){
            //プレイヤーをアニメーションさせる
            SpriteRenderer renderer = e.GetComponent<SpriteRenderer>();
            Enums.Direction dir = e.GetComponent<Enemy>().direction;
            //アニメーションゲット
            Sprite changesprite = this.enemy.GetComponent<Enemy>().GetSpriteFromDirAndTime(dir,Time.frameCount);
            renderer.sprite = changesprite;

        }
    }
    
    void PlayerDisplay(){
        
        Enums.Direction playerdir = this.player.GetComponent<Player>().direction;
        //プレイヤーをアニメーションさせる
        SpriteRenderer renderer = this.player.GetComponent<SpriteRenderer>();
        //アニメーションゲット
        Sprite changesprite = this.player.GetComponent<Player>().GetSpriteFromDirAndTime(playerdir,Time.frameCount);
        renderer.sprite = changesprite;
    }

    void SenbeiDisplay(){
        Vector3 scale = new Vector3(2.0f,2.0f,1.0f);
        
        //enemyの描写処理
        foreach(GameObject e in MapData.senbeis){
            //プレイヤーをアニメーションさせる
            SpriteRenderer renderer = e.GetComponent<SpriteRenderer>();
            Enums.Direction dir = e.GetComponent<Senbei>().direction;
            //アニメーションゲット
            Sprite changesprite = this.senbei.GetComponent<Senbei>().GetSpriteFromDirAndTime(dir,Time.frameCount);
            renderer.sprite = changesprite;
            e.transform.localScale = scale;

        }
    }
    void Display(){
        //カメラを動かす
        float z = this.maincamera.transform.position.z;
        float scale = 1.0f;
        //Debug.Log(this.player.GetComponent<Player>().Location);
        Vector3 settarget = new Vector3(this.player.GetComponent<Player>().Location.x*scale, this.player.GetComponent<Player>().Location.y*scale, z);
        this.maincamera.transform.position = settarget;
        //キャラの描写のし直し
        
    }
}
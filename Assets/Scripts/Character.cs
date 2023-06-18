using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
namespace Character{
    public abstract class Character : MonoBehaviour
    {
        //public float HP = 100;
        //public float MP = 100;
        public Vector2 Location;
        protected readonly Vector2 initloc = new Vector2(0,0);
        [SerializeField] List<Sprite> AnimationSprites;
        public Enums.Direction direction = Enums.Direction.Down;//向いている方向


        public Character(){
            this.Location = new Vector2(this.initloc.x,this.initloc.y);
        }
        public void move(Vector2 targetloc){
            this.Location = targetloc;
        }
        public Sprite GetSpriteFromDirAndTime(Enums.Direction dir,int time){
            int animenum = this.AnimationSprites.Count/4;
            int frameinterval = 30;//何フレームに一回切り替えるか
            int targetanimenum = (time/frameinterval) % animenum;
            int dirnum = this.enumtoint(dir);
            int targetnumber = dirnum*animenum + targetanimenum;

            return this.AnimationSprites[targetnumber];

        }
        private int enumtoint(Enums.Direction dir){
            int ret;
            if (dir == Enums.Direction.Up){
                ret = 0;
            }else if (dir == Enums.Direction.Down){
                ret = 1;
            }else if (dir == Enums.Direction.Right){
                ret = 2;
            }else if (dir == Enums.Direction.Left){
                ret = 3;
            }else{
                ret = -100;
            }
            return ret;
        }
    }
}

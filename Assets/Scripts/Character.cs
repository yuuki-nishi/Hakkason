using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character{
    public abstract class Character
    {
        public float HP = 100;
        public float MP = 100;
        public Vector2 Location;
        protected readonly Vector2 initloc = new Vector2(0,0);


        public Character(){
            this.Location = new Vector2(this.initloc.x,this.initloc.y);
        }
        public void move(Vector2 targetloc){
            this.Location = targetloc;
        }
    }
}

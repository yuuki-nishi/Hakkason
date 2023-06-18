using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Enums;
public class Maplayer
{
    GameObject powergaugeImage;
    List<GameObject> images;

    GameObject canvas;
    private Sprite WallSprite;
    private Sprite FloorSprite;
    private Sprite LadderSprite;

    public Maplayer (GameObject canvasobject,Sprite WallSprite,Sprite FloorSprite,Sprite LadderSprite){
        this.canvas = canvasobject;
        Debug.Log("maplayer consructed");
        this.images = new List<GameObject>();
        this.WallSprite=WallSprite;
        this.FloorSprite=FloorSprite;
        this.LadderSprite=LadderSprite;
    }
    void Start()
    {

    }
    
    void Update()
    {

    }

    public void clear(){
        if (this.images.Count>0){
            this.images.Clear();

        }
    }
    public void hello(){
        Debug.Log("hello");
    }
    public void addchip(float x,float y, Enums.Maptile tile){
        int imagenum = this.images.Count;
        string name = "MapImageobject_"+(imagenum+1).ToString();
        GameObject chipImage = new GameObject(name);
        chipImage.transform.SetParent(this.canvas.transform, false);
        RectTransform rectTransform = chipImage.AddComponent<RectTransform>();
        float scale = 1.0f;
        rectTransform.localPosition = new Vector2 (x*scale,y*scale);
        float scale2 = 0.01f;
        rectTransform.localScale = new Vector3 (scale2,scale2,scale2);
        images.Add(chipImage);
        // スプライト変更
        Image img  = chipImage.AddComponent<Image>();
        if (tile == Enums.Maptile.Floor){
            img.sprite = this.FloorSprite;
        }else if (tile == Enums.Maptile.Wall){
            img.sprite = this.WallSprite;
        }else if (tile == Enums.Maptile.Ladder){
            img.sprite = this.LadderSprite;
        }else{
            Debug.Log("error tile is" + tile.ToString());
            Application.Quit();
        }
        
        ;
    }

}
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

    public Maplayer (GameObject canvasobject,Sprite WallSprite,Sprite FloorSprite){
        this.canvas = canvasobject;
        Debug.Log("maplayer consructed");
        this.images = new List<GameObject>();
        this.WallSprite=WallSprite;
        this.FloorSprite=FloorSprite;
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
        GameObject chipImage   = new GameObject(name);
        chipImage.transform.SetParent(this.canvas.transform, false);
        RectTransform rectTransform = chipImage.AddComponent<RectTransform>();
        rectTransform.localPosition = new Vector2 (x,y);
        images.Add(chipImage);
        // スプライト変更
        Image img  = chipImage.AddComponent<Image>();
        img.sprite = powergaugeImage.GetComponent<Image>().sprite;
        
        ;
    }

}
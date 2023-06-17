using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class Maplayer
{
    GameObject powergaugeImage;
    List<GameObject> images;

    [SerializeField] GameObject canvas;

    Maplayer(){
        this.powergaugeImage  = GameObject.Find("Canvas/Maplayer");
    }

    public void clean(){
        this.images.Clear();
    }
    public void addchip(int x,int y, Enums.Maptile tile){
        GameObject chipImage   = new GameObject("backgaugeImage");
        chipImage.transform.SetParent(this.canvas.transform, false);
        RectTransform rectTransform = chipImage.AddComponent<RectTransform>();
        rectTransform.localPosition = new Vector2 (x,y);
        images.Add(chipImage);
        
        ;
    }

}
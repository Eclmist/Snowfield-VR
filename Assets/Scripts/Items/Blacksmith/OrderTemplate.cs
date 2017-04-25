using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrderTemplate {

    [SerializeField]
    private Sprite sprite;
    [HideInInspector]public string spritePath;
    [SerializeField]
    private int spriteIndex;
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private int baseGold;
    [SerializeField]
    private int levelUnlocked;

    public Sprite Sprite
    {
        get { return this.sprite; }
        set { this.sprite = value; }
    }

    public string SpritePath
    {
        get { return this.spritePath; }
        set { this.spritePath = value; }
    }
    public int SpriteIndex
    {
        get { return this.spriteIndex; }
        set { this.spriteIndex = value; }
    }
    public Mesh Mesh
    {
        get { return this.mesh; }
        set { this.mesh = value; }


    }

    public int BaseGold
    {
        get { return this.baseGold; }
        set { this.baseGold = value; }
    }

    public int LevelUnlocked
    {
        get { return this.levelUnlocked; }
        set { this.levelUnlocked = value; }
    }






}

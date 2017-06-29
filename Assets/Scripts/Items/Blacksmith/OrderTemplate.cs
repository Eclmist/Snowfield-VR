using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SUFFIX
{
    sword,
    potion
}

[System.Serializable]
public class OrderTemplate {

    [SerializeField]
    private Sprite sprite;
    [HideInInspector]private string spritePath;
    [SerializeField]
    private int spriteIndex;
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private int baseGold;
    [SerializeField]
    private int duration;
    [SerializeField]
    private int levelUnlocked;
    [SerializeField]
    private JobType jobType;
    [SerializeField]
    private SUFFIX productSuffix;

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

    public int Duration
    {
        get { return this.duration; }
        set { this.duration = value; }
    }

    public int LevelUnlocked
    {
        get { return this.levelUnlocked; }
        set { this.levelUnlocked = value; }
    }


    public JobType JobType
    {
        get { return this.jobType; }
        set { this.jobType = value; }
    }

    public SUFFIX ProductSuffix
    {
        get { return this.productSuffix; }
        set { this.productSuffix = value; }
    }
    





}

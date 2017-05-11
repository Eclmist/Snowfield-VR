using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order {

    private string name;
    private Sprite sprite;
    private int duration;//i feel like having an actual time here would be better 
    private int goldReward;
    
    public Order(string _name, Sprite _sprite, int _duration, int _goldReward)
    {
        name = _name;
        sprite = _sprite;
        duration = _duration;
        goldReward = _goldReward;
    }

    public string Name
    {
        get{ return this.name; }
    }

    public Sprite Sprite
    {
        get { return this.sprite; }
    }

    public int Duration
    {
        get { return this.duration; }
    }

    public int GoldReward
    {
        get { return this.goldReward; }
    }


}

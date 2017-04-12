using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Request {

    private string name;
    private Image image;
    private int duration;//i feel like having an actual time here would be better 
    private int goldReward;
    
    public Request(string _name, Image _image, int _duration, int _goldReward)
    {
        name = _name;
        image = _image;
        duration = _duration;
        goldReward = _goldReward;
    }
}

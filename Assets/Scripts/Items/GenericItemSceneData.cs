using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericItemSceneData {

    protected int dataID;
    protected float posX, posY, posZ;
    protected float rotX, rotY, rotZ;

    public int ID
    {
        get
        {
            return dataID;
        }
    }
    public Vector3 Position
    {
        get
        {
            return new Vector3(posX, posY, posZ);
        }
    }

    public Quaternion Rotation
    {
        get
        {
            return Quaternion.Euler(rotX, rotY, rotZ);
        }
    }
	public GenericItemSceneData(int id, Vector3 position, Quaternion rotation)
    {
        dataID = id;
        posX = position.x;
        posY = position.y;
        posZ = position.z;
        rotX = rotation.x;
        rotY = rotation.y;
        rotZ = rotation.z;
    }
}

[System.Serializable]
public class IngotSceneData : GenericItemSceneData
{
    protected float temperature;

    public float Temperature
    {
        get
        {
            return temperature;
        }
    }
    public IngotSceneData(int id, Vector3 position, Quaternion rotation, float _temperature) : base(id, position, rotation)
    {
        temperature = _temperature;
        
    }
}



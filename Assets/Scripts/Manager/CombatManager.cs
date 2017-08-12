using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    [SerializeField]
    [Range(0, 100)]
    private float critChance;
    [SerializeField]
    private List<AudioClip> hitSounds;

	[SerializeField] private float hitSoundVol = 0.2F;

    public static CombatManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    public void PlayRandomHitSoundAt(Transform t)
    {
        if(hitSounds.Count > 0)
            AudioSource.PlayClipAtPoint(hitSounds[Random.Range(0,hitSounds.Count)],t.position, hitSoundVol);
    }

    public float GetDamageDealt(float damage,Transform target)
    {

        if(damage == 0)
        {
            TextSpawnerManager.Instance.SpawnText("Miss!", Color.red, target, 2);
            return 0;
        }

        bool isCrit = false;
        float rand = Random.Range(1,101);
        

        if (rand <= critChance)
        {
            damage *= 3;
            isCrit = true;
        }

        
        if(isCrit)
            TextSpawnerManager.Instance.SpawnText(Mathf.Round(damage).ToString(), Color.white, target, 6,true);
        else
            TextSpawnerManager.Instance.SpawnText(Mathf.Round(damage).ToString(), Color.white, target, 2);


        return damage;

    }
}

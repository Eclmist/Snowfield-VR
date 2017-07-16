using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class WaveGroup : IComparable
{
    public Monster[] monsters;
    public int cost;
    public int CompareTo(object obj)
    {
        return cost < ((WaveGroup)obj).cost ? 1 : -1;
    }
}
public class WaveManager : MonoBehaviour
{

    public static WaveManager Instance;

    [SerializeField]
    protected List<WaveGroup> groups = new List<WaveGroup>();
    [SerializeField]
    protected int bossWaveNumbers = 10;
    [SerializeField]
    protected WaveGroup bossGroup;
    [SerializeField]
    private int maxCost = 30;
    [SerializeField]
    private float timeBetweenEachMonsterSpawn = .5f, timeBetweenEachGroupSpawn = 2f;
    protected void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("There should only be one instance of WaveManager running");
            Destroy(this);
        }
    }

    protected void Start()
    {
        groups.Sort();
    }

    public void SpawnWave(int waveNumber)//waveNumber = cost
    {
        int cost = waveNumber;
        int level = 1;
        if (cost > maxCost)
        {
            level += cost / maxCost;
            cost /= level;
        }

        List<WaveGroup> spawnGroups = new List<WaveGroup>();
        int maximumIndex = -1;
        foreach (WaveGroup group in groups)
        {
            if (group.cost <= waveNumber)
                maximumIndex++;
        }

        while (cost > 0)
        {
            int randomGroupNumber = UnityEngine.Random.Range(0, maximumIndex);
            spawnGroups.Add(groups[randomGroupNumber]);
            cost -= groups[randomGroupNumber].cost;
        }
        if (waveNumber % bossWaveNumbers == 0)
            spawnGroups.Add(bossGroup);
        StartCoroutine(StartSpawn(spawnGroups, level));
    }

    public IEnumerator StartSpawn(List<WaveGroup> groups, int level)
    {
        for (int i = 0; i < groups.Count; i++)
        {

            for (int j = 0; j < groups[i].monsters.Length; j++)
            {
                Monster monster = Instantiate(groups[i].monsters[j], TownManager.Instance.CurrentTown.MonsterPoint.Position, Quaternion.identity).GetComponent<Monster>();
                (monster.Data as CombatAIData).CurrentJob.SetLevel(level);
                yield return new WaitForSecondsRealtime(timeBetweenEachMonsterSpawn);
            }

            yield return new WaitForSecondsRealtime(timeBetweenEachGroupSpawn);
        }

    }

}
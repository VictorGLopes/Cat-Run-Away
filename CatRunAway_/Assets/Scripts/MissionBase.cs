using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MissionType
{
    SingleRun,
    TotalMeters,
    CoinsSingleRun,
}

public abstract class MissionBase : MonoBehaviour
{
    public int max;
    public int progress;
    public int reward;
    public Player player;
    public int currentProgress;
    public MissionType missionType;

    public abstract void Created();
    public abstract string GetMissionDescription();
    public abstract void RunStart();
    public abstract void Update();

    public bool GetMissionComplete()
    {
        if((progress + currentProgress) >= max)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class SingleRun : MissionBase
{
    public override void Created()
    {
        missionType = MissionType.SingleRun;
        int[] maxValues = { 1000, 2000, 3000, 4000 };
        int randomMaxValue = Random.Range(0, maxValues.Length);
        int[] rewards = { 100, 200, 300, 400 };
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        progress = 0;
    }

    public override string GetMissionDescription()
    {
        return "run " + max + "m on total"; 
    }

    public override void RunStart()
    {
        progress = 0;
        player = FindObjectOfType<Player>();
    }

    public override void Update()
    {
       if(player == null)
        {
            return;
        }
        progress = (int)player.score;
    }
}

public class TotalMeters : MissionBase
{
    public override void Created()
    {
        missionType = MissionType.TotalMeters;
        int[] maxValues = { 10000, 20000, 30000, 40000 };
        int randomMaxValue = Random.Range(0, maxValues.Length);
        int[] rewards = { 1000, 2000, 3000, 4000 };
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        progress = 0;
    }
    //Classe de descriçao das Missoes
    public override string GetMissionDescription()
    {
        return "run " + max + "m on total";
    }

    public override void RunStart()
    {
        progress += currentProgress;
        player = FindObjectOfType<Player>();
    }

    public override void Update()
    {
       if(player == null)
        {
            return;
        }
        currentProgress = (int)player.score;
    }
}
//A quantidade de moedas e o valor necessario para fazer a missao, para coleta-las
public class CoinsSingleRun : MissionBase
{
    public override void Created()
    {
        missionType = MissionType.CoinsSingleRun;
        int[] maxValues = { 100, 200, 300, 400, 500 };
        int randomMaxValue = Random.Range(0, maxValues.Length);
        int[] rewards = { 100, 200, 300, 400, 500 };
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        progress = 0;
    }
    //Descriçao da missao
    public override string GetMissionDescription()
    {
        return "pick up " + max + " fishes in one run";
    }

    public override void RunStart()
    {
        progress = 0;
        player = FindObjectOfType<Player>();
    }

    public override void Update()
    {
        if(player == null)
        {
            return;
        }
        progress = player.coins;
    }
}


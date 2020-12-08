using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;
using System.Linq.Expressions;

//Aqui crio os dados do Player
[Serializable]
public class PlayerData
{
    public int coins;
    public int[] max;
    public int[] progress;
    public int[] currentProgress;
    public int[] reward;
    public string[] missionType;
    public int[] characterCost;
}

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int coins;
    public int[] characterCost;

    public int characterIndex;
    public int selectIndex;

    private MissionBase[] missions;
    private string filePath;

    //Aqui no Awake é onde estou fazendo a origem das missões do jogo
    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
        else if(gm != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/playerInfo.dat";

        missions = new MissionBase[2];

        if(File.Exists(filePath))
        {
            Load();
        }
        else
        {
            for (int i = 0; i < missions.Length; i++)
            {
                GameObject newMission = new GameObject("mission" + i);
                newMission.transform.SetParent(transform);
                MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeters, MissionType.CoinsSingleRun };
                int randomType = Random.Range(0, missionType.Length);
                if (randomType == (int)MissionType.SingleRun)
                {
                    missions[i] = newMission.AddComponent<SingleRun>();
                }
                else if (randomType == (int)MissionType.TotalMeters)
                {
                    missions[i] = newMission.AddComponent<TotalMeters>();
                }
                else if (randomType == (int)MissionType.CoinsSingleRun)
                {
                    missions[i] = newMission.AddComponent<CoinsSingleRun>();
                }

                missions[i].Created();
            }
        }

    }
    //Aqui são onde ficaram salvo os dados, com exceção do Highscore
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerData data = new PlayerData();

        data.coins = coins;

        data.max = new int[2];
        data.progress = new int[2];
        data.currentProgress = new int[2];
        data.reward = new int[2];
        data.missionType = new string[2];
        data.characterCost = new int[characterCost.Length];

        for (int i = 0; i < 2; i++)
        {
            data.max[i] = missions[i].max;
            data.progress[i] = missions[i].progress;
            data.currentProgress[i] = missions[i].currentProgress;
            data.reward[i] = missions[i].reward;
            data.missionType[i] = missions[i].missionType.ToString();
        }

        for (int i = 0; i < characterCost.Length; i++)
        {
            data.characterCost[i] = characterCost[i];
        }

        bf.Serialize(file, data);
        file.Close();
    }

    //Aqui no Load, é onde serao carregados os dados que salvos, com exceção do Highscore
    void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);

        PlayerData data = (PlayerData)bf.Deserialize(file);
        file.Close();

        coins = data.coins;

        for (int i = 0; i < 2; i++)
        {
            GameObject newMission = new GameObject("mission" + i);
            newMission.transform.SetParent(transform);
            if(data.missionType[i] == MissionType.SingleRun.ToString())
            {
                missions[i] = newMission.AddComponent<SingleRun>();
                missions[i].missionType = MissionType.SingleRun; 
            }
            else if(data.missionType[i] == MissionType.TotalMeters.ToString())
            {
                missions[i] = newMission.AddComponent<TotalMeters>();
                missions[i].missionType = MissionType.TotalMeters;
            }
            else if (data.missionType[i] == MissionType.CoinsSingleRun.ToString())
            {
                missions[i] = newMission.AddComponent<CoinsSingleRun>();
                missions[i].missionType = MissionType.CoinsSingleRun;
            }

            missions[i].max = data.max[i];
            missions[i].progress = data.progress[i];
            missions[i].currentProgress = data.currentProgress[i];
            missions[i].reward = data.reward[i];
        }

        for (int i = 0; i < data.characterCost.Length; i++)
        {
            characterCost[i] = data.characterCost[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Classe na qual pega o index(numero em que estao colocados) dos personagens
    public void BuyCharacter(int charIndex)
    {
        characterIndex = charIndex;
    }
    //Funçao chamada ao clicar no botão Run, para que o jogo começe saindo do Menu
    public void StartRun()
    {
        SceneManager.LoadScene("EndlessRun");
    }
    //Ao acabar o Jogo, é setado essa classe para que o jogador retorne ao Menu.
    public void EndRun()
    {
        SceneManager.LoadScene("Menu");
    }

    public MissionBase GetMission(int index)
    {
        return missions[index];
    }
    public void StartMissions()
    {
        for (int i = 0; i < 2; i++)
        {
            missions[i].RunStart();
        }
    }

    //Essa é a classe onde serao geradas as Missoes
    public void GenerateMission(int i)
    {
        Destroy(missions[i].gameObject);
        GameObject newMission = new GameObject("mission" + i);
        newMission.transform.SetParent(transform);
        MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeters, MissionType.CoinsSingleRun };
        int randomType = Random.Range(0, missionType.Length);
        if (randomType == (int)MissionType.SingleRun)
        {
            missions[i] = newMission.AddComponent<SingleRun>();
        }
        else if (randomType == (int)MissionType.TotalMeters)
        {
            missions[i] = newMission.AddComponent<TotalMeters>();
        }
        else if (randomType == (int)MissionType.CoinsSingleRun)
        {
            missions[i] = newMission.AddComponent<CoinsSingleRun>();
        }

        missions[i].Created();

        FindObjectOfType<Menu>().SetMission();
    }

}

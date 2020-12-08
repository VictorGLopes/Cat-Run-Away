using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI[] missionDescription, missionReward, missionProgress;
    public GameObject[] rewardButton;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI costText;
    public GameObject[] characters;
    public GameObject[] showMissions;
    public GameObject[] activeImage;

    public TextMeshProUGUI highscore;

    private int characterIndex = 0;

    //Ao começar o jogo, seta as missoes, atualiza as moedas pertencentes, seu score maximo.
    void Start()
    {
        SetMission();
        UpdateCoins(GameManager.gm.coins);
        showMissions[0].SetActive(false);
        showMissions[1].SetActive(false);
        highscore.text = "Highscore : " + (int)PlayerPrefs.GetFloat("Highscore") + "m";
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Na Tela Menu atualiza os peixes/coins
    public void UpdateCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }
    //Checa os coins do player, se ele possuir o suficiente para comprar outro personagem, 
    // habilita a compra, do contrario, nao habilita a compra
    public void BuyCharacter()
    {
        if (GameManager.gm.characterCost[characterIndex] <= GameManager.gm.coins)
        {
            GameManager.gm.coins -= GameManager.gm.characterCost[characterIndex];
            GameManager.gm.coins -= GameManager.gm.characterCost[characterIndex] = 0;
            GameManager.gm.BuyCharacter(characterIndex);
            GameManager.gm.Save();
            ActiveImage();
        }
    }
    //Ativa as imagens que estçao correlacionadas entre ter ou nao peixes/moedas o suficiente para comprar personagens novos.
    public void ActiveImage()
    {
        if (GameManager.gm.characterCost[characterIndex] <= GameManager.gm.coins)
        {
          
            activeImage[0].SetActive(true);
            StartCoroutine(DisableImage());

        }
        if (GameManager.gm.characterCost[characterIndex] > GameManager.gm.coins)
        {
            
            activeImage[1].SetActive(true);
            StartCoroutine(NotEnoughtGold());
            
        }
    }
    //O jogo começa
    public void StartRun()
    {
        GameManager.gm.StartRun();
    }
    //Ao clicar no botao de missões ele ativado
    public void OpenMissions()
    {
        showMissions[0].SetActive(true);
        showMissions[1].SetActive(true);
    }
    //Aqui é onde as missoes serão colocadas em game, junto de sua recompensa e progresso
    public void SetMission()
    {
        for (int i = 0; i < 2; i++)
        {
            MissionBase mission = GameManager.gm.GetMission(i);
            missionDescription[i].text = mission.GetMissionDescription();
            missionReward[i].text = "reward: " + mission.reward;
            missionProgress[i].text = mission.progress + mission.currentProgress + " / " + mission.max;
            if (mission.GetMissionComplete())
            {
                rewardButton[i].SetActive(true);
            }
        }

        GameManager.gm.Save();
    }
    //Ativa o botao para coletar recompensas das missoes
    public void GetReward(int missionIndex)
    {
        GameManager.gm.coins += GameManager.gm.GetMission(missionIndex).reward;
        UpdateCoins(GameManager.gm.coins);
        rewardButton[missionIndex].SetActive(false);
        GameManager.gm.GenerateMission(missionIndex);
    }
    //Sao as setas do Painel relacionado aos personagens compraveis/selecionaveis
    public void ChangeCharacter(int index)
    {
        characterIndex += index;
        if (characterIndex >= characters.Length)
        {
            characterIndex = 0;
        }
        else if (characterIndex < 0)
        {
            characterIndex = characters.Length - 1;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (i == characterIndex)
            {
                characters[i].SetActive(true);
            }
            else
            {
                characters[i].SetActive(false);
            }
        }

        string cost = "Unlocked";
        if (GameManager.gm.characterCost[characterIndex] != 0)
        {
            cost = GameManager.gm.characterCost[characterIndex].ToString();
        }
        costText.text = cost;
    }
    //Tempo em que a imagem fica ativa até ser desativada
    IEnumerator DisableImage()
    {
        yield return new WaitForSeconds(1f);
        activeImage[0].SetActive(false);
    }
    //Tempo em que a imagem fica ativa até ser desativada
    IEnumerator NotEnoughtGold()
    {
        yield return new WaitForSeconds(1f);
        activeImage[1].SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class HUDController : MonoBehaviour
{
    [SerializeField] TMP_Text goldNickname;
    [SerializeField] TMP_Text goldScore;
    [SerializeField] TMP_Text silverNickname;
    [SerializeField] TMP_Text silverScore;
    [SerializeField] TMP_Text bronzeNickname;
    [SerializeField] TMP_Text bronzeScore;
    [SerializeField] TMP_Text yourScore;
    [SerializeField] TMP_Text tmp_text_VictoryPlayer;
    [SerializeField] GameObject gameEndingPainel;

    public static HUDController instance;

    private void Awake() {
        instance = this;
    }
    
    private void OnEnable() {
        UpdateScores();
    }
    public void UpdateScores(){
        List<int> scores = new List<int>(GameController.instance.GetPlayerPoints());
        int goldIndex = 0, silverIndex = 1, bronzeIndex = 2;
        if(scores[silverIndex] > scores[goldIndex]){
            int aux = silverIndex;
            silverIndex = goldIndex;
            goldIndex = aux;
        }
        if(scores[bronzeIndex] > scores[silverIndex]){
            int aux = silverIndex;
            silverIndex = bronzeIndex;
            bronzeIndex = aux;
        }
        if(scores[silverIndex] > scores[goldIndex]){
            int aux = silverIndex;
            silverIndex = goldIndex;
            goldIndex = aux;
        }

        for(int i = 3; i < scores.Count; i++){
            if(scores[i] > scores[bronzeIndex]){
                int aux = i;
                bronzeIndex = i;
                if(scores[bronzeIndex] > scores[silverIndex]){
                    aux = silverIndex;
                    silverIndex = bronzeIndex;
                    bronzeIndex = aux;
                }
                if(scores[silverIndex] > scores[goldIndex]){
                    aux = silverIndex;
                    silverIndex = goldIndex;
                    goldIndex = aux;
                }
            }
        }
        
        goldNickname.text = PhotonNetwork.CurrentRoom.GetPlayer(goldIndex+1)?.NickName;
        goldScore.text = scores[goldIndex].ToString();
        silverNickname.text = PhotonNetwork.CurrentRoom.GetPlayer(silverIndex+1)?.NickName;
        silverScore.text = scores[silverIndex].ToString();
        bronzeNickname.text = PhotonNetwork.CurrentRoom.GetPlayer(bronzeIndex+1)?.NickName;
        bronzeScore.text = scores[bronzeIndex].ToString();

        yourScore.text = scores[NetworkController.instance.ocp-1].ToString();
    }

    public void ShowEngame(int playerId){
        tmp_text_VictoryPlayer.text = PhotonNetwork.CurrentRoom.GetPlayer(playerId).NickName;
        gameEndingPainel.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loginPainel, salaPainel;

    [SerializeField] private TMP_InputField nomeJogador, nomeSala;
    [SerializeField] public GameObject[] playerPrefabs;
    public string weaponName = "";

    public int selectedPrefabID = 0;
    public int ocp;

    public static NetworkController instance;

    private void Awake() {
        
        if(instance != null){
            Destroy(instance.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    /// <summary>
    /// Identifica conexão ao servidor mestre
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined the lobby");
        //PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// Identifica que foi desconectado do servidor mestre
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected");
    }

    /// <summary>
    /// Identifica que falhou em criar uma sala
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Unable to find room");
        //PhotonNetwork.CreateRoom(null, new RoomOptions());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " has " + PhotonNetwork.CurrentRoom.PlayerCount + " player(s)");

        ocp = PhotonNetwork.CurrentRoom.PlayerCount;

        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void SetIUD(int id){
        this.selectedPrefabID = id;
    }


    /// <summary>
    /// Loga o jogador e ativa o seletor da sala
    /// </summary>
    public void Login(){

        PhotonNetwork.NickName = nomeJogador.text;
        PhotonNetwork.ConnectUsingSettings();

        loginPainel.SetActive(false);
        salaPainel.SetActive(true);
    }

    public void CreateRoom(){
        PhotonNetwork.JoinOrCreateRoom(nomeSala.text, new RoomOptions(), TypedLobby.Default);
    }

    public static void OnLoginClicked(){
        instance.Login();
    }
}

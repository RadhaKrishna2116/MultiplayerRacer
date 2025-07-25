using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI roomNameText;
    private TextMeshProUGUI playerCountText;
    public void OnClickJoinRace()
    {
        PhotonNetwork.NickName = "Player_" + Random.Range(1000, 9999); // or get from input
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "RaceRoom_" + Random.Range(1000, 9999);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.AutomaticallySyncScene = true;

        SceneManager.sceneLoaded += OnGameSceneLoaded;
        SceneManager.LoadScene("GameScene");
        
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            AssignUITextReferences();
            UpdateRoomInfoUI();

            SceneManager.sceneLoaded -= OnGameSceneLoaded;
        }
    }

    void AssignUITextReferences()
    {
        if (roomNameText == null)
        {
            var roomTextObj = GameObject.Find("RoomID");
            if (roomTextObj)
                roomNameText = roomTextObj.GetComponent<TextMeshProUGUI>();
        }

        if (playerCountText == null)
        {
            var playerTextObj = GameObject.Find("PlayerCount");
            if (playerTextObj)
                playerCountText = playerTextObj.GetComponent<TextMeshProUGUI>();
        }
    }

    void UpdateRoomInfoUI()
    {
        if (!PhotonNetwork.InRoom) return;

        if (roomNameText != null)
            roomNameText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;

        if (playerCountText != null)
        {
            int current = PhotonNetwork.CurrentRoom.PlayerCount;
            playerCountText.text = "Players: " + current;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AssignUITextReferences();
        UpdateRoomInfoUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AssignUITextReferences();
        UpdateRoomInfoUI();
    }

    private void Update()
    {
        // This ensures references are assigned even if OnGameSceneLoaded missed them
        if (SceneManager.GetActiveScene().name == "GameScene" && (roomNameText == null || playerCountText == null))
        {
            AssignUITextReferences();
            UpdateRoomInfoUI();
        }
    }
}

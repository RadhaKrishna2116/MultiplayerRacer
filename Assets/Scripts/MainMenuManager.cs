using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
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
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("GameScene");
    }
}

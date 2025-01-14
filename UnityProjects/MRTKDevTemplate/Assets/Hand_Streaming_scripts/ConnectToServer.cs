using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using Microsoft.MixedReality.Toolkit.Input;
using static Microsoft.MixedReality.GraphicsTools.MeshInstancer;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private byte maxPlayersPerRoom = 2;
    private bool _isSpawned = false;
    public GameObject leftHandPrefab;
    public GameObject rightHandPrefab;
    bool first = false;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.JoinRandomRoom();
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
    }

    public override void OnConnectedToMaster()
    {
        // we don't want to do anything if we are not attempting to join a room. 
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

        // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
        first = true;
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");
        if (!_isSpawned)
        {
            _isSpawned = true;
        }
        if (!first)
        {
            GameObject leftHandPrefabRef = CustomInstantiate(leftHandPrefab);
            GameObject rightHandPrefabRef = CustomInstantiate(rightHandPrefab);
        }

    }
   

        public GameObject CustomInstantiate(GameObject controllerModel)
        {
            if (PhotonNetwork.IsConnected)
            {
                return PhotonNetwork.Instantiate(controllerModel.name, controllerModel.transform.position, controllerModel.transform.rotation);
            }
            else
            {
                Debug.LogError("Not connected to Photon Network.");
                return null;
            }
        }
}

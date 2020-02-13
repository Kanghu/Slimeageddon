using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.NetworkSystem;

public class NetworkScript : NetworkManager {



    public Button samuraiButton;
    public Button vikingButton;
    public Button wizardButton;

    public InputField addressToConnect;
    public Button hostButton;
    public Button clientButton;

    public Canvas characterSelectionCanvas;
    public Text selectionText;

    int classIndex = 0;

	// Use this for initialization
	void Start () {

        samuraiButton.onClick.AddListener(delegate { ClassPicker(samuraiButton.name);});
        vikingButton.onClick.AddListener(delegate { ClassPicker(vikingButton.name); });
        wizardButton.onClick.AddListener(delegate { ClassPicker(wizardButton.name); });

        hostButton.onClick.AddListener(delegate { GameConnect("HOST"); });
        clientButton.onClick.AddListener(delegate { GameConnect("CLIENT"); });
    }


    void GameConnect(string mode)
    {
        switch(mode)
        {
            case "HOST":
                GetComponent<NetworkManager>().StartHost();
                break;

            case "CLIENT":
                GetComponent<NetworkManager>().networkAddress = addressToConnect.text;
                GetComponent<NetworkManager>().networkPort = 7777;

                if (GetComponent<NetworkManager>().networkAddress == string.Empty)
                    GetComponent<NetworkManager>().networkAddress = "localhost";

                Debug.Log("ip: " + GetComponent<NetworkManager>().networkAddress);
                Debug.Log("port: " + GetComponent<NetworkManager>().networkPort);

                GetComponent<NetworkManager>().StartClient();
                break;
        }
    }

    void ClassPicker(string buttonName)
    {
        switch(buttonName)
        {
            case "Viking":
                classIndex = 0;
                selectionText.text = "Viking Selected";
                break;

            case "Samurai":
                classIndex = 1;
                selectionText.text = "Samurai Selected";
                break;

            case "Wizard":
                classIndex = 2;
                selectionText.text = "Wizard Selected";
                break;
        }

        playerPrefab = spawnPrefabs[classIndex];
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        /// ***
        /// This is added:
        /// First, turn off the canvas...
        characterSelectionCanvas.enabled = false;
      //  GetComponent<NetworkManagerHUD>().enabled = false;
        /// Can't directly send an int variable to 'addPlayer()' so you have to use a message service...
        IntegerMessage msg = new IntegerMessage(classIndex);
        /// ***

        if (!clientLoadedScene)
        {
            // Ready/AddPlayer is usually triggered by a scene load completing. if no scene was loaded, then Ready/AddPlayer it here instead.
            ClientScene.Ready(conn);
            if (autoCreatePlayer)
            {
                ///***
                /// This is changed - the original calls a differnet version of addPlayer
                /// this calls a version that allows a message to be sent
                ClientScene.AddPlayer(conn, 0, msg);
            }
        }

    }

    /// Copied from Unity's original NetworkManager 'OnServerAddPlayerInternal' script except where noted
    /// Since OnServerAddPlayer calls OnServerAddPlayerInternal and needs to pass the message - just add it all into one.
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        /// *** additions
        /// I skipped all the debug messages...
        /// This is added to recieve the message from addPlayer()...
        int id = 0;

        if (extraMessageReader != null)
        {
            IntegerMessage i = extraMessageReader.ReadMessage<IntegerMessage>();
            id = i.value;
        }

        /// using the sent message - pick the correct prefab
        GameObject playerPrefab = spawnPrefabs[id];
        /// *** end of additions

        GameObject player;
        Transform startPos = GetStartPosition();
        if (startPos != null)
        {
            player = (GameObject)Instantiate(playerPrefab, startPos.position, startPos.rotation);
        }
        else
        {
            player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}

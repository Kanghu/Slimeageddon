using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameManager : NetworkBehaviour {

    public GameObject canvasChSel;

    public string playerName = string.Empty;

    public AnimationClip[] damageParticles;
    public Color[] damageColor;


    public void InitPlayer(PlayerBehaviour player)
    {






        //Disable Character Select
        if (canvasChSel.active == true)
        {
            canvasChSel.SetActive(false);
        }


        /*

        if (isServer)
        {
            RpcName(player);
        }
        else
        {
            CmdName(player);

            //Set Player's name
            if (playerName == string.Empty || player.name == string.Empty)
            {
                string name = GameObject.Find("Menus/Character Selection/InputField PlayerName").GetComponent<InputField>().text;


                if (name != string.Empty)
                {
                    player.PlayerName = name;
                    player.name = player.PlayerName;
                    playerName = player.PlayerName;
                }
                else
                {
                    name = "Player";
                    player.PlayerName = name;
                    player.name = player.PlayerName;
                    playerName = player.PlayerName;
                }

                GameObject.Find("Player UI's/Player UI/My Player Name/Text").GetComponent<Text>().text = name;

            }
            else
            {
                playerName = player.PlayerName;
            }

            //Disable Character Select
            if (canvasChSel.active == true)
            {
                canvasChSel.SetActive(false);
            }
            //

        }

        */





    }

    /*

    [ClientRpc]
    public void RpcName(PlayerBehaviour player)
    {
        PlayerBehaviour[] players = FindObjectsOfType<PlayerBehaviour>();




        if (playerName == string.Empty || player.name == string.Empty)
        {
            string name = GameObject.Find("Menus/Character Selection/InputField PlayerName").GetComponent<InputField>().text;


            if (name != string.Empty)
            {
                player.PlayerName = name;
                player.name = player.PlayerName;
                playerName = player.PlayerName;
            }
            else
            {
                name = "Player";
                player.PlayerName = name;
                player.name = player.PlayerName;
                playerName = player.PlayerName;
            }

            GameObject.Find("Player UI's/Player UI/My Player Name/Text").GetComponent<Text>().text = name;

        }
        else
        {
            playerName = player.PlayerName;
        }
    }

    

    [Command]
    public void CmdName(PlayerBehaviour player)
    {
        if (playerName == string.Empty || player.name == string.Empty)
        {
            string name = GameObject.Find("Menus/Character Selection/InputField PlayerName").GetComponent<InputField>().text;


            if (name != string.Empty)
            {
                player.PlayerName = name;
                player.name = player.PlayerName;
                playerName = player.PlayerName;
            }
            else
            {
                name = "Player";
                player.PlayerName = name;
                player.name = player.PlayerName;
                playerName = player.PlayerName;
            }

            GameObject.Find("Player UI's/Player UI/My Player Name/Text").GetComponent<Text>().text = name;

        }
        else
        {
            playerName = player.PlayerName;
        }
    }
    */

}

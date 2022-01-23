using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialTiles : MonoBehaviour
{
    public GameObject Tiles, TextDisplayer1, TextDisplayer2, Player1, Player2;
    private TextMeshProUGUI Text1, Text2;

    private Transform current;

    void Start()
    {
        Text1 = TextDisplayer1.GetComponent<TextMeshProUGUI>();
        Text2 = TextDisplayer2.GetComponent<TextMeshProUGUI>();

        for (int i = 1; i < Tiles.transform.childCount - 1; i++)
        {
            Transform current = Tiles.transform.GetChild(i);

            if (Random.Range(0, 3) > 0.5f)
            {
                int effect = Random.Range(1, 2);
                string Tag = "";

                switch (effect)
                {
                    case 0:
                        Tag = "GoBack";
                        break;
                    case 1:
                        Tag = "Boost";
                        break;
                    case 2:
                        Tag = "Teleport";
                        break;
                }

                current.tag = Tag;
            }

        }
    }


    void Update()
    {
        int Index = 0;
        GameObject Player;

        if (PlayerBehavior.Player1Turn == false)
        {
            current = Tiles.transform.GetChild(PlayerBehavior.TileIndex_Player1);
            Index = PlayerBehavior.TileIndex_Player1;
            Player = Player1;
        }
        else
        {
            current = Tiles.transform.GetChild(PlayerBehavior.TileIndex_Player2);
            Index = PlayerBehavior.TileIndex_Player2;
            Player = Player2;
        }



        if (PlayerBehavior.Finished == true)
        {
            if (current.tag == "Boost")
            {
                Debug.Log("You Got a boost");

                PlayerBehavior.Player1Turn = !PlayerBehavior.Player1Turn;
                GameObject.Find("Players").GetComponent<PlayerBehavior>().RollTheDice_EventHandler();

                StartCoroutine(ShowSpecial("Tou Got a boost of a few Tiles"));
            }
            else if (current.tag == "Teleport")
            {
                Debug.Log("You will go to a random tile");
                int Num = Mathf.Clamp(Random.Range(Index - 7, Tiles.transform.childCount - 2), 0,
                                                                             Tiles.transform.childCount);

                StartCoroutine(Show($"You Teleported {Mathf.Abs(Index - Num)} " +
                     (Index < Num ? " Forward" : " Backward")));

                Index = Num;

                Player.transform.position = Tiles.transform.GetChild(Index).position + Vector3.up * 0.4f;

                if (PlayerBehavior.Player1Turn == false)
                    PlayerBehavior.TileIndex_Player1 = Index;
                else
                    PlayerBehavior.TileIndex_Player2 = Index;

            }
            else if (current.tag == "GoBack")
            {
                Debug.Log("You Got a Pinelity");
                int num = Random.Range(1, 6);

                Index = Mathf.Clamp(Index - num, 0, Tiles.transform.childCount);

                Player.transform.position = Tiles.transform.GetChild(Index).position + Vector3.up * 0.4f;

                if (PlayerBehavior.Player1Turn == false)
                    PlayerBehavior.TileIndex_Player1 = Index;
                else
                    PlayerBehavior.TileIndex_Player2 = Index;

                StartCoroutine(Show($"you Got Back by {num} Tiles"));
            }

            PlayerBehavior.Finished = false;
        }
    }

    IEnumerator ShowSpecial(string message)
    {
        if (PlayerBehavior.Player1Turn == true)
        {
            Text1.text = message;
        }
        else
        {
            Text2.text = message;
        }

        yield return new WaitForSeconds(3f);

        Text1.text = "";

        Text2.text = "";
    }
    IEnumerator Show(string message)
    {
        if (PlayerBehavior.Player1Turn == false)
        {
            Text1.text = message;                                          
        }
        else
        {
            Text2.text = message;                 
        }     
        
        yield return new WaitForSeconds(3f);

        Text1.text = "";

        Text2.text = "";
    }
}

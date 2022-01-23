using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{    
    [Tooltip("The Parent object of all the Tiles")]
    public GameObject Tiles;
    [Tooltip("The Button to role the Dice")]
    public GameObject RollingButton;
    [Tooltip("Win screen object")]//AFB
    public GameObject WinScreen;//AFB
    [Tooltip("The text saying that the Dice is Rolling")]//AFB
    public GameObject Character;//AFB
    [Tooltip("The dice roll sound")]//AFB
    public AudioSource RollSound;//AFB
    [Tooltip("The roll 6 sound")]//AFB
    public AudioSource Roll6Sound;//AFB
    [Tooltip("The roll 1 sound")]//AFB
    public AudioSource Roll1Sound;//AFB
    [Tooltip("The text saying that the Dice is Rolling")]
    public TextMeshProUGUI WaitingText_Player1, WaitingText_Player2;
    [Tooltip("The Number of Actual Tile")]
    public TextMeshProUGUI Score1, Score2;
    [Tooltip("The Final Canvas To show in the end")]
    public GameObject EndDesplayer;
    [Tooltip("The Text that displays the winner")]
    public TextMeshProUGUI Winner;

    [Header("The Players")]
    [Space]
    public GameObject Player1;
    public GameObject Player2;

    public static bool Player1Turn = true;

    //The spees of movement of the player it can be changed through setting    
    [Range(1, 20)] public static float Speed = 5;


    private int DiceNumber;
    private bool DiceRolled;

    private int TileCount;

    public static int TileIndex_Player1 = 0;
    public static int TileIndex_Player2 = 0;

    private float Default_Y;
    private bool Positioned = false;
    private bool End = false;
    public static bool Finished = false;

    [Header("Events")]
    [Space]
    public GameObject Event;


    void Start()
    {
        Player1.transform.position = Tiles.transform.GetChild(0).position + Vector3.up * 0.4f;
        Player2.transform.position = Tiles.transform.GetChild(0).position + Vector3.up * 0.4f;

        DiceRolled = false;
        Default_Y = Player1.transform.position.y;

        TileCount = Tiles.transform.childCount;//AFB
    }

    void Update()
    {
        Score1.text = $"Score : {TileIndex_Player1} / {TileCount}";
        Score2.text = $"Score : {TileIndex_Player2} / {TileCount}";

        if (Input.GetButtonDown("Action") && RollingButton.activeSelf == true)
        {
            RollTheDice_EventHandler();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Event.GetComponent<Button>().onClick.Invoke();
        }


        Transform CurrentTile = (Player1Turn == true) ? Tiles.transform.GetChild(TileIndex_Player1) :
                                                                 Tiles.transform.GetChild(TileIndex_Player2);
        
        //keep in mind if something don't work change GameObject To Transform

        Debug.Log("the One who is gonna move now is " + ((Player1Turn == true) ? "Player1" : "Player2"));

        Transform CurrentPlayer = (Player1Turn == true) ? Player1.transform : Player2.transform;

        //if the dice is rolled i can start moving
        if (DiceRolled)
        {
            Debug.Log("i will start elevating is : " + (CurrentPlayer.position.y < Default_Y + 1.5f));

            if (CurrentPlayer.position.y < Default_Y + 1.5f && Positioned == false)
            {
                transform.position += Vector3.up * Time.deltaTime * Speed;

                if (DiceNumber == 6)
                {
                    Roll6Sound.Play();
                    //transform.Rotate(Time.deltaTime * Speed*420, 0.0f, 0.0f, Space.Self);
                }
            }
            else
            {
                string RollText = $"You rolled : {DiceNumber}";
                if (DiceNumber == 1)
                {
                    RollText = $"You rolled : {DiceNumber} \n, -1 Social Credit!";
                    Roll1Sound.Play();
                }

                if (Player1Turn)
                {
                    WaitingText_Player1.text = RollText;
                }
                else
                {
                    WaitingText_Player2.text = RollText;
                }


                //Then i move forward till i arrive at The Current Tile position

                if (Mathf.Abs(CurrentPlayer.position.z) < Mathf.Abs(CurrentTile.position.z))
                {
                    CurrentPlayer.position += Vector3.forward * Time.deltaTime * Speed;
                }
                else
                {
                    //I set positioned to true to start landing on the tile

                    Positioned = true;
                }

            }

            if (Positioned)
            {
                //Here i will start landing

                transform.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                if (CurrentPlayer.position.y > Default_Y)
                {
                    CurrentPlayer.position += Vector3.down * Time.deltaTime * Speed;
                }
                else
                {
                    Positioned = false;
                    DiceRolled = false;

                    RollingButton.SetActive(true);

                    WaitingText_Player1.enabled = false;
                    WaitingText_Player2.enabled = false;

                    Player1Turn = !Player1Turn;
                    Finished = true;

                    WaitingText_Player1.text = "The Dice is Rolling ...";
                    WaitingText_Player2.text = "The Dice is Rolling ...";
                }
            }

        }
        else
        {
            CurrentPlayer.position = CurrentTile.position + Vector3.up * 0.4f;

            if (TileIndex_Player1 == Tiles.transform.childCount - 1 ||
                                                    TileIndex_Player2 == Tiles.transform.childCount - 1)
            {
                End = true;
            }
        }

        if (End)
        {
            EndDesplayer.SetActive(true); //pakeisti, jei nepatiks
            Winner.text = (TileIndex_Player1 == Tiles.transform.childCount - 1) ? "The Player 1 Won !" : "The Player 2 Won !";
        }
    }
        
    public void RollTheDice_EventHandler()
    {
        RollSound.Play();
        DiceNumber = Random.Range(1, 7);

        if (Player1Turn)
        {
            TileIndex_Player1 += DiceNumber;
        }
        else
        {
            TileIndex_Player2 += DiceNumber;
        }


        if (TileIndex_Player1 >= Tiles.transform.childCount - 1)
        {
            TileIndex_Player1 = Tiles.transform.childCount - 1;
        }
        else if (TileIndex_Player2 >= Tiles.transform.childCount - 1)
        {
            TileIndex_Player2 = Tiles.transform.childCount - 1;
        }

        DiceRolled = true;

        RollingButton.SetActive(false);

        if (Player1Turn)
        {
            WaitingText_Player1.enabled = true;
        }
        else
        {
            WaitingText_Player2.enabled = true;
        }
    }
}

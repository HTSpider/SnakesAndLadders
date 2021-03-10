using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMovement : MonoBehaviour
{
    //text and string variables
    public Text QuestionRightList; public Text QuestionWrongList;
    public Text ansRightText; public Text ansWrongText;
    public Text rollText;
    public string answer;

    //int and float variables
    int ansUserRight; int ansUserWrong;
    int diceRoll;
    int randQ;
    int currentLadder; int currentSnake;
    float posx; float posy;

    //bools to see if you are on a ladder ot not and at the edge of screen
    bool onSnake; bool onLadder;
    bool leftToRight = true;

    //game objects that the user interacts with
    public GameObject endScreenUI;
    public GameObject playerInputUI;
    public GameObject dice;
    public GameObject player;
    public GameObject textDisplay;
    public InputField inputField;
    public Button enterButton;

    //lists to store questions and answers
    List<string> questions = new List<string>();
    List<string> answers = new List<string>();
    List<string> answersWrong = new List<string>();
    List<string> answersRight = new List<string>();

    //Array containing coordinates of each ladder
    public int[,] ladders = new int[3, 2] { { 8, 0 }, { 4, 8 }, { 6, 10 }};
    public int[,] snakes = new int[1, 2] { { 4, 4 }};


    void Start()
    {
        //setting location of square to the start 
        posx = player.transform.position.x;
        posy = player.transform.position.y;
    }

    

    // Gets a random question and displays it on the screen, disables rolling of the dice
    public void GetQ()
    {
        questions.Add("1 + 1");
        questions.Add("2 + 2");
        questions.Add("3 + 3");
        questions.Add("4 + 4");
        questions.Add("5 + 5");
        questions.Add("6 + 6");
        questions.Add("7 + 7");
        questions.Add("8 + 8");
        questions.Add("9 + 9");
        questions.Add("10 + 10");

        answers.Add("2");
        answers.Add("4");
        answers.Add("6");
        answers.Add("8");
        answers.Add("10");
        answers.Add("12");
        answers.Add("14");
        answers.Add("16");
        answers.Add("18");
        answers.Add("20");

        //UI stuff
        dice.SetActive(false);
        textDisplay.SetActive(true);
        enterButton.enabled = true;

        //getting a diffrent question when you are on a ladder or a snake
        randQ = UnityEngine.Random.Range(1, questions.Count);
        textDisplay.GetComponent<Text>().text = (questions[randQ]);
    }

    //Checks if inputted value is correct for the current question and moves depending on which ladder player is currently at
    public void CheckAns()
    {
        answer = inputField.text;
        //answer is right and on ladder
        if (onLadder == true)
        {
            if (answer.Equals(answers[randQ]))
            {
                //diffrent case deppending on which ladder
                dice.SetActive(true);
                switch (currentLadder)
                {
                    //ladder 1
                    case 0:
                        //move pos
                        posx = posx - 8;
                        posy = posy + 4;
                        move(1);
                        //no longer on ladder
                        onLadder = false;
                        //+1 to questions answered right
                        ansUserRight++;
                        break;
                    case 1:
                        posx = posx + 0;
                        posy = posy + 4;
                        move(1);
                        onLadder = false;
                        ansUserRight++;
                        break;
                    case 2:
                        posx = posx + 0;
                        posy = posy + 4;
                        move(1);
                        onLadder = false;
                        ansUserRight++;
                        break;
                }
                //add the question answered right to a list 
                answersRight.Add(questions[randQ]);
                UpdateList(1);
            }

            //on ladder and answer is wrong
            else
            {
                onLadder = false;
                answersWrong.Add(questions[randQ]);
                ansUserWrong++;
                UpdateList(2);
            }
        }

        // are u on a snake
        if (onSnake == true)
        {
            //is the answer wrong
            if (!(answer.Equals(answers[randQ])))
            {
                //diffrent case deppending on which snake
                dice.SetActive(true);
                switch (currentSnake)
                {
                    //snake 1
                    case 0:
                        //move pos
                        posx = 0;
                        posy = 0;
                        move(1);
                        //question wrong +1
                        ansUserWrong++;
                        //no longer on snake
                        onSnake = false;
                        break;
                }
                //add the question answered wrong to a list
                answersWrong.Add(questions[randQ]);
                UpdateList(2);
            }

            //on snake and answer is right
            else
            {
                onSnake = false;
                ansUserRight++;
                answersRight.Add(questions[randQ]);
                UpdateList(1);
            }
        }
        //Debug.Log(ansUserRight);
        //Debug.Log(ansUserWrong);
        //answersRight.ForEach(Debug.Log);
        //answersWrong.ForEach(Debug.Log);

        //UI stuff
        textDisplay.SetActive(false);
            enterButton.enabled = false;
            inputField.text = "";
            dice.SetActive(true);
        
    }


    // Method to roll dice and get number
    public void roll()
    {
        // Getting random value for the dice roll
        diceRoll = UnityEngine.Random.Range(1, 7);
        rollText.text = ("You Rolled a: " + diceRoll);
        move(diceRoll);
    }

    //adding the right and wrong questions at the end screen
    public void UpdateList(int RW)
    {
        if (RW == 1)
        {
            QuestionRightList.text += "\n" + questions[randQ];
        }

        if (RW == 2)
        {
            QuestionWrongList.text += "\n" + questions[randQ];
        }
    }
 
    //Moves specific number of squares
    void move(int count)
    {
        // iterating through the random value
        for (int i = 0; i < count; i++)
        {
            if (leftToRight == true)
            {
                //if player pos is for right
                if (player.transform.position.x == 18)
                {

                    //move up by 1 square
                    posy = posy + 2;
                    player.transform.position = new Vector2(posx, posy);
                    //we are now moving right to left
                    leftToRight = false;
                }

                else
                {
                    //move left to right untill x is 18
                    player.transform.position = new Vector2(posx + 2, posy + 0);
                    posx = posx + 2;
                    posy = posy + 0;

                }
            }

            else
            {
                //if player pos is far left and not at start 
                if (player.transform.position.x == 0 && player.transform.position.y != 0)
                {
                    //change it back to left to right
                    leftToRight = true;
                    //move up square
                    posy = posy + 2;
                    player.transform.position = new Vector2(posx, posy);

                }

                else
                {
                    //moving right to left
                    player.transform.position = new Vector2(posx - 2, posy + 0);
                    posx = posx - 2;
                    posy = posy + 0;

                }
            }

            
            for(int j = 0; j < ladders.GetLength(0); j++)
            {
                // checking if it is on last iteration and a ladder square 
                if (player.transform.position.x == ladders[j, 0] && player.transform.position.y == ladders[j, 1] && i == diceRoll - 1)
                {
                    GetQ();
                    currentLadder = j;
                    onLadder = true;
                }
            }

            for (int j = 0; j < snakes.GetLength(0); j++)
            {
                // checking if it is on last iteration and a ladder square 
                if (player.transform.position.x == snakes[j, 0] && player.transform.position.y == snakes[j, 1] && i == diceRoll - 1)
                {
                    GetQ();
                    currentSnake = j;
                    onSnake = true;
                }
            }



            //show the win screen make certain thing visable and invisable 
            if (player.transform.position.x == 0 && player.transform.position.y == 18)
            {
                Console.Write(ansUserRight);
                ansRightText.text = ansUserRight.ToString();
                ansWrongText.text = ansUserWrong.ToString();
                endScreenUI.SetActive(true);
                playerInputUI.SetActive(false);
                dice.SetActive(false);
                enterButton.gameObject.SetActive(false);
                inputField.gameObject.SetActive(false);
                rollText.gameObject.SetActive(false);
            }
        }
    }
}

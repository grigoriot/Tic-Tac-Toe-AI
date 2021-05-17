using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public char playerSymbol = 'O';
    public char enemySymbol = 'X';

    public bool playerTurn = true;
    
    public GameObject[] board;

    private TicTacToe currentState;

    private TicTacToe newState;

    [SerializeField] private Text endText;

    [SerializeField] private Slider slider;

    [SerializeField] private Text sliderText;

    // Start is called before the first frame update
    void Start()
    {
        currentState = new TicTacToe();

        //currentState.expand(enemySymbol);

        newState = new TicTacToe();

        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                currentState.board[i][j] = board[index];
                newState.board[i][j] = board[index];
                index++;
            }
        }

        if (endText == null)
        {
            endText = FindObjectOfType<Text>();
        }

        if (slider == null)
        {
            slider = FindObjectOfType<Slider>();
        }

        slider.enabled = true;
        
        ChangeSliderText();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControls();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void PlayerControls()
    {
        if (Input.GetMouseButtonDown(0) && playerTurn)
        {
            //Debug.Log("Player's turn");
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.Log("Raycasting...");
            
            if (Physics.Raycast(ray, out hit))
            {
                
                //Debug.Log("Success");
                if (hit.collider != null)
                {
                    // Debug.Log("Collider not null");

                    slider.enabled = false;
                    
                    currentState.setCell(hit.collider.gameObject.GetComponent<PointScript>().x,
                        hit.collider.gameObject.GetComponent<PointScript>().y, playerSymbol);
                    
                    hit.collider.enabled = false;
                    
                    CheckGameState();

                    playerTurn = false;
                    AIPlay();
                }
                else
                {
                    //Debug.Log("Collider null");
                }
            }
        }
    }

    private void AIPlay()
    {
        //Debug.Log("AI's turn");

        TicTacToe best = new TicTacToe();
        
        int exp = minimax(currentState, (int)slider.value, true, ref best);
        // Debug.Log("Current State \n" + currentState.toString());
        // Debug.Log("New State \n" + best.toString());
        // Debug.Log("Evaluate new state : " + exp);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (currentState.GetCellSymbol(i, j) != best.GetCellSymbol(i, j))
                {
                    currentState.setCell(i, j, enemySymbol);
                }
            }
        }

        CheckGameState();
        playerTurn = true;
    }
    
    private int minimax (TicTacToe s, int depth, bool isMax, ref TicTacToe best)
    {
        int k = s.evaluate();

        if (depth==0 || k!=-100)
        {
            best = s;
            return k;
        }

        int max,temp;
        TicTacToe maxState = new TicTacToe(),tempState = new TicTacToe();

        LinkedList<TicTacToe> childrenList = s.expand(isMax? enemySymbol:playerSymbol);
        TicTacToe[] children = new TicTacToe[childrenList.Count];
        childrenList.CopyTo(children, 0);

        max=minimax(children[0],depth-1,!isMax,ref maxState);
        maxState=children[0];


        for (int i=1;i<children.Length; i++)
        {
            temp=minimax(children[i],depth-1,!isMax,ref tempState);
            if ((temp>max) == isMax)
            {
                max=temp;
                maxState=children[i];
            }
        }
        best = maxState;

        return max;
    }

    private void CheckGameState()
    {
        int v = currentState.evaluate();
        switch (v)
        {
            case 0:
                ShowText("Tie");
                break;

            case 1:
                ShowText("You lose");
                break;
            
            case -1:
                ShowText("You win");
                break;
        }
    }

    private void ShowText(string text)
    {
        endText.text = text;
        endText.gameObject.SetActive(true);
        StartCoroutine(Wait(4f));
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeSliderText()
    {
        sliderText.text = "Difficulty : " + slider.value;
    }
}

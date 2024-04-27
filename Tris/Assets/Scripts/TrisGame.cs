using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrisGame : MonoBehaviour
{
    public List<GameObject> buttons;
    private string[] board;

    public TMP_Text UIturno;
    private string currentSymbol;

    private void Start()
    {
        foreach (GameObject go in buttons)
        {
            go.GetComponentInChildren<TMP_Text>().text = "";
            go.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        currentSymbol = Random.Range(0, 2) == 0 ? "O" : "X";
        UIturno.text = $"Turno: {currentSymbol}";

        board = new string[9];
    }


    public void Click(GameObject caller)
    {
        // Ottieni il nome del metodo chiamante
        string methodName = caller.name;

        if (int.TryParse(methodName, out int inputInt))
        {
            board[inputInt] = currentSymbol;
        }

        foreach (GameObject buttonObj in buttons)
        {
            if (buttonObj.name == methodName)
            {
                TMP_Text textButton = buttonObj.GetComponentInChildren<TMP_Text>();
                if (textButton.text == "")
                {

                    textButton.text = currentSymbol;
                    if (CheckWin())
                    {
                        UIturno.text = $"Ha vinto {currentSymbol}";
                        WaitSecond();
                        SceneManager.LoadScene("WinningScene");

                    }
                    else
                    {
                        currentSymbol = currentSymbol == "X" ? "O" : "X";
                        UIturno.text = $"Turno: {currentSymbol}";
                    }
                }
                break;
            }
        }

        int checkDraw = 0;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == "X" || board[i] == "O")
            {
                checkDraw++;
                if (checkDraw == 9)
                {
                    UIturno.text = $"Pareggio";
                    WaitSecond();
                    SceneManager.LoadScene("Draw");
                }
            }
            else checkDraw--;
            
            Debug.Log(checkDraw);
        }

        checkDraw = 0;
    }

    private bool CheckWin()
    {
        int[,] winningCombinations = {
            /*0*/{ 0, 1, 2 }, /*1*/{ 3, 4, 5 }, /*2*/{ 6, 7, 8 }, // conbinazioni orrizontali
            /*3*/{ 0, 3, 6 }, /*4*/{ 1, 4, 7 }, /*5*/{ 2, 5, 8 }, // conbinazioni verticali
            /*6*/{ 0, 4, 8 }, /*7*/{ 2, 4, 6 }                    // conbinazioni oblique
        };

        for (int i = 0; i < winningCombinations.GetLength(0); i++)
        {
            //          Debug.Log(board[winningCombinations[i, 0]] + " " + board[winningCombinations[i, 1]] + " " + board[winningCombinations[i, 2]]);
            if (board[winningCombinations[i, 0]] == currentSymbol
                && board[winningCombinations[i, 1]] == currentSymbol
                && board[winningCombinations[i, 2]] == currentSymbol)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerable WaitSecond()
    {
        yield return new WaitForSeconds(3);
    }
}
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TicTacToe
{
    public GameObject[][] board;
    
    private char[][] symbols;

    public TicTacToe()
    {
        board = new GameObject[3][];
        symbols = new char[3][];
        for (int i = 0; i < 3; i++)
        {
            board[i] = new GameObject[3];
            symbols[i] = new char[3];
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                symbols[i][j] = ' ';
            }
        }
    }

    public bool setCell(int i, int j, char value)
    {
        if (symbols[i][j] == ' ')
        {
            symbols[i][j] = value;
            board[i][j].GetComponent<PointScript>().ShowSymbol(value);
            return true;
        }

        return false;
    }

    public char GetCellSymbol(int i, int j)
    {
        return symbols[i][j];
    }

    public LinkedList<TicTacToe> expand(char k)
    {
        LinkedList<TicTacToe> children = new LinkedList<TicTacToe>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (symbols[i][j] == ' ')
                {
                    TicTacToe child = new TicTacToe();

                    for (int p = 0; p < 3; p++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            //Debug.Log("(" + p + "," + l + ")");
                            child.board[p][l] = this.board[p][l];
                            child.symbols[p][l] = this.symbols[p][l];
                        }
                    }

                    //Debug.Log("(" + i + "," + j + ")");
                    child.symbols[i][j] = k;
                    children.AddLast(child);
                }
            }
        }

        //if (children.Count > 0) Debug.Log("Expanding");
        
        return children;
    }
    
    public int evaluate()
    {
        bool same;
        
        for (int i=0;i<3;i++) //check for rows
        {
            same=true;
            for (int j=1;j<3;j++)
                if (symbols[i][j]==' '|| symbols[i][j]!=symbols[i][j-1])
                {
                    same=false;
                    break;
                }
            if (same)
            {
                return symbols[i][0]=='X'?1:-1;
            }
        }

        for (int j=0;j<3;j++) //check for columns
        {
            same=true;
            for (int i=1;i<3;i++)
                if (symbols[i][j]==' '|| symbols[i][j]!=symbols[i-1][j])
                {
                    same=false;
                    break;
                }
            if (same)
            {
                return symbols[0][j]=='X'?1:-1;
            }
        }
        same=true;
        for (int i=1;i<3;i++)
            if (symbols[i][i]==' '|| symbols[i][i]!=symbols[i-1][i-1])
            {
                same=false;
                break;
            }
        if (same)
            return symbols[0][0]=='X'?1:-1;

        same=true;
        for (int i=1;i<3;i++)
            if (symbols[i][2-i]==' '|| symbols[i][2-i]!=symbols[i-1][2-(i-1)])
            {
                same=false;
                break;
            }
        if (same)
            return symbols[1][1]=='X'?1:-1;


        for (int i=0;i<3;i++)
        for (int j=0;j<3;j++)
            if (symbols[i][j]==' ')
                return -100; //undefined

        return 0;
    }

    public string toString()
    {
        StringBuilder ot = new StringBuilder();
        for (int i=0;i<3;i++)
        {
            for (int j = 0; j < 3; j++)
                ot.Append(symbols[i][j]).Append(' ');
            ot.Append('\n');
        }
        return ot.ToString();
    }
}

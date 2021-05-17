using System.Collections;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    public int x;
    public int y;

    public GameObject OSymbol;
    public GameObject XSymbol;
    
    public void ShowSymbol(char value)
    {
        StartCoroutine(MyWait());
        
        if (value == 'X')
        {
           XSymbol.SetActive(true);
        }
        else if (value == 'O')
        {
            OSymbol.SetActive(true);
        }
    }

    IEnumerator MyWait()
    {
        yield return new WaitForSeconds(1f);
    }
}

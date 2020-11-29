using System.Collections;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField]private GameObject WinUI; 

    internal void playerWon(){

        StartCoroutine(WaitBeforeWin());
    }

    private IEnumerator WaitBeforeWin(){
        
        yield return new WaitForSeconds(0.5f);
        WinUI.SetActive(true);
        Time.timeScale = 0;
    }
}

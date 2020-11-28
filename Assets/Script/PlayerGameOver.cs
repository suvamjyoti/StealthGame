using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGameOver : MonoBehaviour
{
    [SerializeField]private Player player;
    [SerializeField]private GameObject GameOverUI; 

    internal void playerDetected(){
        player.enabled = false;
        StartCoroutine(WaitBeforeGameOver());
    }

    private IEnumerator WaitBeforeGameOver(){
        yield return new WaitForSeconds(0.5f);
        GameOverUI.SetActive(true);
    }


}

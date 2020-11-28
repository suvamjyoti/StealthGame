using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameOver : MonoBehaviour
{
    [SerializeField]Player player;

    private void playerDetected(){
        player.GetComponent<Player>().enabled = false;
    }


}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    [SerializeField]private Button Replaybutton;

    void Start(){
        Replaybutton.onClick.AddListener(OnClickPlay);
    }

    void OnClickPlay(){
        
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        Time.timeScale = 1;
    }
}

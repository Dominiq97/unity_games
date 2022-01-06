using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    
    public string menuScene;
    // Start is called before the first frame update

    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resume()
    {
        GameManager.instance.PauseUnpause();
    }
    public void menu()
    {
        SceneManager.LoadScene(menuScene);
    }
    public void quit()
    {
        Application.Quit();
    }
}

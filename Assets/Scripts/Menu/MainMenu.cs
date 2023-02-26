
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadScene(int number)
    {
        SceneManager.LoadScene(number);
    }

    public void Exit()
    {
        Application.Quit();
    }

}

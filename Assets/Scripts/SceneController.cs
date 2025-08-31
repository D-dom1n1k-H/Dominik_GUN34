
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    private int _index;

    public void OpenMainScene()
    {
        SceneManager.LoadScene(_index);
        Debug.Log("[SceneController] MainScene has been successfully loaded!");
    }
    public void OpenGameScene()
    {
        SceneManager.LoadScene(_index, LoadSceneMode.Additive);
        Debug.Log("[SceneController] GameScene has been opened addictive successfully!");
    }

    public void Start()
    {
        switch(_index)
        {
            case 0:
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    Debug.Log("[SceneController] MainScene is already loaded!");
                    return;
                }
                OpenMainScene();
                break;
            case 1:
                OpenGameScene();
                break;
        }
    }
}

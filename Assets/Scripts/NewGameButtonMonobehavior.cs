using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButtonMonobehavior : MonoBehaviour
{
    public void ChangeSceneToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}

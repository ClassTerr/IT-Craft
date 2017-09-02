using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadHelper : MonoBehaviour {
    //Загружает сцену с нужным индексом
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    //Загружает сцену с нужным именем
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}

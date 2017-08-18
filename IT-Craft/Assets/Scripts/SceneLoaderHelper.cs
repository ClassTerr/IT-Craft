using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderHelper : MonoBehaviour {
    //Загружает сцену с нужным индексом
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}

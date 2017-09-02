using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadHelper : MonoBehaviour {
    //Load scene with specified index
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    //Load scene with specified name
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHelper : MonoBehaviour {

    private Slider slider;

    // Use this for initialization
    void Start () {
        slider = GetComponentInChildren<Slider>();
    }

    // потихоньку подтягивает значение слайдера к целевому
    void Update () {
        slider.value += (TapManager.CurrentClickCount - slider.value) * 0.3f;
	}
}

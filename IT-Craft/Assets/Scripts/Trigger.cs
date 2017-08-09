using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
    public GameObject hero;

    private Animator anim;
	// Use this for initialization
	void Start () {
        anim = hero.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("FiveTimeTapped");
    }
}

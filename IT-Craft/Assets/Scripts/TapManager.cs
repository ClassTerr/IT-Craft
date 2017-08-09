using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapManager : MonoBehaviour {

    public float doubleClickInterval = 0.7f;
    public int animationTapCount = 5;
    private Animator anim = null;
    public static int CurrentClickCount = 0;

    public ParticleSystem bloodParticle = null;

    // Use this for initialization
    void Start () {
        prevTime = Time.fixedTime;
        anim = GetComponent<Animator>();

    }

    int c = 0;
    float prevTime = 0;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.A))
            anim.SetTrigger("FiveTimeTapped");


        RaycastHit hit;

        if (Time.fixedTime - prevTime > doubleClickInterval)
        {
                CurrentClickCount = 0;
                prevTime = Time.fixedTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider == null || hit.collider.gameObject == null)
                    return;

                if (hit.collider.gameObject.name == "HeadColliderCapsule")
                {
                    if (CurrentClickCount < 5)
                    {
                        CurrentClickCount++;
                        prevTime = Time.fixedTime;

                        if (CurrentClickCount == 5)
                        {
                            anim.SetTrigger("FiveTimeTapped");
                        }
                    }

                    //Play particles
                    Transform from = hit.collider.gameObject.transform;
                    Quaternion q = from.rotation;
                    Vector3 to = hit.point;

                    from.LookAt(to);

                    ParticleSystem particle = Instantiate(bloodParticle, hit.point, from.rotation);
                    from.rotation = q;
                    particle.transform.parent = hit.collider.gameObject.transform;
                    particle.transform.localScale.Set(0.5f, 0.5f, 0.5f);
                    Destroy(particle.gameObject, 0.75f);
                }
            }
        }
	}
}

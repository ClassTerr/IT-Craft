using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapManager : MonoBehaviour {

    /// <summary>
    /// Maximum time interval to continue tap combo
    /// </summary>
    public float doubleClickInterval = 0.7f;
    /// <summary>
    /// Tap count required to play animation
    /// </summary>
    public int animationTapCount = 5;
    /// <summary>
    /// How many times user tapped on head (turn to zero on idle)
    /// </summary>
    public static int CurrentClickCount = 0;
    public ParticleSystem bloodParticle = null;
    private Animator anim = null;
    int c = 0;
    float prevTime = 0;

    // Use this for initialization
    void Start () {
        prevTime = Time.fixedTime;
        anim = GetComponent<Animator>();

    }

	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKeyDown(KeyCode.A))
            anim.SetTrigger("FiveTimeTapped");
        */

        RaycastHit hit;

        // If user do not touch head for long time
        if (Time.fixedTime - prevTime > doubleClickInterval)
        {
                CurrentClickCount = 0;
                prevTime = Time.fixedTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // user tapped on screen
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider == null || hit.collider.gameObject == null)
                    return;

                // if head have been hitted
                if (hit.collider.gameObject.name == "HeadColliderCapsule")
                {
                    if (CurrentClickCount < 5)
                    {
						// play animation
                        CurrentClickCount++;
                        prevTime = Time.fixedTime;

                        if (CurrentClickCount == 5)
                        {
                            anim.SetTrigger("FiveTimeTapped");
                        }
                    }

                    // play particles
                    Transform from = hit.collider.gameObject.transform;
                    Quaternion q = from.rotation;
                    Vector3 to = hit.point;
                    // direct the partics from the center of the head
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapManager : MonoBehaviour {

    /// <summary>
    /// Максиимальный интервал времени между кликами для продолжения последовательности
    /// </summary>
    public float doubleClickInterval = 0.7f;
    /// <summary>
    /// Число тапов по голове для проигрывания анимации
    /// </summary>
    public int animationTapCount = 5;
    /// <summary>
    /// Сколько раз юзер раз клацнул на голову (обнуляется при простое)
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

        // если пользователь долго не клацал на голову
        if (Time.fixedTime - prevTime > doubleClickInterval)
        {
                CurrentClickCount = 0;
                prevTime = Time.fixedTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // пользователь нажал на экран
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider == null || hit.collider.gameObject == null)
                    return;

                // если попал на голову
                if (hit.collider.gameObject.name == "HeadColliderCapsule")
                {
                    //играть анимацию, если попал 5 раз подряд
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
                    // направляем партиклы из центра головы
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

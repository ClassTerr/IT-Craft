using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TargetDetectionHelper : MonoBehaviour,
                                            ITrackableEventHandler
{
    public GameObject TextToDisable = null;

    private TrackableBehaviour mTrackableBehaviour;

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            // Hide text when target is found
            if (TextToDisable != null)
                TextToDisable.SetActive(false);
            //Debug.Log("Found");
        }
    }
}

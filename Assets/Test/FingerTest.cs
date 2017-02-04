using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System.Linq;

public class FingerTest : MonoBehaviour {

    public GameObject magicalTeam;

    Controller controller;

	// Use this for initialization
	void Start () {
        controller = new Controller(); 
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = controller.Frame();
        HandList hands = frame.Hands;

        foreach(Hand hand in hands) {
            Finger indexFinger = hand.Fingers.Where(x => x.Type() == Finger.FingerType.TYPE_INDEX).ToArray()[0];
            Vector3 tipPos = indexFinger.TipPosition.ToUnityScaled() * 15;
            RaycastHit hit;
            if(Physics.Raycast(tipPos, indexFinger.Direction.ToUnity(), out hit, 15)) {
                magicalTeam.transform.position = hit.point + hit.normal * 0.1f;
                magicalTeam.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
            } else {
                magicalTeam.transform.position = tipPos + indexFinger.Direction.ToUnity() * 15;
                magicalTeam.transform.rotation = Quaternion.identity;
            }
        }
	}

    void OnApplicationQuit() {
        if (controller != null) {
            controller.Dispose();
            controller = null;
        }
    }

}

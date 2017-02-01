using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class GestureTest : MonoBehaviour {

    public float interval;
    public GameObject fire;
    public GameObject magicalTeam;

    Controller controller;
    Magic magic;
    float timer;

	// Use this for initialization
	void Start () {
        controller = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = controller.Frame();
        HandList hands = frame.Hands;
        Hand leftHand = null, rightHand = null;

        for (int i = 0; i < hands.Count; i++) {
            if (hands[i].IsLeft) {
                leftHand = hands[i];
            } else if (hands[i].IsRight) {
                rightHand = hands[i];
            }
        }

        if (leftHand == null || rightHand == null) return;

        var leftPos = leftHand.PalmPosition.ToUnityScaled() * 15;
        var rightPos = rightHand.PalmPosition.ToUnityScaled() * 15;
        var velocity = (leftHand.PalmVelocity + rightHand.PalmVelocity).ToUnityScaled() / 2;
        var dis = (leftPos - rightPos);

        if (magic == null && timer >= interval) {
            if(dis.magnitude <= 3.0f && Vector3.Dot(leftHand.PalmNormal.ToUnity(), rightHand.PalmNormal.ToUnity()) < -0.1f) {
                magic = new Fireball(leftHand, rightHand, 15, fire);
            } else if(Vector3.Dot(leftHand.PalmNormal.ToUnity(), transform.forward) >= 0.8f) {
                magic = new Shield(leftHand, rightHand, 15, magicalTeam);
                (magic as Shield).forwerd = transform.forward;
            }
        }

        if (magic != null) {
            magic.Update(leftHand, rightHand);
            var flag = magic.Action();
            if (flag) {
                magic.Destroy();
                magic = null;
                timer = 0;
            }
        }

        timer += Time.deltaTime;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class GestureTest : MonoBehaviour {

    public GameObject fire;
    public GameObject ball;
    public GameObject cube;

    public bool isLeftHold;
    public bool isRightHold;
    Controller controller;
    HandModel handModel;
    GameObject leftFireInstance;
    GameObject rightFireInstance;
    GameObject cubeInstance;
    float timer;

	// Use this for initialization
	void Start () {
        controller = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = controller.Frame();
        HandList hands = frame.Hands;
        List<Vector3> p = new List<Vector3>();
        Vector3 velocity = Vector3.zero;

        if (hands.Count > 1) {
            for (int i = 0; i < hands.Count; i++) {
                var pos = hands[i].PalmPosition.ToUnityScaled();
                var normal = hands[i].PalmNormal.ToUnityScaled().normalized;
                var v = hands[i].PalmVelocity.ToUnityScaled();

                p.Add(pos * 15);

                velocity = v + velocity;

                //if (hands[i].IsLeft) {
                //    isLeftHold = hands[i].GrabStrength > 0.5f;

                //    if(!isLeftHold && !leftFireInstance)
                //        leftFireInstance = Instantiate(fire, pos * 15 + normal, Quaternion.identity) as GameObject;
                //    if(leftFireInstance) 
                //        leftFireInstance.transform.position = pos * 15 + normal * 0.5f;
                //    if (isLeftHold) Destroy(leftFireInstance);

                //    var velocity = hands[i].PalmVelocity.ToUnityScaled();
                //    if (velocity.sqrMagnitude >= 0.5f && timer > 0.5f && !isLeftHold) {
                //        var b = Instantiate(ball, pos * 15 + normal * 2, Quaternion.identity) as GameObject;
                //        var body = b.GetComponent<Rigidbody>();
                //        body.AddForce(normal * 1000);
                //        timer = 0;
                //    }
                //} else if (hands[i].IsRight) { 
                //    isRightHold = hands[i].GrabStrength > 0.5f;

                //    if (!isRightHold && !rightFireInstance)
                //        rightFireInstance = Instantiate(fire, pos * 15 + normal, Quaternion.identity) as GameObject;
                //    if (rightFireInstance)
                //        rightFireInstance.transform.position = pos * 15 + normal * 0.5f;
                //    if (isRightHold) Destroy(rightFireInstance);

                //    var velocity = hands[i].PalmVelocity.ToUnityScaled();
                //    if (velocity.sqrMagnitude >= 0.5f && timer > 0.5f && !isRightHold) {
                //        var b = Instantiate(ball, pos * 15 + normal * 2, Quaternion.identity) as GameObject;
                //        var body = b.GetComponent<Rigidbody>();
                //        body.AddForce(normal * 1000);
                //        timer = 0;
                //    }
                //}
            }

            var dis = (p[1] - p[0]);
            if (!cubeInstance && timer >= 1.0f && dis.magnitude >= 2.0f) {
                cubeInstance = Instantiate(cube, Vector3.zero, Quaternion.identity) as GameObject;
            }

            if (cubeInstance) {
                cubeInstance.transform.position = (p[1] + p[0]) / 2;
                cubeInstance.GetComponent<Ball>().scale = dis.magnitude;

                if (velocity.magnitude / 2 >= 0.5f) {
                    var body = cubeInstance.GetComponent<Rigidbody>();
                    body.mass = dis.magnitude;
                    body.AddForce((velocity / 2).normalized * 50, ForceMode.Impulse);
                    cubeInstance.GetComponent<Collider>().isTrigger = false;
                    cubeInstance = null;
                    timer = 0;
                }
            }
        } else {
            if (cubeInstance) Destroy(cubeInstance);
        }

        timer += Time.deltaTime;
	}
}
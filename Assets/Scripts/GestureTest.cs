using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System.Linq;

public class GestureTest : MonoBehaviour {

    public float interval;
    public GameObject fire;
    public GameObject magicalTeam;
    public GameObject iceMagicalTeam;
    public GameObject ice;
    public GameObject lightningMagicalTeam;
    public GameObject lightning;

    Controller controller;
    Magic magic;
    float timer;
    Hand leftHand = null, rightHand = null;

    void Start () {
        controller = GetComponent<HandController>().GetLeapController();
	}
	
	void Update () {
        Frame frame = controller.Frame();
        HandList hands = frame.Hands;
        leftHand = null;
        rightHand = null;

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

        // 魔法の発動条件 
        if (timer >= interval) {
            Fireball(() => dis.magnitude <= 3.0f && Vector3.Dot(leftHand.PalmNormal.ToUnity(), rightHand.PalmNormal.ToUnity()) < -0.1);
            Shield(() => {
                var leftDot = Vector3.Dot(leftHand.PalmNormal.ToUnity(), transform.forward);
                var rightDot = Vector3.Dot(rightHand.PalmNormal.ToUnity(), transform.forward);
                return leftDot >= 0.8f && rightDot >= 0.8f;
            });
            Freeze(() => {
                int extendNum = leftHand.Fingers.Count(x => x.IsExtended);
                bool isIndex = leftHand.Fingers
                               .Where(x => x.IsExtended)
                               .Where(x => x.Type() == Finger.FingerType.TYPE_INDEX).ToArray().Length == 1;
                return extendNum == 1 && isIndex;
            });
            Lightning(() => {
                int extendNum = rightHand.Fingers.Count(x => x.IsExtended);
                bool isIndex = rightHand.Fingers
                               .Where(x => x.IsExtended)
                               .Where(x => x.Type() == Finger.FingerType.TYPE_INDEX).ToArray().Length == 1;
                return extendNum == 1 && isIndex;
            });
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

    // --- 魔法呼び出し ---
    void Fireball(System.Func<bool> func) {
        if(func.Invoke() && magic == null) {
            magic = new Fireball(leftHand, rightHand, 15, fire);
        }
    }

    void Shield(System.Func<bool> func) {
        if(func.Invoke() && magic == null) {
            magic = new Shield(leftHand, rightHand, 15, magicalTeam, transform.forward);
        }
    }

    void Freeze(System.Func<bool> func) {
        if(func.Invoke() && magic == null) {
            magic = new Freeze(leftHand, rightHand, 15, iceMagicalTeam, ice);
        }
    }

    void Lightning(System.Func<bool> func) {
        if(func.Invoke() && magic == null) {
            magic = new Lightning(leftHand, rightHand, 15, lightningMagicalTeam, lightning);
        }
    }
}
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
    public GameObject summonMagicalTeam;
    public GameObject golem;

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

        // 魔法の発動条件 
        if (timer >= interval) {
            Fireball();
            Shield();
            Freeze();
            Lightning();
            Summon();
        }

        // 魔法の更新
        if (magic != null) {
            magic.Update();
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
    // 火
    void Fireball() {
        var leftPos = leftHand.PalmPosition.ToUnityScaled() * 15;
        var rightPos = rightHand.PalmPosition.ToUnityScaled() * 15;
        var dis = (leftPos - rightPos);
        if(dis.magnitude <= 3.0f && Vector3.Dot(leftHand.PalmNormal.ToUnity(), rightHand.PalmNormal.ToUnity()) < -0.1 && magic == null) {
            magic = new Fireball(controller, 15, fire);
        }
    }
    // 氷
    void Freeze() {
        int extendNum = leftHand.Fingers.Count(x => x.IsExtended);
        bool isIndex = leftHand.Fingers
                               .Where(x => x.IsExtended)
                               .Where(x => x.Type() == Finger.FingerType.TYPE_INDEX).ToArray().Length == 1;
        if(extendNum <= 2 && isIndex && magic == null) {
            magic = new Freeze(controller, 15, iceMagicalTeam, ice);
        }
    }
    // 雷
    void Lightning() {
        int extendNum = rightHand.Fingers.Count(x => x.IsExtended);
        bool isIndex = rightHand.Fingers
                               .Where(x => x.IsExtended)
                               .Where(x => x.Type() == Finger.FingerType.TYPE_INDEX).ToArray().Length == 1;
        if(extendNum <= 2 && isIndex && magic == null) {
            magic = new Lightning(controller, 15, lightningMagicalTeam, lightning);
        }
    }
    // シールド
    void Shield() {
        var leftDot = Vector3.Dot(leftHand.PalmNormal.ToUnity(), transform.forward);
        var rightDot = Vector3.Dot(rightHand.PalmNormal.ToUnity(), transform.forward);
        if(leftDot >= 0.8f && rightDot >= 0.8f && magic == null) {
            magic = new Shield(controller, 15, magicalTeam, transform.forward);
        }
    }
    // 召喚
    void Summon() {
        var leftNormal = leftHand.PalmNormal.ToUnity();
        var rightNormal = rightHand.PalmNormal.ToUnity();
        if(Vector3.Dot(leftNormal, Vector3.down) >= 0.8f && Vector3.Dot(rightNormal, Vector3.down) >= 0.8f && magic == null) {
            magic = new Summon(controller, 15, summonMagicalTeam, golem);
        }
    }
}
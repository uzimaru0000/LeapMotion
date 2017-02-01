using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class Fireball : Magic {

    GameObject fireInstance;

    public Fireball(Hand _lHand, Hand _rHand, float _scale, GameObject _fire) : base(_lHand, _rHand, _scale) {
        fireInstance = Object.Instantiate(_fire, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public override bool Action() {
        var leftPos = leftHand.PalmPosition.ToUnityScaled() * scale;
        var rightPos = rightHand.PalmPosition.ToUnityScaled() * scale;
        var velocity = (leftHand.PalmVelocity + rightHand.PalmVelocity).ToUnityScaled() / 2;

        var dis = (leftPos - rightPos);

        fireInstance.transform.position = (leftPos + rightPos) / 2;
        fireInstance.GetComponent<Ball>().scale = dis.magnitude;

        if (velocity.magnitude >= 0.5f) {
            var body = fireInstance.GetComponent<Rigidbody>();
            body.mass = dis.magnitude;
            body.AddForce(velocity.normalized * 50, ForceMode.Impulse);
            fireInstance.GetComponent<Collider>().isTrigger = false;
            return true;
        }

        return false;
    }

    public override void Destroy() {
    }
}

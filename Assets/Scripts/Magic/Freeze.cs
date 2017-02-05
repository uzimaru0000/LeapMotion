using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System.Linq;

public class Freeze : Magic {

    GameObject ice;
    GameObject magicalTeam;
    GameObject iceParent;

    public Freeze(Controller _controller , float _scale, GameObject _magicalTeam, GameObject _ice) : base(_controller, _scale) {
        ice = _ice;
        magicalTeam = Object.Instantiate(_magicalTeam, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public override bool Action() {
        Finger indexFinger = leftHand.Fingers.Where(x => x.Type() == Finger.FingerType.TYPE_INDEX).ToArray()[0];
        Vector3 tipPos = indexFinger.TipPosition.ToUnityScaled() * scale;
        Vector3 dir = indexFinger.Direction.ToUnity();
        float grab = rightHand.GrabStrength;
        RaycastHit hit;
        int extendNum = leftHand.Fingers.Count(x => x.IsExtended);

        if(extendNum > 2 && !iceParent) {
            magicalTeam.GetComponent<magicalTeam>().Finish();
            return true;
        }

        if(!iceParent) {
            if (Physics.Raycast(tipPos, dir, out hit, 15)) {
                magicalTeam.transform.position = hit.point + hit.normal * 0.1f;
                magicalTeam.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
            } else {
                magicalTeam.transform.position = tipPos + indexFinger.Direction.ToUnity() * 15;
                magicalTeam.transform.rotation = Quaternion.identity;
            }
            if(grab >= 0.5f) {
                iceParent = Object.Instantiate(ice, magicalTeam.transform.position, Quaternion.identity) as GameObject;
                magicalTeam.GetComponent<magicalTeam>().Finish();
            }
        } else {
            if(grab < 0.5f) {
                iceParent.GetComponent<IceParent>().isKrush = true;
                return true;
            }
        }

        return false;
    }

    public override void Destroy() {
        
    }

}

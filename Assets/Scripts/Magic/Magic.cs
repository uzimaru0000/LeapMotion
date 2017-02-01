using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class Magic {

    public Hand leftHand;
    public Hand rightHand;

    protected float scale;

    public Magic(Hand _lHand, Hand _rHand, float _scale) {
        scale = _scale;
        Update(_lHand, _rHand);
    }

    public void Update(Hand _lHand, Hand _rHand) {
        leftHand = _lHand;
        rightHand = _rHand;
    }
    
    // 返り値 -> 魔法が完了したか
    public virtual bool Action() {
        return true;
    }
	
    // 魔法終了時に呼び出す(強制的に)
    public virtual void Destroy() {

    }
}

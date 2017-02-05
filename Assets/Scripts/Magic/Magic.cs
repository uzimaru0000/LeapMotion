using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System.Linq;

public class Magic {

    protected Controller controller;
    protected Hand leftHand;
    protected Hand rightHand;
    protected float scale;

    public Magic(Controller _controller, float _scale) {
        controller = _controller;
        scale = _scale;
        Update();
    }

    public void Update() {
        Frame frame = controller.Frame();
        leftHand = frame.Hands.Where(x => x.IsLeft).ToArray()[0];
        rightHand = frame.Hands.Where(x => x.IsRight).ToArray()[0];
    }
    
    // 返り値 -> 魔法が完了したか
    public virtual bool Action() {
        return true;
    }
	
    // 魔法終了時に呼び出す(強制的に)
    public virtual void Destroy() {

    }
}

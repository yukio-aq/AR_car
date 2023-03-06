using UnityEngine;
using System.Collections;

public class OnClickForScaling : MonoBehaviour {
	void OnMouseDown() {
        //オブジェクトごとにScalingManagerのScaleTransfromを変更する
		 ScalingManager.ScaleTransform = this.transform;
	}
}
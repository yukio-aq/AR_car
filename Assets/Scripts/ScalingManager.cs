using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingManager : MonoBehaviour
{

    public float initialFingersDistance;
    public Vector3 initialScale;
    public static Transform ScaleTransform;

    // Update is called once per frame
    void Update()
    {
		int fingersOnScreen = 0;

		foreach (Touch touch in Input.touches)
		{
			fingersOnScreen++; //タッチパネルのタッチ回数を計る

			//2本指の場合はオブジェクトの大きさを変更
			if (fingersOnScreen == 2)
			{

				//タッチ時に指１本目と２本目の距離をinitialScaleに代入
				if (touch.phase == TouchPhase.Began)
				{
					initialFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
					initialScale = ScaleTransform.localScale;
				}
				else
				{
					float currentFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);

					// 現在の２本の指の幅を最初の指の距離から割ってScaleFactorにする
					float scaleFactor = currentFingersDistance / initialFingersDistance;

					//localScaleをinitialScaleとScaleFactorでかけて更新する
					ScaleTransform.localScale = initialScale * scaleFactor;
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]

public class RotateController : MonoBehaviour 
{

	#region ROTATE
	private float _sensitivity = 1f;
	private Vector3 _mouseReference;
	private Vector3 _mouseOffset;
	private Vector3 _rotation = Vector3.zero;
	private bool _isRotating;


	#endregion

	void Update()
	{
		if(_isRotating)
		{
			// オフセット
			_mouseOffset = (Input.mousePosition - _mouseReference);

			// 回転量の計算
			_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

			// 実際に回転する
			gameObject.transform.Rotate(_rotation);

			// 指を位置を記録
			_mouseReference = Input.mousePosition;
		}
	}

	void OnMouseDown()
	{
		// オブジェクトを触れると回転するようにする
		_isRotating = true;

		// 触れた際の指の位置を記録
		_mouseReference = Input.mousePosition;
	}

	void OnMouseUp()
	{
		// タッチするのをやめたら回転できないようにする
		_isRotating = false;
	}

}
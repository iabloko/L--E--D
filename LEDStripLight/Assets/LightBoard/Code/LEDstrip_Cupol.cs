using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDstrip_Cupol : MonoBehaviour {
	[SerializeField] private MeshRenderer[] _VVertical;
	[SerializeField] private Color[] _Color;
	[SerializeField] private Material _LightOn, _LightOff, Cupol_Gray;

	private int b = 1;
	private WaitForSeconds _waitForSeconds;

	private void Awake () {
		_waitForSeconds = new WaitForSeconds (Time.deltaTime);
	}
	void Update () {
		Debug.Log ("1");
		if (Input.GetKeyUp (KeyCode.Space)) {
			Debug.Log ("Space");
			StartCoroutine ("StartAnimation");
		}
	}

	private IEnumerator StartAnimation () {
		int i = 0;
		for (i = 0; i < _VVertical.Length; i++) {
			if (_VVertical[i].material != _LightOn) {
				_VVertical[i].material = _LightOn;
				if (i == 1) {
					StartCoroutine ("AnimationOff");
				}
				yield return _waitForSeconds;
			} else {
				Debug.Log ("yield return null");
				yield return null;
			}
		}
	}
	private IEnumerator AnimationOff () {
		int i = 0;
		for (i = 0; i < _VVertical.Length - b; i++) {
			_VVertical[i].material = _LightOff;
			if (i == _VVertical.Length - 1 - b) {
				b++;
				StartCoroutine ("StartAnimation");
			}
			yield return _waitForSeconds;
		}
	}
}
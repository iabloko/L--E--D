using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LEDstrip : MonoBehaviour {

	//1 - StripLengthAnimation
	//2 - ColorChange, SpeedChange
	//3 - Stop_All_Coroutiones_AndStart_Star_Effect
	//4 - ArcEffect
	//5 - StarEffect

	[System.Serializable]
	public class HHorizontal : System.Object {
		public MeshRenderer[] _HMesh;
	}

	[System.Serializable]
	public class VVertical : System.Object {
		public MeshRenderer[] _VMesh;
	}

	[SerializeField] private bool MainBool = false;
	[SerializeField] private int StripLength = 1; //Длинна подсвеченных элементов
	[SerializeField] private int LengthStripVoid = 3; //Удлиненние расстояния между подсвеченными элементами
	[SerializeField] private List<HHorizontal> _HHorizontal;
	[SerializeField] private List<VVertical> _VVertical;
	[SerializeField] private Color[] _Color;
	[SerializeField] private Material _LightOn, _LightOff, _LightSet, ArcMaterial;
	[Range (0.001f, 0.1f)]
	[SerializeField] private float Speed = 0.06f;
	[SerializeField] private MeshRenderer Iron;

	[SerializeField] private Light Light_1;
	[SerializeField] private Light Light_2;
	[SerializeField] private Light Light_3;
	//[Range (0.01f, 5)]
	//[SerializeField] private float frequency = 0.1f;

	private WaitForSeconds _WaitForSeconds, _WaitForSeconds2, WfS_SpeedChange, Wfs_TimedeltaTime;
	private int HHhorizontalCount = 0;

	void Awake () {
		HHhorizontalCount = _HHorizontal.Count;
		_WaitForSeconds = new WaitForSeconds (0.035f);
		_WaitForSeconds2 = new WaitForSeconds (0.05f);
		Wfs_TimedeltaTime = new WaitForSeconds (Time.deltaTime);
		WfS_SpeedChange = new WaitForSeconds (0.05f);

		ResetSettiong_Materials ();
	}

	private void ResetSettiong_Materials () {
		ArcMaterial.color = _Color[9];
		_LightSet.color = Color.black;
		_LightSet.SetColor ("_EmissionColor", Color.black);
	}

	void Update () {
		//Debug.Log("t"+Time.time);
		if (Input.GetKeyUp (KeyCode.Space)) {
			StartCoroutine ("StripLengthAnimation");

			//StartCoroutine (horizontalEffect ());
			//StartCoroutine ("horizontalAll");
			//StartCoroutine ("VerticalEffect");
			//StartCoroutine (ArcEffect (MainBool));
			//StartCoroutine ("FlicceringPhase_1");
			//StartCoroutine ("StarEffect");
			//StartCoroutine ("SpeedChange");
		}
	}
	#region StripLengthAnimation
	private IEnumerator StripLengthAnimation () {
		int i = 0, b = 0;
		bool _inversive = false;
		StartCoroutine (ColorChange (_Color[2], _Color[3]));
		StartCoroutine ("SpeedChange");
		for (b = 0; b < HHhorizontalCount; b++) {
			if (b == 1 || b == 2 || b == 3 || b == 5 || b == 6 || b == 7 || b == 9 || b == 10 || b == 11) {
				_inversive = b % 3 == 0 ? true : false;
				if (!_inversive) {
					for (i = 0; i < _HHorizontal[b]._HMesh.Length; i++) {
						_HHorizontal[b]._HMesh[i].material = _LightOn;
						_HHorizontal[b]._HMesh[i].material.color = _Color[0];
						_HHorizontal[b]._HMesh[i].material.SetColor ("_EmissionColor", _Color[0]);
						if (i == StripLength) {
							StartCoroutine (StripLengthAnimationOff (b, _inversive));
						}
					}
				} else { //inversive==true;
					for (i = _HHorizontal[b]._HMesh.Length - 1; i > 0; i--) {
						_HHorizontal[b]._HMesh[i].material = _LightOn;
						_HHorizontal[b]._HMesh[i].material.color = _Color[1];
						_HHorizontal[b]._HMesh[i].material.SetColor ("_EmissionColor", _Color[1]);
						if (i == (_HHorizontal[b]._HMesh.Length - StripLength - LengthStripVoid)) {
							StartCoroutine (StripLengthAnimationOff (b, _inversive));
						}
					}
				}
			}
		}
		yield return null;
	}
	private IEnumerator StripLengthAnimationOff (int _b, bool inversive) {
		int i = 0;
		if (!inversive) {
			for (i = 0; i < _HHorizontal[_b]._HMesh.Length; i++) {
				_HHorizontal[_b]._HMesh[i].material = _LightOff;
				if (i == StripLength) { //Длинна пустого пространства
					StartCoroutine (StripLengthAnimationOn (_b, inversive));
				}
				yield return new WaitForSeconds (Speed);
			}
		} else { //inversive==true;
			for (i = _HHorizontal[_b]._HMesh.Length - 1; i > 0; i--) {
				_HHorizontal[_b]._HMesh[i].material = _LightOff;
				if (i == (_HHorizontal[_b]._HMesh.Length - StripLength - LengthStripVoid)) { //Длинна пустого пространства
					StartCoroutine (StripLengthAnimationOn (_b, inversive));
				}
				yield return new WaitForSeconds (Speed);
			}
		}
	}
	private IEnumerator StripLengthAnimationOn (int _b, bool inversive) {
		int i = 0;
		if (!inversive) {
			for (i = 0; i < _HHorizontal[_b]._HMesh.Length; i++) {
				_HHorizontal[_b]._HMesh[i].material = _LightOn;
				_HHorizontal[_b]._HMesh[i].material.color = _Color[0];
				_HHorizontal[_b]._HMesh[i].material.SetColor ("_EmissionColor", _Color[0]); //Меняем цвет для последующих повторений
				if (i == StripLength) {
					StartCoroutine (StripLengthAnimationOff (_b, inversive));
				}
				yield return new WaitForSeconds (Speed);
			}
		} else { //inversive==true;
			for (i = _HHorizontal[_b]._HMesh.Length - 1; i > 0; i--) {
				_HHorizontal[_b]._HMesh[i].material = _LightOn;
				_HHorizontal[_b]._HMesh[i].material.color = _Color[1];
				_HHorizontal[_b]._HMesh[i].material.SetColor ("_EmissionColor", _Color[1]); //Меняем цвет для последующих повторений
				if (i == (_HHorizontal[_b]._HMesh.Length - StripLength - LengthStripVoid)) {
					StartCoroutine (StripLengthAnimationOff (_b, inversive));
				}
				yield return new WaitForSeconds (Speed);
			}
		}
	}
	#endregion

	#region Fliccering
	private IEnumerator FlicceringPhase_1 () {
		int i = 0, b = 0;
		for (b = 0; b < _VVertical.Count; b++) {
			Color CColor = _Color[Random.Range (0, _Color.Length)];
			for (i = 0; i < _VVertical[b]._VMesh.Length; i++) {
				_VVertical[b]._VMesh[i].material = _LightOn;
				_VVertical[b]._VMesh[i].material.color = CColor;
				_VVertical[b]._VMesh[i].material.SetColor ("_EmissionColor", CColor);
			}
			if (b == _VVertical.Count / 3) {
				StartCoroutine ("FlicceringPhase_1_end");
			}
			yield return _WaitForSeconds2;
		}
	}
	private IEnumerator FlicceringPhase_1_end () {
		int i = 0, b = 0;
		for (b = 0; b < _VVertical.Count; b++) {
			for (i = 0; i < _VVertical[b]._VMesh.Length; i++) {
				_VVertical[b]._VMesh[i].material = _LightOff;
				_VVertical[b]._VMesh[i].material.color = Color.black;
				_VVertical[b]._VMesh[i].material.SetColor ("_EmissionColor", Color.black);
				if (b == _VVertical.Count - 1 && i == _VVertical[b]._VMesh.Length - 1) {
					StartCoroutine (Fliccering_main (0.05f));

					yield return new WaitForSeconds (0.2f);
					StartCoroutine (Fliccering_main (0.09f));

					yield return new WaitForSeconds (0.3f);
					StartCoroutine (Fliccering_main (0.13f));

					yield return new WaitForSeconds (0.1f);
					StartCoroutine (Fliccering_main (0.07f));
				}
			}
			yield return _WaitForSeconds2;
		}
	}

	private IEnumerator Fliccering_main (float WFS) {
		int i = 0, b = Random.Range (0, _VVertical.Count);
		if (b == 0 || b == 2 || b == 4 || b == 7 || b == 9 || b == 11) {
			for (i = 0; i < _VVertical[b]._VMesh.Length; i++) {
				_VVertical[b]._VMesh[i].material = _LightOn;
				_VVertical[b]._VMesh[i].material.color = Color.red;
				_VVertical[b]._VMesh[i].material.SetColor ("_EmissionColor", Color.red);
				if (i == _VVertical[b]._VMesh.Length - 1) {
					Material_Light_On ();
					yield return new WaitForSeconds (WFS * 1.2f);
					StartCoroutine (Fliccering_mainEnd (b, WFS));
				}
			}
		} else {
			yield return new WaitForSeconds (WFS);
			StartCoroutine (Fliccering_mainEnd (b, WFS));
		}

	}
	private IEnumerator Fliccering_mainEnd (int _Check, float WFS) {
		int i = 0;
		for (i = 0; i < _VVertical[_Check]._VMesh.Length; i++) {
			_VVertical[_Check]._VMesh[i].material = _LightOff;
			_VVertical[_Check]._VMesh[i].material.color = Color.black;
			_VVertical[_Check]._VMesh[i].material.SetColor ("_EmissionColor", Color.black);
			if (i == _VVertical[_Check]._VMesh.Length - 1) {
				Material_Light_Off ();
				yield return new WaitForSeconds (WFS * 1.2f);
				StartCoroutine (Fliccering_main (WFS));
			}
		}
	}
	#endregion

	private void Material_Light_Off () {
		ArcMaterial.color = _Color[8];
		Iron.material.color = _Color[8];
		Light_1.enabled = false;
		Light_2.enabled = false;
		Light_3.enabled = false;
	}
	private void Material_Light_On () {
		ArcMaterial.color = _Color[7];
		Iron.material.color = _Color[7];
		Light_1.enabled = true;
		Light_1.color = _Color[5];
		Light_2.enabled = true;
		Light_2.color = _Color[5];
		Light_3.enabled = true;
		Light_3.color = _Color[5];
	}

	#region ColorChange and SpeedChange
	private IEnumerator ColorChange (Color color_2, Color color_3) {
		_Color[0] = Color.black;
		_Color[1] = Color.black;
		Debug.Log ("ColorChange");
		yield return new WaitForSeconds (1f);
		while (_Color[1] != _Color[3]) {
			_Color[0] = Color.Lerp (_Color[0], color_2, Time.deltaTime);
			_Color[1] = Color.Lerp (_Color[1], color_3, Time.deltaTime);
			yield return Wfs_TimedeltaTime;
		}
	}

	private IEnumerator SpeedChange () {
		bool _Check = true, _Check2 = true;
		yield return new WaitForSeconds (3f);
		while (Speed != 0.001f) {
			Speed = Mathf.Lerp (Speed, 0.001f, Time.deltaTime * 1.25f);

			if (_Check == true && Speed <= 0.015f) {
				_Check = false;
				StartCoroutine ("FlicceringPhase_1");
			}
			if (Speed <= 0.0025f && _Check2 == true) {
				_Check2 = false;
				StartCoroutine ("Stop_All_Coroutiones_AndStart_Star_Effect");
			}
			yield return WfS_SpeedChange;
		}
	}
	#endregion

	#region Stop_All_Coroutiones_AndStart_Star_Effect
	private IEnumerator Stop_All_Coroutiones_AndStart_Star_Effect () {
		int i = 0, b = 0;
		_Color[0] = Color.black;
		_Color[1] = Color.black;
		Material_Light_Off ();
		StopAllCoroutines ();
		for (b = 0; b < _VVertical.Count; b++) {
			for (i = 0; i < _VVertical[b]._VMesh.Length; i++) {
				_VVertical[b]._VMesh[i].material = _LightOff;
			}
		}
		for (b = 0; b < _HHorizontal.Count; b++) {
			for (i = 0; i < _HHorizontal[b]._HMesh.Length; i++) {
				_HHorizontal[b]._HMesh[i].material = _LightOff;
			}
		}
		Iron.material.color = _Color[8];
		Light_1.enabled = false;
		Light_2.enabled = false;
		Light_3.enabled = false;
		StartCoroutine (ArcEffect (MainBool));
		yield return null;
	}
	#endregion

	#region ArcEffect
	private IEnumerator ArcEffect (bool isInversive) {
		int i = 0, b = 0;
		yield return new WaitForSeconds (0.75f);
		if (!isInversive) {
			for (b = 0; b < HHhorizontalCount; b++) {
				for (i = 0; i < _HHorizontal[b]._HMesh.Length; i++) {
					//Debug.Log ("ArcEffect_Count" + " = " + ArcEffect_Count);
					_HHorizontal[b]._HMesh[i].material = _LightOn;
					_HHorizontal[b]._HMesh[i].material.color = _Color[2];
					_HHorizontal[b]._HMesh[i].material.SetColor ("_EmissionColor", _Color[2]);

				}
				if (b == 3) {
					StartCoroutine (ArcEffectOff (MainBool));
				}

				yield return _WaitForSeconds;
			}
		} else if (isInversive) {
			for (b = HHhorizontalCount - 1; b > 0; b--) {
				for (i = _HHorizontal[b]._HMesh.Length - 1; i > 0; i--) {
					_HHorizontal[b]._HMesh[i].material = _LightOn;
					_HHorizontal[b]._HMesh[i].material.color = Color.white;
				}
				if (b == 2) {
					StartCoroutine (ArcEffectOff (MainBool));
				}
				yield return _WaitForSeconds;
			}
		}
	}

	private IEnumerator ArcEffectOff (bool isInversive) {
		int i = 0, b = 0;
		if (!isInversive) {
			for (b = 0; b < HHhorizontalCount; b++) {
				for (i = 0; i < _HHorizontal[b]._HMesh.Length; i++) {
					_HHorizontal[b]._HMesh[i].material = _LightOff;
					_HHorizontal[b]._HMesh[i].material.color = Color.black;
					_HHorizontal[b]._HMesh[i].material.SetColor ("_EmissionColor", Color.black);
				}
				yield return _WaitForSeconds;
			}

		} else if (isInversive) {
			for (b = HHhorizontalCount - 1; b > 0; b--) {
				for (i = _HHorizontal[b]._HMesh.Length - 1; i > 0; i--) {
					_HHorizontal[b]._HMesh[i].material = _LightOff;
					_HHorizontal[b]._HMesh[i].material.color = Color.black;
				}
				yield return _WaitForSeconds;
			}
		}
		StartCoroutine ("_SetMaterial");
	}
	private IEnumerator _SetMaterial () {
		int i = 0, b = 0;
		for (b = 0; b < HHhorizontalCount; b++) {
			for (i = 0; i < _HHorizontal[b]._HMesh.Length; i++) {
				if ((i + b) % 5 < 1) {
					_HHorizontal[b]._HMesh[i].material = _LightSet;
				}
			}
		}
		for (b = 0; b < HHhorizontalCount; b++) {
			for (i = 0; i < _HHorizontal[b]._HMesh.Length; i++) {
				if ((i + b) % 5 < 1) {
					_VVertical[b]._VMesh[i].material = _LightSet;
				}
			}
		}

		StartCoroutine ("ColorChange_2");
		yield return null;
	}
	private IEnumerator ColorChange_2 () {
		Debug.Log ("ColorChange_2");
		_LightSet.color = _Color[0];
		yield return new WaitForSeconds (1f);
		while (_Color[0] != _Color[2]) {
			_Color[0] = Color.Lerp (_Color[0], _Color[2], Time.deltaTime);
			_LightSet.color = _Color[0];
			_LightSet.SetColor ("_EmissionColor", _Color[0]);
			yield return Wfs_TimedeltaTime;
		}
	}

	private IEnumerator ColorChangeMain (int i, Color _color1, Color _color2, Material _Material) {
		_Color[0] = _Color[8];
		Debug.Log ("ColorChangeMain");
		while (_color1 != _color2) {
			_Color[0] = Color.Lerp (_color1, _color2, Time.deltaTime);
			_Material.color = _Color[0];
			_Material.SetColor ("_EmissionColor", _Color[0]);
			yield return Wfs_TimedeltaTime;
			yield return Wfs_TimedeltaTime;
		}
	}

	#endregion
}

/*	#region ColorChange and SpeedChange
	private IEnumerator ColorChange (Color color_2, Color color_3) {
		_Color[0] = Color.black;
		_Color[1] = Color.black;
		Debug.Log ("ColorChange");
		yield return new WaitForSeconds (1f);
		while (_Color[1] != _Color[3]) {
			_Color[0] = Color.Lerp (_Color[0], color_2, Time.deltaTime);
			_Color[1] = Color.Lerp (_Color[1], color_3, Time.deltaTime);
			yield return Wfs_TimedeltaTime;
		}
	}
 */

/*	#region HorizontalEffect
	private IEnumerator horizontalEffect () {
		int b = Random.Range (0, _HHorizontal.Count), i = 0; //_HHorizontal.Count
		for (i = 0; i <= _HHorizontal[b]._HMesh.Length - 1; i++) {
			_HHorizontal[b]._HMesh[i].material = _LightOn;
			if (i == 20) {
				StartCoroutine (horizontalEffectOff (b));
			}
			yield return _WaitForSeconds;
		}
	}
	private IEnumerator horizontalEffectOff (int _Check) {
		int i = 0;
		for (i = 0; i <= _HHorizontal[_Check]._HMesh.Length - 1; i++) {
			_HHorizontal[_Check]._HMesh[i].material = _LightOff;
			yield return _WaitForSeconds;
		}
	}
	private IEnumerator horizontalAll () {
		int b = 0, i = 0; //_HHorizontal.Count
		for (b = 0; b <= _HHorizontal.Count - 1; b++) {
			for (i = 0; i <= _HHorizontal[b]._HMesh.Length - 1; i++) {
				_HHorizontal[b]._HMesh[i].material = _LightOn;
				if (b == _HHorizontal.Count - 1 && i == _HHorizontal[b]._HMesh.Length - 1)
					StartCoroutine ("horizontalAlloff");
			}
			yield return _WaitForSeconds;
		}
	}
	private IEnumerator horizontalAlloff () {
		int b = 0, i = 0; //_HHorizontal.Count
		for (b = 0; b <= _HHorizontal.Count - 1; b++) {
			for (i = 0; i <= _HHorizontal[b]._HMesh.Length - 1; i++) {
				_HHorizontal[b]._HMesh[i].material = _LightOff;
			}
			yield return _WaitForSeconds;
		}
	}
	#endregion

	#region VerticalEffect
	private IEnumerator VerticalEffect () {
		int b = Random.Range (0, _HHorizontal.Count), i = 0;
		for (i = 0; i < _VVertical[b]._VMesh.Length - 1; i++) {
			_VVertical[b]._VMesh[i].material = _LightOn;
			if (i == 49) {
				StartCoroutine (VerticalEffectOff (b));
			}
			yield return _WaitForSeconds;
		}
	}
	private IEnumerator VerticalEffectOff (int _Check) {
		int i = 0;
		for (i = 0; i < _VVertical[_Check]._VMesh.Length; i++) {
			_VVertical[_Check]._VMesh[i].material = _LightOff;
			if (i == _VVertical[_Check]._VMesh.Length - 1) {
				StartCoroutine (VerticalEffect ());
			}
			yield return _WaitForSeconds;
		}
	}
	#endregion

	#region ArcEffect
	private IEnumerator ArcEffect (bool isInversive) {
		int i = 0, b = 0;
		if (!isInversive) {
			for (b = 0; b < HHhorizontalCount; b++) {
				for (i = 0; i < _HHorizontal[b]._HMesh.Length; i++) {
					_HHorizontal[b]._HMesh[i].material = _LightOn;
				}
				if (b == 7) {
					StartCoroutine (ArcEffectOff (MainBool));
				}
				yield return _WaitForSeconds;
			}
		} else if (isInversive) {
			for (b = HHhorizontalCount - 1; b > 0; b--) {
				for (i = _HHorizontal[b]._HMesh.Length - 1; i > 0; i--) {
					_HHorizontal[b]._HMesh[i].material = _LightOn;
				}
				if (b == 2) {
					StartCoroutine (ArcEffectOff (MainBool));
				}
				yield return _WaitForSeconds;
			}
		}
	}
	private IEnumerator ArcEffectOff (bool isInversive) {
		int i = 0, b = 0;
		if (!isInversive) {
			for (b = 0; b < HHhorizontalCount; b++) {
				for (i = 0; i < _HHorizontal[b]._HMesh.Length; i++) {
					_HHorizontal[b]._HMesh[i].material = _LightOff;
				}
				yield return _WaitForSeconds;
			}

		} else if (isInversive) {
			for (b = HHhorizontalCount - 1; b > 0; b--) {
				for (i = _HHorizontal[b]._HMesh.Length - 1; i > 0; i--) {
					_HHorizontal[b]._HMesh[i].material = _LightOff;
				}
				yield return _WaitForSeconds;
			}
		}
	}
	#endregion

 */
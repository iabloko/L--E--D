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

	private WaitForSeconds _WaitForSeconds, _WaitForSeconds2, WfS_SpeedChange, Wfs_TimedeltaTime;
	private int HHhorizontalCount = 0;
	private int ArcEffectCounter = 0;

	void Awake () {
		HHhorizontalCount = _HHorizontal.Count;
		_WaitForSeconds = new WaitForSeconds (0.035f);
		_WaitForSeconds2 = new WaitForSeconds (0.05f);
		Wfs_TimedeltaTime = new WaitForSeconds (Time.deltaTime);
		WfS_SpeedChange = new WaitForSeconds (0.05f);

		ResetSettiong_Materials ();
	}

	private void ResetSettiong_Materials () {
		ArcMaterial.color = _Color[7];
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
			//StartCoroutine (ArcEffect (MainBool, _Color[2]));
			//StartCoroutine ("FlicceringPhase_1");
			//StartCoroutine ("SpeedChange");
			//StartCoroutine ("TheCometEffect");
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
	private IEnumerator FlicceringPhase_1 () { //Волна с первой до послденей
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
	private IEnumerator FlicceringPhase_1_end () { //Волна с первой до послденей
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

	private IEnumerator Fliccering_main (float WFS) { //Включаем псевдо рандомный LED_Strip
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
	private IEnumerator Fliccering_mainEnd (int _Check, float WFS) { //Выключаем псевдо рандомный LED_Strip
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
		ArcMaterial.color = _Color[8]; //new Color(45,45,45);
		Iron.material.color = _Color[8];
		Iron.material.SetColor ("_EmissionColor", _Color[8]);
		Light_1.enabled = false;
		Light_2.enabled = false;
		Light_3.enabled = false;
	}
	private void Material_Light_On () {
		ArcMaterial.color = _Color[7];
		Iron.material.color = _Color[7];
		Iron.material.SetColor ("_EmissionColor", _Color[7]);
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
		Material_Light_Off ();
		StartCoroutine (ArcEffect (MainBool, _Color[10]));
		yield return null;
	}
	#endregion

	#region ArcEffect
	private IEnumerator ArcEffect (bool isInversive, Color _Color) {
		int i = 0, b = 0;
		yield return new WaitForSeconds (1f);
		if (!isInversive) {
			for (b = 0; b < HHhorizontalCount; b++) {
				for (i = 0; i < _HHorizontal[b]._HMesh.Length; i++) {
					_HHorizontal[b]._HMesh[i].material = _LightOn;
					_HHorizontal[b]._HMesh[i].material.color = _Color;
					_HHorizontal[b]._HMesh[i].material.SetColor ("_EmissionColor", _Color);
				}
				if (b == 3 + ArcEffectCounter) {
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
					if (b == HHhorizontalCount - 1 && i == _HHorizontal[b]._HMesh.Length - 1 && ArcEffectCounter < 2) {
						ArcEffectCounter++;
						yield return new WaitForSeconds ((0.25f + ArcEffectCounter) / 2);
						StartCoroutine (ArcEffect (false, _Color[10 + ArcEffectCounter]));
					} else if (b == HHhorizontalCount - 1 && i == _HHorizontal[b]._HMesh.Length - 1 && ArcEffectCounter == 2) {
						StartCoroutine ("_SetMaterial");
					}
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
		for (b = 0; b < _VVertical.Count; b++) {
			for (i = 0; i < _VVertical[b]._VMesh.Length; i++) {
				if ((i + b) % 5 < 1) {
					_VVertical[b]._VMesh[i].material = _LightSet;
				}
			}
		}
		StartCoroutine ("ColorChange_2");
		yield return null;
	}

	private IEnumerator ColorChange_2 () {
		_LightSet.color = _Color[0];
		_LightSet.SetColor ("_EmissionColor", _Color[0]);
		yield return new WaitForSeconds (1f);
		while (_Color[0] != _Color[2]) {
			_Color[0] = Color.Lerp (_Color[0], _Color[2], Time.deltaTime * 3);
			_LightSet.color = _Color[0];
			_LightSet.SetColor ("_EmissionColor", _Color[0]);
			yield return Wfs_TimedeltaTime;
		}
		StartCoroutine ("TheCometEffect");
	}
	#endregion

	#region TheCometEffect
	private IEnumerator TheCometEffect () {
		int b = Random.Range (0, _VVertical.Count), i = _VVertical[b]._VMesh.Length - 1;
		for (i = _VVertical[b]._VMesh.Length - 1; i > 0; i--) {
			_VVertical[b]._VMesh[i].material = _LightOn;
			//_VVertical[b]._VMesh[i].material.color = _Color[Random.Range(0,10)];
			//_VVertical[b]._VMesh[i].material.SetColor("_EmissionColor",_VVertical[b]._VMesh[i].material.color);
			if (i == (_VVertical[b]._VMesh.Length - 7)) {
				StartCoroutine (TheCometEffect_Off (b));
			}
			yield return Wfs_TimedeltaTime;
		}
	}
	private IEnumerator TheCometEffect_Off (int _b) {
		int i = _VVertical[_b]._VMesh.Length - 1;
		for (i = _VVertical[_b]._VMesh.Length - 1; i > 0; i--) {
			_VVertical[_b]._VMesh[i].material = _LightOff;
			if (i == (_VVertical[_b]._VMesh.Length - 7)) { //Длинна пустого пространства
				StartCoroutine ("TheCometEffect");
			}
			yield return Wfs_TimedeltaTime;
		}
	}
	#endregion
}
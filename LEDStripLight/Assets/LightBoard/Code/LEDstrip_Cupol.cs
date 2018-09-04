using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDstrip_Cupol : MonoBehaviour {

	[System.Serializable]
	public class VVertical : System.Object {
		public MeshRenderer[] _VMesh;
	}

	[SerializeField] private List<VVertical> _VVertical;
	[SerializeField] private Color[] _Color;
	[SerializeField] private Material _LightOn, _LightOff;
}
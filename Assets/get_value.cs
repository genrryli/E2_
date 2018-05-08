using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class get_value : MonoBehaviour {

    public Slider s;

	void Update () {
        gameObject.GetComponent<Text>().text = s.value.ToString();
	}
}

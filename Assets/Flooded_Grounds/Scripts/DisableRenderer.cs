using UnityEngine;

public class DisableRenderer : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		GetComponent<Renderer>().enabled = false;

	}

	// Update is called once per frame
	void Update()
	{

	}
}

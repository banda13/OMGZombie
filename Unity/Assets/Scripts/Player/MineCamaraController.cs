using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCamaraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(startFading());
    }
	
	// Update is called once per frame
	void Update () {
        rotateWord();
	}

    private void rotateWord()
    {
        Transform cameraPos = transform.GetChild(0);
        transform.GetChild(3).transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y, cameraPos.position.z) + (cameraPos.forward) * 0.1f;
        transform.GetChild(3).transform.LookAt(cameraPos);
    }

    private IEnumerator startFading()
    {
        float fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }
    
    public IEnumerator fadingWithCartActions(cartController.fadingAction f)
    {
        float fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        f();
        fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }
}

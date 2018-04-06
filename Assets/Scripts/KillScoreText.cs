using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScoreText : MonoBehaviour 
{
    [SerializeField]
    private TextMesh textMesh;
    [SerializeField]
    private new MeshRenderer renderer;
    private float currentAlpha = 1;

	void Update() {
        currentAlpha -= Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + (2 * Time.deltaTime), transform.position.z);
        renderer.material.color = new Color(1, 1, 0, currentAlpha);
	}
}

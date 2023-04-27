using UnityEngine;
using System.Collections;

public class T_Rotation : MonoBehaviour {

    [SerializeField]private Vector3 t_R_Value;
    
	// Update is called once per frame
	void Update () {
        StartCoroutine(T_Rotate_());
	}

    IEnumerator T_Rotate_()
    {
        transform.Rotate(t_R_Value*Time.deltaTime);
        yield return null;
    }
}

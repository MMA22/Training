using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM3 : MonoBehaviour {

    public GameObject prefab;

    void FixedUpdate() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100)) {
                if (hit.collider.tag == "ground") {
                    Debug.Log(hit.point);
                    Instantiate(prefab, hit.point, Quaternion.identity);
                }   
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class LineTriggers : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Line otherLine = other.GetComponentInParent<Line>();
        if (otherLine != null)
        {
            Debug.Log("Çizgi, baþka bir çizgiyle çarpýþtý: " + other.transform.parent.gameObject.name);
            // Çarpýþma olayýný burada iþleyin
        }
    }

}

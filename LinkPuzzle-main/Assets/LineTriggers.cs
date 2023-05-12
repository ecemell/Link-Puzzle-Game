using System.Collections.Generic;
using UnityEngine;

public class LineTriggers : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Line otherLine = other.GetComponentInParent<Line>();
        if (otherLine != null)
        {
            Debug.Log("�izgi, ba�ka bir �izgiyle �arp��t�: " + other.transform.parent.gameObject.name);
            // �arp��ma olay�n� burada i�leyin
        }
    }

}

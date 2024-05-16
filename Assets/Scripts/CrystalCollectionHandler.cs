using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalCollectionHandler : MonoBehaviour
{
    private int Crystal = 0;
    public TextMeshProUGUI scoreText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Crystal")
        {
            Crystal++;
            scoreText.text = "Score: " + (Crystal * 100).ToString();
            Destroy(other.gameObject);
        }
    }
}

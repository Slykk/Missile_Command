using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    private int scaler;
    public GameObject numbersParent;
    
    // Start is called before the first frame update
    void Start()
    {
        // scaler = 108;
        // scaler = 1;

        // Sequence mySeq = DOTween.Sequence();
        // mySeq.Append(transform.DOLocalMove(new Vector3(0*scaler,5*scaler,0), 5f));
    }

    public void Init(string number, Vector3 spawnPosition)
    {
        // Camera.main.ScreenToWorldPoint()

        // Text text = this.GetComponent<Text>();
        // text.text = number;
        // transform.SetParent(GameObject.Find("FloatingTextParent").transform, false);
        // transform.position = new Vector3(5*scaler, 5*scaler, 0);

        transform.position = spawnPosition;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

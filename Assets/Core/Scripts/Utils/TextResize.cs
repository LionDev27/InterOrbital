using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextResize : MonoBehaviour
{
    private TextMeshProUGUI _text;
    // Start is called before the first frame update
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_text.text.Length > 2)
        {
            _text.fontSize = 16;
        }
        else
        {
            _text.fontSize = 32;
        }
    }
}

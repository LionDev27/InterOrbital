using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberPopup : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    private float dissappearTimer;
    private Color textColor;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }

    public void Setup(int dmg)
    {
        _textMeshPro.SetText(dmg.ToString());
        textColor = _textMeshPro.color;
    }

    private void Update()
    {
        float moveYSpeed = 3f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        dissappearTimer -= Time.deltaTime;
        if(dissappearTimer < 0)
        {
            float dissapearSpeed = 3f;
            textColor.a -= dissapearSpeed * Time.deltaTime;
            _textMeshPro.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

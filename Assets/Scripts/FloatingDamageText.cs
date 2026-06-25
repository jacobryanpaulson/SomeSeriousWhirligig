using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] TextMeshProUGUI missedText;

    public void Setup(string text)
    {
        textMesh.text = text;
        // Float upwards and fade away using your DOTween package
        transform.DOMoveY(transform.position.y + 1.5f, 1f);
        textMesh.DOFade(0, 1f).OnComplete(() => Destroy(gameObject));
    }

    public void MissedText(string text)
    {
        missedText.text = text;

        missedText.DOFade(0, 1f).OnComplete(() => Destroy(gameObject));
    }
}

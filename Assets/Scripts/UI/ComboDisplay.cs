using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ComboDisplay : MonoBehaviour
{
    private TextMeshProUGUI comboText;
    private CanvasGroup canvasGroup;
    private Tween fadeTween;
    private Tweener shakeTween;
    TweenCallback shakeTweenComplete;
    private Vector3 defaultPosition;

    [SerializeField] private Color regularColor;
    [FormerlySerializedAs("readyColor")] [SerializeField] private Color chargeReadyColor;
    [SerializeField] private Color jumpReadyColor;
    [SerializeField] private Vector2 shakeIntensity;
    [SerializeField] private AnimationCurve shakeCurve;
    [SerializeField] private int maxCombo;

    private void Awake()
    {
        comboText = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();

        comboText.color = regularColor;

        defaultPosition = comboText.rectTransform.localPosition;
    }

    public void ChangeCombo(int num)
    {
        comboText.text = "x" + num;
        
        shakeTween.Kill();
        comboText.rectTransform.localPosition = defaultPosition;

        if (num > 3)
            shakeTween = comboText.transform.DOShakePosition(0.1f,  GetShakeIntensity(num), 10, fadeOut: false).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutQuad);
        

    }
    private float GetShakeIntensity(int num)
    {
        return Mathf.Lerp(shakeIntensity.x, shakeIntensity.y, shakeCurve.Evaluate(((float)num / maxCombo)));
    }

    public void StartCombo()
    {
        fadeTween.Kill();
        fadeTween = canvasGroup.DOFade(1.0f, 0.5f);
    }

    public void LoseCombo()
    { 
        fadeTween.Kill();
        fadeTween = canvasGroup.DOFade(0.0f, 0.5f);
        
        shakeTween.Kill();
        comboText.rectTransform.localPosition = defaultPosition;
        
        shakeTween.Complete();
        ComboUnready();
    }

    public void ChargeComboReady()
    {
        comboText.color = chargeReadyColor;
    }

    public void JumpComboReady()
    {
        comboText.color = jumpReadyColor;
    }

    public void ComboUnready()
    {
        comboText.color = regularColor;
    }
}
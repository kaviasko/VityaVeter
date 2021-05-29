using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1.0f, 0.25f);
    }
}

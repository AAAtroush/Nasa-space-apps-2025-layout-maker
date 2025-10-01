using UnityEngine;

public class ShowAndHide : MonoBehaviour
{
    private bool isShown;

    void Start()
    {
        transform.localPosition = new Vector3(-659, 0, 0); // start hidden
        isShown = false;
    }


    public void ShowOrHide()
    {
        LeanTween.cancel(gameObject);

        Vector3 target = isShown ? new Vector3(-659, 0, 0) : Vector3.zero;

        LeanTween.moveLocal(gameObject, target, 1f)
                 .setEaseOutCubic();

        isShown = !isShown;
    }
}

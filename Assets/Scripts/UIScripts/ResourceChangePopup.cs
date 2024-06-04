using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Popup
{
    public RectTransform transform;
    public Text text;
    public Image icon;

    public void setAlpha(float alpha)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
    }
}

public class ResourceChangePopup : MonoBehaviour
{
    public static ResourceChangePopup instance;

    public Color gainColor;
    public Color lossColor;

    public RectTransform[] popupObjects;
    private Queue<Popup> popupQueue;
    private List<Popup> popupsInUse;
    float popupHalfHeight = 30f;

    public Sprite woodIcon;
    public Sprite metalIcon;

    private Queue<ResourceBlock> resourceQueue;

    private float fadeDuration = 0.7f;

    private float canvasScale = 1;

    void Start()
    {
        instance = this;

        popupQueue = new Queue<Popup>();

        foreach(RectTransform pObj in popupObjects)
        {
            Popup popup = new Popup();
            popup.transform = pObj;
            popup.text = pObj.GetComponent<Text>();
            popup.icon = pObj.GetComponentInChildren<Image>();

            popupQueue.Enqueue(popup);
        }

        resourceQueue = new Queue<ResourceBlock>();

        foreach(Popup popup in popupQueue) { popup.setAlpha(0); }

        popupsInUse = new List<Popup>();
    }

    void Update()
    {
        canvasScale = transform.parent.localScale.x;

        Vector3 screenPoint = PlayerCamera.instance.camera.WorldToScreenPoint(PlayerEntity.instance.transform.position + Vector3.up * 2);
        transform.position = screenPoint;   

        if (resourceQueue.Count == 0) return;
        if (popupQueue.Count == 0) return;

        if (!popupStartSlotCleared()) return;

        ResourceBlock item = resourceQueue.Dequeue();

        if(item.wood != 0)
        {
            Popup popup = popupQueue.Dequeue();
            popupsInUse.Add(popup);

            popup.text.text = (item.wood < 0 ? "" : "+") + item.wood.ToString();
            popup.text.color = (item.wood < 0 ? lossColor : gainColor);
            popup.icon.sprite = woodIcon;

            StartCoroutine(nameof(fadeOutPopup), popup);
        }
        else if (item.metal != 0)
        {
            Popup popup = popupQueue.Dequeue();
            popupsInUse.Add(popup);

            popup.text.text = (item.metal < 0 ? "" : "+") + item.metal.ToString();
            popup.text.color = (item.metal < 0 ? lossColor : gainColor);
            popup.icon.sprite = metalIcon;

            StartCoroutine(nameof(fadeOutPopup), popup);
        }
    }

    public void queueResourceChange(ResourceBlock item) 
    {
        // we split it into wood and metal to make them separate popups back to back
        if(item.wood != 0) resourceQueue.Enqueue(new ResourceBlock(item.wood, 0));
        if(item.metal != 0) resourceQueue.Enqueue(new ResourceBlock(0, item.metal));
    }

    private IEnumerator fadeOutPopup(Popup popup)
    {
        popup.setAlpha(1);

        yield return new WaitForSeconds(0.2f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            popup.setAlpha(alpha);

            popup.transform.localPosition += Vector3.up * popup.transform.rect.height * 3 * Time.deltaTime;

            yield return null;
        }

        popup.setAlpha(0);
        popup.transform.localPosition = Vector3.right * 50;

        popupsInUse.Remove(popup);
        popupQueue.Enqueue(popup);
    }

    /// <summary>
    /// Returns true if there is enough space to start another popup
    /// </summary>
    /// <returns></returns>
    private bool popupStartSlotCleared()
    {
        float lowestLocalY = 100f;
        foreach(Popup popup in popupsInUse) 
        {
            if(popup.transform.localPosition.y < lowestLocalY) lowestLocalY = popup.transform.localPosition.y;
        }

        return lowestLocalY >= popupHalfHeight;
    }
}

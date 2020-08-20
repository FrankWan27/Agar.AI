using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public DetailPanel dp;
    [SerializeField]
    protected RectTransform RawImageRectTrans;
    [SerializeField]
    protected Camera UICamera;
    [SerializeField] 
    protected Camera RenderToTextureCamera;
    public void OnPointerClick(PointerEventData eventData)
    {
        Rect rect = new Rect(RawImageRectTrans.transform.position - new Vector3(400f, 400f), new Vector2(800f, 800f));
        Vector2 normalizedPoint = Rect.PointToNormalized(rect, Input.mousePosition);
        Ray renderRay = RenderToTextureCamera.ViewportPointToRay(normalizedPoint);
        Debug.DrawRay(renderRay.origin, renderRay.direction, Color.green, 60f);
        if (Physics.Raycast(renderRay, out var raycastHit))
        {
            if(raycastHit.collider.gameObject.tag == "Node")
            {
                dp.OpenNodePanel(raycastHit.collider.gameObject.GetComponent<NodeVis>().GetNode());
            }
            else if (raycastHit.collider.gameObject.tag == "Line")
            {
                dp.OpenLinePanel(raycastHit.collider.gameObject.GetComponent<LineVis>().GetLine());
            }
        }
    }

}

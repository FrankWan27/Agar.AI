using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
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
            //On Node Click
            if(raycastHit.collider.gameObject.tag == "Node")
            {
                raycastHit.collider.gameObject.GetComponent<NodeVis>().ShowNodeInfo();
            }
        }
    }

}

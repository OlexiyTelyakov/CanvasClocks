using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CanvasClocks
{
    /// <summary>
    /// Simple helper class that allows the clocks to be dragged on canvas.
    /// </summary>
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        RectTransform rect;
        Vector2 offset;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Get offset between the cursor and the elements position so it is moved relatively to the point it was clicked on.
            offset = (Vector2)transform.position - eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rect.position = eventData.position + offset;
        }
    }
}


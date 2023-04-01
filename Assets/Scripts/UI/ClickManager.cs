using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DonBigo.UI
{
    public static class ClickManager
    {
        public static bool Blocked()
        {
            var cam = Camera.main;
            if (cam == null) return true;

            var pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> hit = new();
            EventSystem.current.RaycastAll(pointer, hit);
            return hit.Count > 0;
            //Debug.Log(hit.ToCommaSeparatedString());
            //return hit.Any(h => h.gameObject.GetComponent<IClickBlocker>() != null);
        }
    }
}
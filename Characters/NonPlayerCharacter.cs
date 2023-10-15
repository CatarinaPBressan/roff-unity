using UnityEngine;

namespace Characters
{
    public class NonPlayerCharacter : MonoBehaviour
    {
        protected Texture2D CursorTexture = null;

        private void OnMouseEnter()
        {
            Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.Auto);
        }

        private void OnMouseExit()
        {
            ClearCursor();
        }

        protected void ClearCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}

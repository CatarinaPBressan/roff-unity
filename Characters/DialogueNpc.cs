using UnityEngine;

namespace Characters
{
    public class DialogueNpc : NonPlayerCharacter
    {
        private void Start()
        {
            CursorTexture = (Texture2D)Resources.Load("Cursors/conversation_alt");
        }
    }
}

using UnityEngine;

public static class Utility {
    public static void LockAndHideCursor() {
        Screen.lockCursor = true;
        Cursor.visible = false;
    }

    public static void UnlockAndShowCursor() {
        Screen.lockCursor = false;
        Cursor.visible = false;
    }
}
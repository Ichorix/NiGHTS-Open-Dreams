using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCursor : MonoBehaviour
{
    void Update()
    {
        // Screw you, your cursor is visible
        Cursor.visible = true;
    }
    void OnDisable()
    {
        Cursor.visible = false;
    }
}

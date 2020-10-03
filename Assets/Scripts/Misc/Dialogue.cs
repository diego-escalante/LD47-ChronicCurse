using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

    public SubDialogue[] subDialogues;

    [System.Serializable]
    public class SubDialogue {
        public bool emphasis;
        public int fontSize;
        public Color color;
        [TextArea(3, 10)]
        public string text;
    }
}

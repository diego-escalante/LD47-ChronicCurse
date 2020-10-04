using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    private Queue<Dialogue.SubDialogue> subDialogues = new Queue<Dialogue.SubDialogue>();
    private Text uiText;
    private Animator animator;
    private Coroutine co;
    private bool isOpen = false;

    private void Start() {
        Transform trans = GameObject.FindWithTag("UI").transform.Find("Dialogue");
        uiText = trans.Find("Text").GetComponent<Text>();
        animator = trans.GetComponent<Animator>();
        animator.SetBool("isOpen", isOpen);

    }

    public void Update() {
        if (isOpen && Input.GetButtonDown("Action")) {
            DisplayNextText();
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        if (animator == null || uiText == null) {
            Transform trans = GameObject.FindWithTag("UI").transform.Find("Dialogue");
            uiText = trans.Find("Text").GetComponent<Text>();
            animator = trans.GetComponent<Animator>();
            animator.SetBool("isOpen", isOpen);
        }

        Time.timeScale = 0f;
        subDialogues.Clear();
        isOpen = true;
        animator.SetBool("isOpen", isOpen);

        foreach (Dialogue.SubDialogue subDialogue in dialogue.subDialogues) {
            subDialogues.Enqueue(subDialogue);
        }
        DisplayNextText();
    }

    public void DisplayNextText() {
        if (subDialogues.Count == 0) {
            EndDialogue();
            return;
        }

        Dialogue.SubDialogue subDialogue = subDialogues.Dequeue();
        uiText.fontSize = subDialogue.fontSize;
        uiText.color = subDialogue.color;
        if (co != null) {
            StopCoroutine(co);
        }
        co = StartCoroutine(ScrollCharacters(subDialogue.text, subDialogue.emphasis));

    }

    private void EndDialogue() {
        Time.timeScale = 1f;
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
    }

    private IEnumerator ScrollCharacters(string text, bool emphasis) {
        string currentText = "";
        foreach (char letter in text.ToCharArray()) {
            currentText += letter;
            if (emphasis) {
                uiText.text = "<i>" + currentText + "</i>";
            } else {
                uiText.text = currentText;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}

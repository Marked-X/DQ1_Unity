using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private GameObject DialogBox;
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private float letterPerSecond = 10f;

    public static DialogManager Instance { get; private set; }
    public event EventHandler OnDialogEnded;
    public event EventHandler OnLineEnded;

    private TextMeshProUGUI textBox;
    private Queue<string> dialog;
    private bool isTyping = false;
    private string lineOfDialog;
    private GameManager.States enteredState;

    private void OnEnable()
    {
        inputReader.nextLineEvent += DisplayNextSentence;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            textBox = DialogBox.GetComponentInChildren<TextMeshProUGUI>();
            dialog = new Queue<string>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        inputReader.nextLineEvent -= DisplayNextSentence;
    }

    public void StartDialog(List<string> dialogArray)
    {
        foreach(string sentence in dialogArray)
        {
            dialog.Enqueue(sentence);
        }

        enteredState = GameManager.Instance.CurrentState;
        GameManager.Instance.ChangeState(GameManager.States.Dialog);

        DialogBox.SetActive(true);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialog.Count == 0 && isTyping == false)
        {
            DialogBox.SetActive(false);
            GameManager.Instance.ChangeState(enteredState);
            OnDialogEnded?.Invoke(this, EventArgs.Empty);
        }
        else if (isTyping == false)
        {
            lineOfDialog = dialog.Dequeue();
            StartCoroutine(TypeLine(lineOfDialog, letterPerSecond));
        }
        else
        {
            StopAllCoroutines();
            textBox.text = lineOfDialog;
            isTyping = false;
        }
    }

    private IEnumerator TypeLine(string dialog, float LPS)
    {
        isTyping = true;

        textBox.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSecondsRealtime(1f / LPS);
        }

        OnLineEnded?.Invoke(this, EventArgs.Empty);
        isTyping = false;
    }

    public void FailedTaskDialog()
    {
        StartDialog(new List<string>() { "But there was nothing." });
    }

    public void ShowLine(string line)
    {
        StopAllCoroutines();
        DialogBox.SetActive(true);
        StartCoroutine(TypeLine(line, 40f));
    }

    public void EndDialog()
    {
        StopAllCoroutines();
        DialogBox.SetActive(false);
    }
}

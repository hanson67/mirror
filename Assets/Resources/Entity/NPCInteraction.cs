using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public enum NPCState
    {
        ReadyToTalk,
        Talking,
        HasTalked,
        Idle
    }

    private NPCState npcState = NPCState.ReadyToTalk;

    public void setNpcState(NPCState n)
    {
        npcState = n;
    }

    public int dialogueId;
    public bool autoInteract;
    // public Text idleDialogueText;
    public string fixedDialogue;
    public float typeSpeed = 0.05f;
    public GameObject guideUI;
    private GameObject uiInstance;

    void Start()
    {
        autoInteract?? StartDialogue(); : return;
    }
    private void Update()
    {
        interAction();
    }
    void interAction()
    {
        Vector2 pos = new Vector2(transform.position.x - 3, transform.position.y + 0.65f);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.right, 6.0f, LayerMask.GetMask("Player"));
        if (!hit)
        {
            if (npcState == NPCState.Idle || npcState == NPCState.Talking)
                npcState = NPCState.HasTalked;
            BubbleChat bubbleChat = gameObject.GetComponent<BubbleChat>();
            bubbleChat.removeBubble();
            Destroy(uiInstance);
            return;
        }
        bubbleInterAction();
        interactionGuide();
        if (npcState == NPCState.ReadyToTalk && Input.GetKeyDown(KeyCode.C) && !autoInteract)
        {
            StartDialogue();
        }
    }
    void bubbleInterAction()
    {
        if (npcState == NPCState.HasTalked)
        {
            if (fixedDialogue != null)
            {
                BubbleChat bubbleChat = gameObject.GetComponent<BubbleChat>();
                bubbleChat.showBubble(fixedDialogue);
                npcState = NPCState.Idle;
            }
        }
    }
    void interactionGuide()
    {
            GameObject canvas = GameObject.Find("DialogueCanvas");
            uiInstance = Instantiate(guideUI, canvas.transform);
            uiInstance.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0, 2f, 0));
    }
    void StartDialogue()
    {
        Debug.Log("대화 시작");
        if (DialogueManager.Instance.StartDialogue(dialogueId))
            npcState = NPCState.Talking;
    }
}
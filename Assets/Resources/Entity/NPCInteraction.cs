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
    
    public NPCState NpcState
    {
        get { return npcState; }
        set { npcState = value; }
    } 
    public int dialogueId;
    public bool autoInteract;
    public float timeoffset;
    // public Text idleDialogueText;
    public string fixedDialogue;
    public float typeSpeed = 0.05f;
    public GameObject guideUI;
    private GameObject uiInstance;

    void Start()
    {
        if (autoInteract == true) StartCoroutine(StartDialogue());
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
            if (NpcState == NPCState.Idle || NpcState == NPCState.Talking)
                NpcState = NPCState.HasTalked;
            BubbleChat bubbleChat = gameObject.GetComponent<BubbleChat>();
            bubbleChat?.removeBubble();
            Destroy(uiInstance);
            return;
        }
        bubbleInterAction();
        //interactionGuide();
        if (NpcState == NPCState.ReadyToTalk && Input.GetKeyDown(KeyCode.C) && !autoInteract)
        {
            StartCoroutine(StartDialogue());
        }
    }
    void bubbleInterAction()
    {
        if (NpcState == NPCState.HasTalked)
        {
            if (fixedDialogue != null)
            {
                BubbleChat bubbleChat = gameObject.GetComponent<BubbleChat>();
                bubbleChat?.showBubble(fixedDialogue);
                NpcState = NPCState.Idle;
            }
        }
    }
    void interactionGuide()
    {
            GameObject canvas = GameObject.Find("DialogueCanvas");
            
            uiInstance = Instantiate(guideUI, canvas.transform);
            uiInstance.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0, 2f, 0));
    }
    IEnumerator StartDialogue()
    {
        Debug.Log("대화 시작");
        GameManager.movable = false;
        yield return new WaitForSeconds(timeoffset);
        GameManager.movable = true;
        if (DialogueManager.Instance.StartDialogue(dialogueId))
            NpcState = NPCState.Talking;
    }
}
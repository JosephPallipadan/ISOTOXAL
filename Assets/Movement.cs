using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour {

    [HideInInspector]
    public bool movementChanging=false;
    private bool moveByTap;
    private bool tempMoveByTap;

    private SpriteRenderer playerSpriteRenderer;
    private PlayerInner player;
    private TextMeshProUGUI movementDisplay;

    
    public Sprite moveByTapForm;
    public Sprite moveByDragForm;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerInner>();
        playerSpriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        movementDisplay = GameObject.Find("Movement Display").GetComponent<TextMeshProUGUI>();
    }

    private Vector3 prevMousePos;

    public bool MoveByTap
    {
        get
        {
            return moveByTap;
        }

        set
        {
            moveByTap = true;
            player.Destination = Vector3.down;
            tempMoveByTap = value;
            movementChanging = true;
            Invoke("doMovementChangeThings", 1);
        }
    }

    void doMovementChangeThings()
    {
        moveByTap = tempMoveByTap;
        displayMovement();
        if (moveByTap)
        {
            playerSpriteRenderer.sprite = moveByTapForm;
        }
        else
        {
            playerSpriteRenderer.sprite = moveByDragForm;
        }
        movementChanging = false;
    }

    private void Update()
    {
        if (MoveByTap && !movementChanging)
        {
            if (GameObject.Find("Player") && Input.GetMouseButtonDown(0))
            {
                Vector3 pos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
                pos.z = 0;
                GameObject.Find("Player").GetComponent<PlayerInner>().Destination = pos;
            }
        }
    }

    private void OnMouseDown()
    {
        prevMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prevMousePos.z = 0;
    }

    private void OnMouseDrag()
    {
        if (!MoveByTap && !movementChanging && GameObject.Find("Player"))
        {
            Vector3 currentPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
            currentPos.z = 0;

            GameObject.Find("Player").transform.position += currentPos - prevMousePos;

            prevMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            prevMousePos.z = 0;
        }
    }

    void displayMovement()
    {
        movementDisplay.text = (MoveByTap ? "Tap" : "Drag");
        movementDisplay.color = new Color(0, 0, 0, 1);
        Invoke("hideMovement", 0.5f);
    }

    void hideMovement()
    {
        movementDisplay.color = new Color(0, 0, 0, 0);
    }
}

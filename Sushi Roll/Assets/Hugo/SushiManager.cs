using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FInputBindings
{
    public EIngredient Ingredient;
    public KeyCode InputKey;
}

public class SushiManager : MonoBehaviour
{
    [Header("Read-only")]
    [SerializeField]
    private List<FTicket> _ticketsList = new List<FTicket>();
    [SerializeField]
    private List<EIngredient> _toInput = new List<EIngredient>();

    [Header("Tickets")]
    [SerializeField]
    private List<UTicketUI> _UITickets = new List<UTicketUI>();

    [Header("Ticket animations")]
    [SerializeField]
    private List<RectTransform> _ticketPoints = new List<RectTransform>();
    [SerializeField]
    private float _ticketMoveSpeed = 0.5f;

    [Header("Game Data")]
    [SerializeField]
    private Data GameData;

    // UI Ticket system junk
    private int _frontOfList = 0;
    private int _backOfList = 0;
    
    // UI Ticket animations
    private float T = 0f;
    private float MovingT = 0f;

    private enum State : int
    {
        ReceivingInput,
        Success,
        Failure,
        AnimatingMovement
    }

    private State _currentState = State.ReceivingInput;

    // Start is called before the first frame update
    void Start()
    {
        _backOfList = _UITickets.Count - 1;
        for(int i = 0; i < GameData.NumberOfVisibleTickets; i++)
        {
            if(_UITickets.Count - 1 >= i)
            {
                _UITickets[i].Initialise(i);
                _UITickets[i].AssignTicket(GenerateTicket());
            }
            else
            {
                // we have an issue
            }
            
        }

        UpdateToInput();

        for(int i = 0; i < GameData.NumberOfIngredients; i++)
        {
            Debug.Log(_ticketsList[0].IngredientList[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(_currentState)
        {
            case State.ReceivingInput:
                CheckInputs();
                break;

            case State.Success:
                Success();
                _currentState = State.AnimatingMovement;
                break;

            case State.Failure:
                Failure();
                _currentState = State.AnimatingMovement;
                break;

            case State.AnimatingMovement:

                if(AnimateTickets())
                {
                    AdvanceTickets();
                    _currentState = State.ReceivingInput;
                }
                break;

        }    
    }

    private void AdvanceTickets()
    {
        // remove old first and replace w/ one at end
        if(_ticketsList.Count != 0)
        {
            _ticketsList.RemoveAt(0);
        }
        UpdateToInput();

        _backOfList += 1;
        _frontOfList += _frontOfList + 1;

        if(_backOfList == _UITickets.Count)
        {
            _backOfList = 0;
        }
        if(_frontOfList == _UITickets.Count)
        {
            _frontOfList = 0;
        }

        _UITickets[_backOfList].AssignTicket(GenerateTicket());
    }

    private bool AnimateTickets()
    {
        T = T + (Time.deltaTime * _ticketMoveSpeed);

        MovingT = MovingT + (Time.deltaTime * _ticketMoveSpeed * 2);

        int successes = 0;

        foreach (UTicketUI ticket in _UITickets)
        {
            RectTransform trans = ticket.gameObject.GetComponent<RectTransform>();

            Vector2 initialPos = new Vector2(0f, 0f);

            if (ticket.IndexInList != _UITickets.Count - 1)
            {
                initialPos = _ticketPoints[ticket.IndexInList + 1].anchoredPosition;

                trans.anchoredPosition = new Vector2(Mathf.SmoothStep(initialPos.x, _ticketPoints[ticket.IndexInList].anchoredPosition.x, T), initialPos.y);

                if (Mathf.Abs(trans.anchoredPosition.x - _ticketPoints[ticket.IndexInList].anchoredPosition.x) <= 1f)
                {
                    successes += 1;

                    trans.anchoredPosition = new Vector2(_ticketPoints[ticket.IndexInList].anchoredPosition.x, trans.anchoredPosition.y);
                }
            }
            else
            {
                if(trans.anchoredPosition.x == _ticketPoints[0].anchoredPosition.x)
                {
                    initialPos = _ticketPoints[0].anchoredPosition;
                    // Still going down
                    trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, Mathf.SmoothStep(initialPos.y, _ticketPoints[0].anchoredPosition.y - 600, MovingT));
                    
                    if(Mathf.Abs( trans.anchoredPosition.y - (_ticketPoints[0].anchoredPosition.y - 600)) <= 1f)
                    {
                        // going up
                        trans.anchoredPosition = new Vector2(_ticketPoints[ticket.IndexInList].anchoredPosition.x, _ticketPoints[ticket.IndexInList].anchoredPosition.y - 600);
                        MovingT = 0f;
                    }
                }
                else if(trans.anchoredPosition.x == _ticketPoints[ticket.IndexInList].anchoredPosition.x)
                {
                    initialPos = _ticketPoints[ticket.IndexInList].anchoredPosition;
                    trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, Mathf.SmoothStep(initialPos.y - 600, initialPos.y, MovingT));

                    if (Mathf.Abs(trans.anchoredPosition.y - _ticketPoints[ticket.IndexInList].anchoredPosition.y) <= 1f)
                    {
                        // success
                        trans.anchoredPosition = _ticketPoints[ticket.IndexInList].anchoredPosition;
                        successes += 1;
                    }
                }
            }


        }

        return successes == _UITickets.Count;
    }

    private void UpdateToInput()
    {
        _toInput.Clear();

        foreach(EIngredient ingredient in _ticketsList[0].IngredientList)
        {
            _toInput.Add(ingredient);
        }
    }

    private FTicket GenerateTicket()
    {
        FTicket NewTicket = new FTicket();

        for(int i = 0; i < GameData.NumberOfIngredients; i++)
        {
            NewTicket.IngredientList.Add((EIngredient)Random.Range(0, 8));
        }

        _ticketsList.Add(NewTicket);
        return _ticketsList[_ticketsList.Count - 1];
    }

    private void CheckInputs()
    {
        foreach(FInputBindings input in GameData.InputBindings)
        {
            if(Input.GetKeyDown(input.InputKey))
            {
                EventManager.m_eventManager.KeyPress( input.Ingredient );

                if (!_toInput.Remove(input.Ingredient))
                {
                    _currentState = State.Failure;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            _currentState = State.Success;
        }

        CheckStatus();
    }

    private void CheckStatus()
    {
        if(_toInput.Count == 0 && _currentState == State.ReceivingInput)
        {
            _currentState = State.Success;
        }
    }

    private void UpdateTicketIndex()
    {
        for (int i = 0; i < _UITickets.Count; i++)
        {
            if (_UITickets[i].IndexInList == 0)
            {
                _UITickets[i].IndexInList = GameData.NumberOfVisibleTickets - 1;
            }
            else
            {
                _UITickets[i].IndexInList -= 1;
            }
        }
        T = 0f;
        MovingT = 0f;
    }

    private void Success()
    {
        // drop on plate
        UpdateTicketIndex();
        Debug.Log("Success");
    }

    private void Failure()
    {
        // consume sushi
        UpdateTicketIndex();
        Debug.Log("Failure");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Fail / Success events")]
    public UnityEvent SuccessEvent;
    public UnityEvent FailEvent;

    [Header("Sushi")]
    [SerializeField]
    private SushiRoll Sushi;

    private Dictionary<EIngredient, Material> _materialMap = new Dictionary<EIngredient, Material>();

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
        AnimatingMovement,
        Idle,
    }

    private State _currentState = State.ReceivingInput;

    void Start()
    {
        _currentState = State.Idle;

        foreach (FIngredientMaterials pair in GameData.IngredientMaterials)
        {
            _materialMap.Add(pair.Ingredient, pair.IngredientMaterial);
        }

        BeginRound();
    }

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
                    _currentState = State.ReceivingInput;
                }
                break;
        }    
    }

    public void BeginRound()
    {
        // External "start" func.

        _ticketsList.Clear();

        _backOfList = _UITickets.Count - 1;
        for(int i = 0; i < GameData.NumberOfVisibleTickets; i++)
        {
            _UITickets[i].Initialise(i);
            _UITickets[i].AssignTicket(GenerateTicket());

            // snap tickets to correct pos
            _UITickets[i].gameObject.GetComponent<RectTransform>().anchoredPosition = _ticketPoints[i].anchoredPosition;
        }

        UpdateToInput();

        _currentState = State.ReceivingInput;
    }

    public void EndRound()
    {
        for (int i = 0; i < GameData.NumberOfVisibleTickets; i++)
        {
            _UITickets[i].Hide();
        }

        _currentState = State.Idle;
    }

    public void NextTicket()
    {
        // public call to advance tickets (switches to animating state)
        UpdateTicketIndex();
        _currentState = State.AnimatingMovement;

        Debug.Log("External ticket progress");
    }

    public void CreateFinishedSushi()
    {
        List<Material> sushiMaterial = new List<Material>
        {
            _materialMap[_ticketsList[0].IngredientList[0]],
            _materialMap[_ticketsList[0].IngredientList[1]],
            _materialMap[_ticketsList[0].IngredientList[2]]
        };

        Sushi.FillSushi(sushiMaterial);
    }

    public void AddIngredient(EIngredient ingredient)
    {
        if(_currentState == State.ReceivingInput)
        {
            // External Add Ingredient call (e.g. used on physical ingredients)
            if (!_toInput.Remove(ingredient))
            {
                _currentState = State.Failure;
            }
        }
    }

    private void AdvanceTickets()
    {
        // Circular list shite
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

                        AdvanceTickets();
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
            NewTicket.IngredientList.Add((EIngredient)Random.Range(0, GameData.NumberOfPossibleIngredients - 1));
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
            // Assumes success when all ingredients are successfully input (no more looked for)
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
        CreateFinishedSushi();
        SuccessEvent.Invoke();
    }

    private void Failure()
    {
        // consume sushi
        UpdateTicketIndex();
        CreateFinishedSushi();
        FailEvent.Invoke();
    }
}

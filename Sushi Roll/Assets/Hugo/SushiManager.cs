using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SushiManager : MonoBehaviour
{
    [Header("Read-only")]
    private List<FTicket> _ticketsList = new List<FTicket>();
    private List<EIngredient> _toInput = new List<EIngredient>();
    private List<EIngredient> _toUse = new List<EIngredient>();

    private List<EIngredient> _hasInput = new List<EIngredient>();

    [Header("Tickets")]
    [SerializeField]
    private List<UTicketUI> _UITickets = new List<UTicketUI>();

    private List<EIngredient> _sushiIngredients = new List<EIngredient>();

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
    [SerializeField]
    private List<IngredientDispenser> _dispensers = new List<IngredientDispenser>();
    [SerializeField]
    private float _fallTime = 0.5f;
    private float _fallTimeCount = 0f;

    [Header("Chameleon")]
    [SerializeField]
    private Animator _chameleonAnimator;

    [SerializeField]
    private float _hackySushiTimeToAppear = 0.75f;
    private float _hackySushiTimer = 0f;
    private bool _hackyWaitingForSushi = false;

    private Dictionary<EIngredient, Material> _materialMap = new Dictionary<EIngredient, Material>();

    // UI Ticket system junk
    private int _frontOfList = 0;
    private int _backOfList = 0;
    
    // UI Ticket animations
    private float T = 0f;
    private float MovingT = 0f;

    public AudioClip m_newTicketSFX;

    public AudioClip m_ticketTearSFX;

    public AudioClip m_chameleonCurlTongue;

    public AudioClip m_sushiRollComplete;

    public AudioClip m_recipeFailed;

    public bool m_failedSushi;

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

        foreach(UTicketUI ticket in _UITickets)
        {
            ticket.Hide();
        }

        _hackyWaitingForSushi = false;


        //BeginRound();
    }

    void Update()
    {
        _hackySushiTimer += Time.deltaTime;

        if (_hackySushiTimer >= _hackySushiTimeToAppear && _hackyWaitingForSushi == true)
        {
            CreateFinishedSushi();
            _hackyWaitingForSushi = false;
        }

        switch (_currentState)
        {
            case State.ReceivingInput:
                CheckInputs();
                break;

            case State.Success:
                _fallTimeCount += Time.deltaTime;
                if(_fallTimeCount >= _fallTime)
                {
                    Success();
                    Progress();
                }
                break;

            case State.Failure:
                _fallTimeCount += Time.deltaTime;
                if (_fallTimeCount >= _fallTime)
                {
                    Failure();
                    Progress();
                }
                    break;

            case State.AnimatingMovement:
                if(AnimateTickets())
                {
                    _currentState = State.ReceivingInput;
                }
                break;

            case State.Idle:

                if (Input.GetKeyDown(KeyCode.B))
                {
                    BeginRound(GameData.ActiveIngredients);
                }

                break;
        }    
    }

    public void BeginRound(List<EIngredient> ingredientsToUse)
    {
        // External "start" func.
        _toUse.Clear();
        _toUse = ingredientsToUse;

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
        if(Sushi != null)
        {
            Sushi.gameObject.SetActive(true);
            List<Material> sushiMaterial = new List<Material>();
            //{
            //    _materialMap[_sushiIngredients[0]],
            //    _materialMap[_sushiIngredients[1]],
            //    _materialMap[_sushiIngredients[2]]
            //};
            int use = 0;
            for(int i = 0; i < 3; i++)
            {
                sushiMaterial.Add(_materialMap[_hasInput[use]]);
                use++;
                if(use == _hasInput.Count)
                {
                    use = 0;
                }
            }

            Sushi.FillSushi(sushiMaterial);

            Sushi.m_success = !m_failedSushi;
        }

        _hasInput.Clear();
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

    public IEnumerator DisableAllIngredients()
    {
        yield return new WaitForSeconds(0.5f);
        
        foreach (IngredientDispenser dispenser in _dispensers)
        {
            dispenser.DisableAll();
        }
    }

    private void AdvanceTickets()
    {

        _sushiIngredients = _ticketsList[0].IngredientList;

        // Circular list shite
        if (_ticketsList.Count != 0)
        {
            _ticketsList.RemoveAt(0);
        }

        UpdateToInput();

        _backOfList += 1;
        _frontOfList += 1;

        if (_backOfList == _UITickets.Count)
        {
            _backOfList = 0;
        }
        if (_frontOfList == _UITickets.Count)
        {
            _frontOfList = 0;
        }

        _UITickets[_backOfList].AssignTicket(GenerateTicket());
    }

    private bool AnimateTickets()
    {
        T += (Time.deltaTime * _ticketMoveSpeed);

        MovingT += (Time.deltaTime * _ticketMoveSpeed * 2);

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
                        _UITickets[_backOfList].TicketAnimator.SetTrigger("ResetTicket");
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
            NewTicket.IngredientList.Add(_toUse[Random.Range(0, _toUse.Count)]);
        }

        _ticketsList.Add(NewTicket);
        EventManager.m_eventManager.SFXPlay( m_newTicketSFX );

        return _ticketsList[_ticketsList.Count - 1];

    }

    private void CheckInputs()
    {
        foreach(FInputBindings input in GameData.InputBindings)
        {
            if(Input.GetKeyDown(input.InputKey))
            {
                EventManager.m_eventManager.KeyPress( input.Ingredient );

                _hasInput.Add(input.Ingredient);

                if (!_toInput.Remove(input.Ingredient))
                {
                    BeginFail();
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
            BeginSuccess();
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

    private void BeginSuccess()
    {
        _fallTimeCount = 0f;
        _currentState = State.Success;

        EventManager.m_eventManager.SFXPlay( m_ticketTearSFX );
        _UITickets[_frontOfList].TicketAnimator.SetTrigger("TearTicket");

        _hackySushiTimer = 0f;
        _hackyWaitingForSushi = true;

        m_failedSushi = false;
    }

    private void BeginFail()
    {
        _fallTimeCount = 0f;
        _currentState = State.Failure;

        _UITickets[_frontOfList].TicketAnimator.SetTrigger("TearTicket");

        _hackySushiTimer = 0f;
        _hackyWaitingForSushi = true;

        m_failedSushi = true;
    }

    private void Success()
    {
        // drop on plate
        SuccessEvent.Invoke();

        EventManager.m_eventManager.SFXPlay( m_sushiRollComplete );

        EventManager.m_eventManager.RecipeSuccess( );

        Debug.LogError("Success");
    }

    private void Failure()
    {
        // consume sushi
        FailEvent.Invoke();

        EventManager.m_eventManager.SFXPlay( m_recipeFailed );

        Debug.LogError("Failed");
    }

    private void Progress()
    {
        UpdateTicketIndex();
        _chameleonAnimator.SetTrigger("CurlSushi");
        EventManager.m_eventManager.SFXPlay( m_chameleonCurlTongue );

        StartCoroutine("DisableAllIngredients");

        _currentState = State.AnimatingMovement;
    }
}

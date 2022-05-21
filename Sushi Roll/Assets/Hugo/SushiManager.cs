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
    [SerializeField]
    private List<FTicket> _ticketsList = new List<FTicket>();
    [SerializeField]
    private List<EIngredient> _toInput = new List<EIngredient>();
    [SerializeField]
    private int NumberOfIngredients = 3;

    [SerializeField]
    private List<FInputBindings> InputBindings = new List<FInputBindings>();

    private enum State : int
    {
        ReceivingInput,
        Success,
        Failure
    }

    private State _currentState = State.ReceivingInput;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            GenerateTicket();
        }

        UpdateToInput();

        for(int i = 0; i < NumberOfIngredients; i++)
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
                _currentState = State.ReceivingInput;
                break;

            case State.Failure:
                Failure();
                _currentState = State.ReceivingInput;
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
        GenerateTicket();

        UpdateToInput();
    }

    private void UpdateToInput()
    {
        _toInput.Clear();

        foreach(EIngredient ingredient in _ticketsList[0].IngredientList)
        {
            _toInput.Add(ingredient);
        }
    }

    private void GenerateTicket()
    {
        FTicket NewTicket = new FTicket();
        for(int i = 0; i < NumberOfIngredients; i++)
        {
            NewTicket.IngredientList[i] = (EIngredient)Random.Range(0, 8);
        }

        _ticketsList.Add(NewTicket);
    }

    private void CheckInputs()
    {
        foreach(FInputBindings input in InputBindings)
        {
            if(Input.GetKeyDown(input.InputKey))
            {
                if(!_toInput.Remove(input.Ingredient))
                {
                    _currentState = State.Failure;
                }
            }
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

    private void Success()
    {
        // drop on plate
        _currentState = State.Success;

        AdvanceTickets();
        Debug.Log("Success");
    }

    private void Failure()
    {
        // consume sushi
        _currentState = State.Failure;
        AdvanceTickets();
        Debug.Log("Failure");
    }
}

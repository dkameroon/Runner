using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class StartupAuthFlowSystem : IInitializable
{
    private readonly AuthFlowService _authFlowService;
    private readonly IAuthenticationService _authenticationService;

    private bool _isInitializationRunning;
    private bool _isInitialized;

    public StartupAuthFlowSystem(
        AuthFlowService authFlowService,
        IAuthenticationService authenticationService)
    {
        _authFlowService = authFlowService;
        _authenticationService = authenticationService;
    }

    public void Initialize()
    {
        StartInitialization();
    }

    private void StartInitialization()
    {
        if (_isInitializationRunning || _isInitialized)
        {
            return;
        }

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        if (_isInitializationRunning || _isInitialized)
        {
            return;
        }

        _isInitializationRunning = true;

        try
        {
            await _authenticationService.InitializeAsync();

            if (_authenticationService.IsAuthorized)
            {
                _authFlowService.ShowMainMenu();
            }
            else
            {
                _authFlowService.ShowAuthScreen();
            }

            _isInitialized = true;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            _authFlowService.ShowAuthScreen();
        }
        finally
        {
            _isInitializationRunning = false;
        }
    }
}
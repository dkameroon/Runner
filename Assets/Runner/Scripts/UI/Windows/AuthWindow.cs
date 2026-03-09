using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthWindow : BaseWindow
{
    [Header("Texts")]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _submitButtonText;
    [SerializeField] private TMP_Text _switchModeButtonText;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _loginInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _confirmPasswordInputField;

    [Header("Field Roots")]
    [SerializeField] private GameObject _loginFieldRoot;
    [SerializeField] private GameObject _confirmPasswordFieldRoot;

    [Header("Buttons")]
    [SerializeField] private Button _submitButton;
    [SerializeField] private Button _switchModeButton;

    private EAuthWindowMode _currentMode;

    public event Action<string, string> SignInRequested;
    public event Action<string, string, string, string> SignUpRequested;
    public event Action<EAuthWindowMode> ModeChanged;

    protected override void Awake()
    {
        base.Awake();

        _submitButton.onClick.AddListener(OnSubmitButtonClicked);
        _switchModeButton.onClick.AddListener(OnSwitchModeButtonClicked);

        SetMode(EAuthWindowMode.SignIn);
        SetError(string.Empty);
    }

    private void OnDestroy()
    {
        _submitButton.onClick.RemoveListener(OnSubmitButtonClicked);
        _switchModeButton.onClick.RemoveListener(OnSwitchModeButtonClicked);
    }

    public override void Show()
    {
        base.Show();
        ClearFields();
        SetError(string.Empty);
    }

    public void SetError(string message)
    {
        _errorText.text = message;
        _errorText.gameObject.SetActive(string.IsNullOrWhiteSpace(message) == false);
    }

    public void SetMode(EAuthWindowMode mode)
    {
        _currentMode = mode;

        bool isSignUpMode = _currentMode == EAuthWindowMode.SignUp;

        _titleText.text = isSignUpMode ? "Sign Up" : "Sign In";
        _submitButtonText.text = isSignUpMode ? "Register" : "Login";
        _switchModeButtonText.text = isSignUpMode ? "Already Have Account" : "Create Account";

        _loginFieldRoot.SetActive(isSignUpMode);
        _confirmPasswordFieldRoot.SetActive(isSignUpMode);

        SetError(string.Empty);

        ModeChanged?.Invoke(_currentMode);
    }

    public void ClearFields()
    {
        _emailInputField.text = string.Empty;
        _loginInputField.text = string.Empty;
        _passwordInputField.text = string.Empty;
        _confirmPasswordInputField.text = string.Empty;
    }

    private void OnSubmitButtonClicked()
    {
        SetError(string.Empty);

        string email = _emailInputField.text;
        string login = _loginInputField.text;
        string password = _passwordInputField.text;
        string confirmPassword = _confirmPasswordInputField.text;

        if (_currentMode == EAuthWindowMode.SignIn)
        {
            SignInRequested?.Invoke(email, password);
            return;
        }

        SignUpRequested?.Invoke(email, login, password, confirmPassword);
    }

    private void OnSwitchModeButtonClicked()
    {
        EAuthWindowMode nextMode = _currentMode == EAuthWindowMode.SignIn
            ? EAuthWindowMode.SignUp
            : EAuthWindowMode.SignIn;

        SetMode(nextMode);
    }
}
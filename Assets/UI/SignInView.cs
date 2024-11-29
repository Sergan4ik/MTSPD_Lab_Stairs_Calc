using System;
using StairsCalc;
using TMPro;
using UnityEngine;

public class SignInView : MonoBehaviour
{
    public GameObject signInPanel, signUpPanel, adminPanel;
    
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public UnityEngine.UI.Button signInButton, toSignUp, toAdmin;
    
    public TMP_InputField signUpUsernameInput;
    public TMP_InputField signUpEmailInput;
    public TMP_InputField signUpPasswordInput;
    
    public UnityEngine.UI.Button signUpButton, toSignIn;
    
    public TMP_InputField adminUsernameInput;
    public TMP_InputField adminPasswordInput;
    
    public UnityEngine.UI.Button adminButton, toSignInAdmin;
    
    public CalcView calcView;
    
    private AuthProvider _authProvider;

    private void Start()
    {
        Show();
    }

    public void Show()
    {
        signInPanel.SetActive(true);
        signUpPanel.SetActive(false);
        adminPanel.SetActive(false);
        
        _authProvider = new AuthProvider(Resources.Load<UsersDB>("Users"));
        
        usernameInput.text = "";
        passwordInput.text = "";
        
        signUpUsernameInput.text = "";
        signUpEmailInput.text = "";
        signUpPasswordInput.text = "";
        
        adminUsernameInput.text = "";
        adminPasswordInput.text = "";
        
        signInButton.onClick.RemoveAllListeners();
        signInButton.onClick.AddListener(() =>
        {
            if (_authProvider.Login(usernameInput.text, passwordInput.text))
            {
                calcView.Show();
                gameObject.SetActive(false);
            }
        });
        
        toSignUp.onClick.RemoveAllListeners();
        toSignUp.onClick.AddListener(() =>
        {
            signInPanel.SetActive(false);
            signUpPanel.SetActive(true);
            adminPanel.SetActive(false);
        });
        
        toAdmin.onClick.RemoveAllListeners();
        toAdmin.onClick.AddListener(() =>
        {
            signInPanel.SetActive(false);
            signUpPanel.SetActive(false);
            adminPanel.SetActive(true);
        });
        
        signUpButton.onClick.RemoveAllListeners();
        signUpButton.onClick.AddListener(() =>
        {
            if (_authProvider.Register(signUpEmailInput.text, signUpPasswordInput.text))
            {
                calcView.Show();
                gameObject.SetActive(false);
            }
        });
        
        toSignIn.onClick.RemoveAllListeners();
        toSignIn.onClick.AddListener(() =>
        {
            signInPanel.SetActive(true);
            signUpPanel.SetActive(false);
            adminPanel.SetActive(false);
        });
        
        adminButton.onClick.RemoveAllListeners();
        adminButton.onClick.AddListener(() =>
        {
            if (_authProvider.LoginAdmin(adminUsernameInput.text, adminPasswordInput.text))
            {
                FindObjectOfType<MaterialEditView>(true).Show(Resources.Load<MaterialDB>("Materials"));
                // calcView.Show()Show;
                gameObject.SetActive(false);
            }
        });
        
        toSignInAdmin.onClick.RemoveAllListeners();
        toSignInAdmin.onClick.AddListener(() =>
        {
            signInPanel.SetActive(true);
            signUpPanel.SetActive(false);
            adminPanel.SetActive(false);
        });
        
        gameObject.SetActive(true);
    }
}
using System;
using MirageSDK.Utils;
using MirageSDK.WalletConnectSharp.Core;
using MirageSDK.WalletConnectSharp.Core.StatusEvents;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace MirageSDK.UI
{
    public class WalletManager : MonoBehaviour
    {
        public GameObject LoadingView, GameView;
        public Button ConnectButton, DisconnectButton;
        public TMP_Text AddressText;
        [SerializeField] private ChooseWalletScreen _chooseWalletScreen;
        [SerializeField] private MirageSDK.Utils.UI.QRCodeImage _qrCodeImage;
        private WalletConnectSharp.Unity.WalletConnect WalletConnect => ConnectProvider<WalletConnectSharp.Unity.WalletConnect>.GetConnect();
        private async void Start()
        {
            await WalletConnect.Connect();
        }

        private void OnEnable()
        {
            ConnectButton.onClick.AddListener(GetLoginAction());
            ConnectButton.gameObject.SetActive(false);

            DisconnectButton.onClick.AddListener(OnDisconnectClick);

            SubscribeToWalletEvents();
            UpdateLoginButtonState();
        }

        private UnityAction GetLoginAction()
        {
            if (!Application.isEditor)
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                        return () => _chooseWalletScreen.Activate(WalletConnect.OpenDeepLink);
                    case RuntimePlatform.Android:
                        return WalletConnect.OpenDeepLink;
                }
            }

            return () =>
            {
                _qrCodeImage.UpdateQRCode(WalletConnect.ConnectURL);
                _qrCodeImage.SetImageActive(true);
            };
        }

        public string GetAddress()
        {
            return WalletConnect.GetDefaultAccount().ToString();
        }

        private void SubscribeToWalletEvents()
        {
            WalletConnect.SessionStatusUpdated += SessionStatusUpdated;
        }

        private void UnsubscribeFromWalletEvents()
        {
            WalletConnect.SessionStatusUpdated -= SessionStatusUpdated;
        }

        private void OnDisable()
        {
            UnsubscribeFromWalletEvents();
        }

        private void SessionStatusUpdated(WalletConnectTransitionBase walletConnectTransition)
        {
            UpdateLoginButtonState();
        }

        private void UpdateLoginButtonState()
        {
            var status = WalletConnect.Status;

            if (status == WalletConnectStatus.Uninitialized)
            {
                return;
            }

            var walletConnected = status == WalletConnectStatus.WalletConnected;
            LoadingView.SetActive(!walletConnected);
            GameView.SetActive(walletConnected);

            _chooseWalletScreen.SetActive(!walletConnected);

            bool waitingForLoginInput = status == WalletConnectStatus.SessionRequestSent;

            ConnectButton.gameObject.SetActive(waitingForLoginInput);

            _qrCodeImage.SetImageActive(false);

            if (!waitingForLoginInput)
            {
                switch (status)
                {
                    case WalletConnectStatus.DisconnectedNoSession:
                    case WalletConnectStatus.DisconnectedSessionCached:
                        {
                            Debug.Log("Disconnected");
                            break;
                        }
                    case WalletConnectStatus.TransportConnected:
                        {
                            Debug.Log("Transport Connected");
                            break;
                        }
                    case WalletConnectStatus.WalletConnected:
                        {
                            Debug.Log("Connected");
                            AddressText.text = WalletConnect.GetDefaultAccount().ToString();
                            break;
                        }
                }
            }
        }

        private void OnDisconnectClick()
        {
            WalletConnect.CloseSession().Forget();
        }
    }
}
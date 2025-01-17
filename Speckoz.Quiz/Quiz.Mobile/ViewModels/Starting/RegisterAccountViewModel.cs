﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Logikoz.XamarinUtilities.Utilities;

using Quiz.Mobile.Services.Requests;
using Quiz.Mobile.Views.Starting;

using RestSharp;

using System.Net;
using System.Threading.Tasks;

using Xamarin.Forms;

using XF.Material.Forms.UI.Dialogs;

namespace Quiz.Mobile.ViewModels.Starting
{
    public class RegisterAccountViewModel : ViewModelBase
    {
        private string __username;
        private string __newPassword;
        private string __confirmNewPassword;
        private string __email;

        public string Username
        {
            get => __username;
            set => Set(ref __username, value);
        }

        public string Email
        {
            get => __email;
            set => Set(ref __email, value);
        }

        public string NewPassword
        {
            get => __newPassword;
            set => Set(ref __newPassword, value);
        }

        public string ConfirmNewPassword
        {
            get => __confirmNewPassword;
            set => Set(ref __confirmNewPassword, value);
        }

        public RelayCommand RegisterCommand { get; private set; }
        public RelayCommand BackCommand { get; private set; }

        public RegisterAccountViewModel() => Init();

        private void Init()
        {
            RegisterCommand = new RelayCommand(Register);
            BackCommand = new RelayCommand(async () =>
            {
                if (!string.IsNullOrEmpty(Email) || !string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(NewPassword))
                {
                    if ((await MaterialDialog.Instance.ConfirmAsync("Realmente deseja sair dessa tela?\nTodos os dados nao salvos seram perdidos!", "ATENÇAO", "Sim", "Cancelar")) == false)
                        return;
                }

                PopPushViewUtil.PopModalAsync<RegisterAccountView>(true);
            });
        }

        private async void Register()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(NewPassword))
            {
                ConfirmNewPassword = await MaterialDialog.Instance.InputAsync("Cadastro", "Por favor, confirme sua senha!", string.Empty, "Senha", "Confirmar", "Esqueci");

                if (string.IsNullOrEmpty(ConfirmNewPassword))
                    return;

                if (NewPassword != ConfirmNewPassword)
                {
                    await MaterialDialog.Instance.AlertAsync("As senhas nao coincidem!", "Ops!", "OK");
                    return;
                }

                using (IMaterialModalPage dialog = await MaterialDialog.Instance.LoadingDialogAsync("Processando cadastro..."))
                {
                    IRestResponse response = await AccountService.RegisterAccountTaskAsync(Username, Email, NewPassword);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        dialog.MessageText = "Cadastro efetuado com sucesso!";
                        await Task.Delay(1000);

                        Application.Current.MainPage = new AuthAccountView();
                    }
                    else
                    {
                        await dialog.DismissAsync();
                        await MaterialDialog.Instance.AlertAsync("Nao foi possivel efetuar o cadastro!\nVerifique sua conexao e tente novamente.", "Algo deu errado!", "OK");
                    }
                }
            }
            else
                await MaterialDialog.Instance.AlertAsync("Voce precisa preencher os campos!\n", "Oh nao!", "OK");
        }
    }
}
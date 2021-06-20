using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers;
using BlazorHero.CleanArchitecture.Shared.Constants.Tenant;

namespace BlazorHero.CleanArchitecture.Client.Pages.Authentication
{
    public partial class Login
    {
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private TokenRequest _tokenModel = new();
        [Inject] private ITenantManager TenantManager { get; set; }
        private string _tenantIdentifier;
        private bool _isTenantMaster = false;
        private bool _visible = false;
        private string _tenantIdentifierChange;
        private bool _rememberTenant = true;
        protected override async Task OnInitializedAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                _navigationManager.NavigateTo("/");
            }
            //check curent tenant in server;
            _tenantIdentifier = await TenantManager.GetCurrentTenantSever();
            _isTenantMaster = _tenantIdentifier == TenantConstants.DefaultTenantId;
            // if current tenant is Master
            // show button change tenant.
            if(_isTenantMaster)
            {
                string tenantlocal = await TenantManager.GetCurrentTenantLocal();
                _tenantIdentifier = string.IsNullOrEmpty(tenantlocal) ? _tenantIdentifier : tenantlocal;
            }    
        }
        private async Task SubmitAsync()
        {
            var result = await _authenticationManager.Login(_tokenModel,_tenantIdentifier);
            if (result.Succeeded)
            {
                _snackBar.Add(string.Format(_localizer["Welcome {0}"], _tokenModel.Email), Severity.Success);
                _navigationManager.NavigateTo("/", true);
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if(_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        private void FillAdministratorCredentials()
        {
            _tokenModel.Email = "mukesh@blazorhero.com";
            _tokenModel.Password = "123Pa$$word!";
        }

        private void FillBasicUserCredentials()
        {
            _tokenModel.Email = "john@blazorhero.com";
            _tokenModel.Password = "123Pa$$word!";
        }

        private async Task OpenChangeTenantModal()
        {
            _visible = true;
        }
        private async Task CloseChangeTenantModal()
        {
            _visible = false;
        }

        private async Task ChangeTenant()
        {
            if(!string.IsNullOrEmpty( _tenantIdentifierChange))
            {
                _tenantIdentifier = _tenantIdentifierChange;
                _visible = false;
               await TenantManager.RememberCurrentTenant(_tenantIdentifier, _rememberTenant);
            }    
            else
            {
                _snackBar.Add(_localizer["Tenant Identifier is not empty!"], Severity.Error);
            }    
        }
    }
}
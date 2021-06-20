using Blazored.FluentValidation;
using BlazorHero.CleanArchitecture.Application.Requests.Tenant;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Tenant
{

    public partial class AddEditTenantModal
    {
        [Inject] private ITenantManager TenantManager { get; set; }
        [Parameter] public UpdateTenantRequest Tenant { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
       
        private async Task SaveAsync()
        {
            var response = await TenantManager.AddUpdateAsync(Tenant);
            if (response.Succeeded)
            {
                _snackBar.Add(_localizer["Save Success!"], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}

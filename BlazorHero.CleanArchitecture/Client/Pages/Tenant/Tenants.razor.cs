using BlazorHero.CleanArchitecture.Application.Requests.Tenant;
using BlazorHero.CleanArchitecture.Application.Responses.Tenant;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Tenant
{
    public partial class Tenants
    {
        [Inject] private ITenantManager _tenantManager { get; set; }
        private List<TenantResponse> _listTenant = new();
        private TenantResponse _tenant = new();
        private MudTable<TenantResponse> _table;
        private int _totalItems;
        private int _currentPage;

        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreatePermisstion;
        private bool _canEditPermisstion;
        private bool _canDeletePermisstion;
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePermisstion = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Tenants.Create)).Succeeded;
            _canEditPermisstion = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Tenants.Edit)).Succeeded;
            _canDeletePermisstion = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Tenants.Delete)).Succeeded;
        }

        private async Task<TableData<TenantResponse>> ServerReload(TableState state)
        {
            await LoadData(state.Page, state.PageSize);
            return new TableData<TenantResponse> { TotalItems = _totalItems, Items = _listTenant };
        }
        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task LoadData(int pageNumber, int pageSize )
        {
            var request = new GetAllPagedTenantsRequest { PageSize = pageSize, PageNumber = pageNumber + 1,SearchString = _searchString };
            var response = await _tenantManager.GetAll(request);
            if (response.Succeeded)
            {
                _totalItems = response.Data.TotalCount;
                _currentPage = response.Data.CurrentPage;
                _listTenant = response.Data.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        
        private bool Search(TenantResponse role)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (role.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (role.Identifier?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private async Task InvokeModal(string id = null)
        {
            var parameters = new DialogParameters();
            if (id != null)
            {
                _tenant = _listTenant.FirstOrDefault(c => c.Id == id);
                if (_tenant != null)
                {
                    parameters.Add(nameof(AddEditTenantModal.Tenant), new UpdateTenantRequest
                    {
                        Id = _tenant.Id,
                        Name = _tenant.Name,
                        ConnectionString = _tenant.ConnectionString,
                        // DatabaseType = _tenant.DatabaseType,
                        Identifier = _tenant.Identifier,
                        IsActive = _tenant.IsActive,
                        TenantLogo = _tenant.TenantLogo
                    }) ;
                }
            }
            else
            {
                parameters.Add(nameof(AddEditTenantModal.Tenant), new UpdateTenantRequest()) ;
            }    
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditTenantModal>(id == null ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch(_searchString) ;
            }
        }

        private async Task Delete(string id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _tenantManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    _snackBar.Add(_localizer["Delete Success"], Severity.Success);
                }
                else
                {
                    OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
    }
}

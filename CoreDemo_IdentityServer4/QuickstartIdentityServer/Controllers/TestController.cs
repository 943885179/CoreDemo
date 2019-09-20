using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuickstartIdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async Task<object> GetAsync()
        {
            return "123";
        }
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;


        public TestController(IClientStore clientStore,
            IResourceStore resourceStore,
            IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        public async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl)
        {
            //根据return url 拿到ClientId 等信息
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (returnUrl == null)
                return null;
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);//根据请求的scope 拿到resources


            return CreateConsentViewModel(request, client, resources);
        }

        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request, Client client, Resources resources)
        {
            var vm = new ConsentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLoggoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.RemeberConsent = client.AllowRememberConsent;

            vm.IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i));
            //api resource
            vm.ResourceScopes = resources.ApiResources.SelectMany(i => i.Scopes).Select(x => CreateScopeViewModel(scope: x));
            return vm;
        }
        //identity 1个scopes
        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource)
        {
            return new ScopeViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Required = identityResource.Required,
                Checked = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }
        //apiresource
        private ScopeViewModel CreateScopeViewModel(Scope scope)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Required = scope.Required,
                Checked = scope.Required,
                Emphasize = scope.Emphasize
            };
        }
    }
    public class InputConsentViewModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }

        public bool RemeberConsent { get; set; }
        public string ReturnUrl { get; set; }
    }
    //解析跳转url后得到的应用权限等信息
    public class ConsentViewModel : InputConsentViewModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
        public string ClientLoggoUrl { get; internal set; }
    }
    //接收Scope
    public class ScopeViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
    }
    public class ProcessConsentResult
    {
        public string RedirectUrl { get; set; }
        public bool IsRedirectUrl => RedirectUrl != null;
        public string ValidationError { get; set; }
        public ConsentViewModel ViewModel { get; set; }
    }
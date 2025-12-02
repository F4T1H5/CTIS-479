using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace CORE.APP.Services.Session.MVC
{
    public abstract class SessionServiceBase
    {
        protected IHttpContextAccessor HttpContextAccessor { get; }

        protected SessionServiceBase(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public virtual T GetSession<T>(string key) where T : class
        {
            var value = HttpContextAccessor.HttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(value))
                return null;
            return JsonSerializer.Deserialize<T>(value);
        }

        public virtual void SetSession<T>(string key, T instance) where T : class
        {
            if (instance is not null)
            {
                var value = JsonSerializer.Serialize(instance);
                HttpContextAccessor.HttpContext.Session.SetString(key, value);
            }
        }

        public virtual void RemoveSession(string key)
        {
            HttpContextAccessor.HttpContext.Session.Remove(key);
        }
    }
}
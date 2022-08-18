using WebApiAutors.Services;

namespace WebApiAutors.Services
{
    public interface IService
    {
        void DoTask(); //asignature 
    }

    public class ServiceA : IService
    {
        private readonly ILogger<ServiceA> logger;

        public ServiceA(ILogger<ServiceA> logger)
        {
            this.logger = logger;
        }

        public void DoTask()
        {
            
        }
    }

    public class ServiceB : IService
    {
        public void DoTask()
        {
            
        }
    }

    public class ServiceTransient
    {
        public Guid Guid = Guid.NewGuid();
    }
    public class ServiceScoped
    {
        public Guid Guid = Guid.NewGuid();
    }
    public class ServiceSingleton
    {
        public Guid Guid = Guid.NewGuid();
    }
}

using System.Runtime.Serialization;

namespace CoreDemo_Consul
{
    public class ConsulOption
    {
        public string Address { get; internal set; }
        public string ServiceName { get; internal set; }
        public string ServiceIP { get; internal set; }
        public int ServicePort { get; internal set; }
        public string ServiceHealthCheck { get; internal set; }
    }
}
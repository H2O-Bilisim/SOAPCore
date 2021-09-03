using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Efatura.Interfaces
{
    public interface IEfaturaChannel : IEfatura, IClientChannel
    {
    }
}

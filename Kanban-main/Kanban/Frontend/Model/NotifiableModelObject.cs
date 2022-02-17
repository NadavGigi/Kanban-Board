using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class NotifiableModelObject : NotifiableObject
    {
        public BackendController bc { get; private set; }
        public NotifiableModelObject(BackendController Bc)
        {
            this.bc = Bc;
        }
    }
}

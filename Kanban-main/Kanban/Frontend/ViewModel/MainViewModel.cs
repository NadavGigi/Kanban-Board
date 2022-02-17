using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class MainViewModel : NotifiableObject
    {
        public BackendController bc { get; private set; }
        public MainViewModel()
        {
            bc = new BackendController();
            this.bc = bc;
        }
    }
}

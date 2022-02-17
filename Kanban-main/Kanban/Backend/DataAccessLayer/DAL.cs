using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class DAL
    {
        protected DALController _controller;
        protected DAL(DALController controller)
        {
            _controller = controller;
        }
    }
}
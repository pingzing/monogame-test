using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Core.Components
{
    interface IDialogComponent : IComponent
    {
        Task LoadAsync();
        void ProgressDialogue();
    }
}

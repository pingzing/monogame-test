using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Core.DialogueSystem
{
    public class Dialogue
    {
        public Guid OwnerEntityId { get; set; }
        public List<string> DialogueEntries { get; set; }
    }
}

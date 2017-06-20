using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;
using monogame_test.Core.DialogueSystem;

namespace monogame_test.Core.Components.TestNpc
{
    public class TestNpcDialogComponent : IDialogComponent
    {
        private DialogueManager _dialogueManager;
        private Dialogue _testNpcDialogue = new Dialogue
        {
            DialogueEntries = new List<string>
            {
                "Entry one!",
                "Entry two!",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse nec leo id quam ultricies feugiat facilisis et ipsum. Nullam vehicula tempus ex. Integer consectetur consectetur dui at congue. Nam elit nisl, gravida id sagittis eget, laoreet vitae dui. Ut malesuada luctus ornare. Aliquam elit erat, vulputate sed luctus sit amet, dictum iaculis augue. Praesent ac ex condimentum, ultricies justo vehicula, bibendum arcu. "

            }
        };

        public TestNpcDialogComponent(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
        }

        public Task LoadAsync()
        {
            return Task.FromResult(false);
        }

        public void ProgressDialogue()
        {
            _dialogueManager.DisplayDialogue(_testNpcDialogue);
        }

        public void Update(GameTime deltaTime, Entity entity)
        {
            if (_testNpcDialogue.OwnerEntityId == Guid.Empty)
            {
                _testNpcDialogue.OwnerEntityId = entity.Id;
            }
        }
    }
}

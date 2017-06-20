using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Core.DialogueSystem
{
    public class DialogueManager
    {
        public static bool IsDialogueBoxOpen => _currentState != DialogueBoxState.Closed;

        private enum DialogueBoxState
        {
            Closed,
            Opening,
            Open,
            Closing
        }

        private SpriteBatch _spriteBatch;
        private SpriteFont _defaultFont;        
        private static DialogueBoxState _currentState;
        private Queue<Dialogue> _dialogueQueue;
        private Dialogue _currentDialogue;

        private int _dialogIndexPreviousFrame = -1;
        private int _currentDialogueIndex = -1;

        private StringBuilder _currentTextInBox = null;

        public DialogueManager(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            _dialogueQueue = new Queue<Dialogue>();
            _spriteBatch = spriteBatch;
            _defaultFont = defaultFont;            
        }

        public void DisplayDialogue(Dialogue dialogue)
        {
            _dialogueQueue.Enqueue(dialogue);
        }

        public void Update(GameTime gameTime)
        {
            switch (_currentState)
            {
                case DialogueBoxState.Closed:
                    if (_dialogueQueue.Count > 0)
                    {
                        _currentDialogue = _dialogueQueue.Dequeue();
                        _currentState = DialogueBoxState.Opening;
                    }
                    break;

                case DialogueBoxState.Opening:
                    // todo animation stuff, just jump right to open for now
                    _currentState = DialogueBoxState.Open;
                    _currentDialogueIndex = 0;
                    break;

                case DialogueBoxState.Open:
                    if (_currentDialogueIndex != _dialogIndexPreviousFrame
                        && _currentDialogue.DialogueEntries.Count > _currentDialogueIndex)
                    {
                        _currentTextInBox = WordWrapper.WrapWord(
                            new StringBuilder(_currentDialogue.DialogueEntries[_currentDialogueIndex]),
                            _defaultFont,
                            new Rectangle(0, 450, 800, 150),
                            1f);

                        _dialogIndexPreviousFrame = _currentDialogueIndex;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Z))
                    { 
                        if (_currentDialogueIndex + 1 < _currentDialogue.DialogueEntries.Count)
                        {
                            _currentDialogueIndex++;
                        }
                        else
                        {
                            _currentState = DialogueBoxState.Closing;
                        }
                    }

                    break;

                case DialogueBoxState.Closing:
                    //todo animation stuff, just jump right to closed for now
                    _currentState = DialogueBoxState.Closed;
                    break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (_currentState == DialogueBoxState.Open && _currentTextInBox != null)
            {
                _spriteBatch.DrawString(_defaultFont,
                    _currentTextInBox,
                    new Vector2(0, 300),
                    Color.White,
                    0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            }
        }
    }
}

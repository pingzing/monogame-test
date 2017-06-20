using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Core.Components.Terra.States
{
    public static class TerraStates
    {
        public static JumpingLeft JumpingLeft = new JumpingLeft();
        public static JumpingRight JumpingRight = new JumpingRight();

        public static StandingUp StandingUp = new StandingUp();
        public static StandingRight StandingRight = new StandingRight();
        public static StandingDown StandingDown = new StandingDown();
        public static StandingLeft StandingLeft = new StandingLeft();
                
        public static WalkingRight WalkingRight = new WalkingRight();        
        public static WalkingLeft WalkingLeft = new WalkingLeft();
    }
}

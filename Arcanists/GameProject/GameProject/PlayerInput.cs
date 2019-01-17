using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GameProject
{
    class PlayerInput
    {
        GamePadState old, curr;
        PlayerIndex player;

        public PlayerInput(PlayerIndex play)
        {
            player = play;
        }

        public bool IsDown(Buttons button)
        {
            if (curr.IsButtonDown(button))
                return true;
            else
                return false;

        }
        public bool WasPressedBack(Buttons button)
        {
            if (old.IsButtonUp(button) && curr.IsButtonDown(button))
                return true;
            else
                return false;
        }

        public bool StickLeft()
        {
            if ((curr.ThumbSticks.Left.X < 0) && (old.ThumbSticks.Left.X >= 0))
                return true;
            else
                return false;
        }

        public bool StickRight()
        {
            if ((curr.ThumbSticks.Left.X > 0) && (old.ThumbSticks.Left.X <= 0))
                return true;
            else
                return false;
        }

        public void MakeMe()
        {
            old = curr;
            curr = GamePad.GetState(player);
        }

    }



}

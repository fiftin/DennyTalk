using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DennyTalk
{
    class CustomPanel : Panel
    {
        protected override System.Drawing.Point ScrollToControl(System.Windows.Forms.Control activeControl)
        {
            // Returning the current location prevents the panel from
            // scrolling to the active control when the panel loses and regains focus
            return this.DisplayRectangle.Location;
        }
    }
}

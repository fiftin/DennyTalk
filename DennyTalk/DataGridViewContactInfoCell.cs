using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DennyTalk
{
    public class DataGridViewContactInfoCell : DataGridViewTextBoxCell
    {
        
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            try
            {
                PointF p = new PointF(cellBounds.Location.X, cellBounds.Location.Y + 17);

                DataGridViewRow row = this.DataGridView.Rows[rowIndex];
                object obj = row.DataBoundItem;
                ContactInfo cont = (ContactInfo)obj;

                switch (cellState)
                {
                    case DataGridViewElementStates.Selected:
                        break;
                    case DataGridViewElementStates.Displayed:

                        break;
                }

                Color foreColor = Color.Gray;
                if ((cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                {
                    foreColor = Color.Yellow;
                }

                graphics.DrawString(cont.StatusText, new Font("Arial", 8), new SolidBrush(foreColor), p);
            } catch { }
        }

    }
}

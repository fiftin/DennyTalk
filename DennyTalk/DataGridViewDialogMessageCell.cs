using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DennyTalk
{
    public class DataGridViewDialogMessageCell : DataGridViewTextBoxCell
    {
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            Message msg = (Message)DataGridView.Rows[rowIndex].DataBoundItem;
            Brush br;
            if (msg.Direction == HistoryMessageDirection.In)
            {
                br = Brushes.Blue;
            }
            else
            {
                br = Brushes.Red;
            }
            Font font = new Font("Arial", 9, FontStyle.Bold);
            string nick = msg.SenderNick;
            if (string.IsNullOrEmpty(nick))
            {
                nick = msg.SenderAddress.Host;
            }
            graphics.DrawString(msg.SenderNick, font, br, cellBounds.X + 2, cellBounds.Y + 5);
            SizeF nickSize = graphics.MeasureString(msg.SenderNick, font);
            if (msg.Delivered)
                graphics.DrawImage(ImageHelper.Tick, cellBounds.X + 2 + nickSize.Width, cellBounds.Y);

            SizeF textSize = graphics.MeasureString(msg.Text, font, cellBounds.Width);

            int height = (int)Math.Ceiling(textSize.Height) + 20;
            if (height < 54)
            {
                height = 54;
            }
            this.DataGridView.Rows[rowIndex].Height = height;
        }


    }
}

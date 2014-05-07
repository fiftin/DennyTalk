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
            Brush br = msg.Direction == HistoryMessageDirection.In ? Brushes.Blue : Brushes.Red;
            Font font = new Font("Courier new", 9, FontStyle.Bold);
            string nick = string.IsNullOrEmpty(msg.SenderNick) ? msg.SenderAddress.Host : msg.SenderNick;
            SizeF nickSize = graphics.MeasureString(msg.SenderNick, font);
            SizeF textSize = graphics.MeasureString(msg.Text, font, cellBounds.Width);
            

            graphics.DrawString(msg.SenderNick, font, br, cellBounds.X + 2, cellBounds.Y + 5);
            if (msg.Delivered)
                graphics.DrawImage(ImageHelper.Tick, cellBounds.X + 2 + nickSize.Width + 5, cellBounds.Y);


            int height = (int)Math.Ceiling(textSize.Height) + 20;
            if (height < 52)
                height = 52;
            this.DataGridView.Rows[rowIndex].Height = height;
        }
    }
}

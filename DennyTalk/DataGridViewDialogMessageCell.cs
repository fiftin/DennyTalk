using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DennyTalk
{
    public class DataGridViewDialogMessageCell : DataGridViewTextBoxCell
    {
        private const int AddHeight = 25;
        private const int MinHeight = 52;

        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            Message msg = (Message)DataGridView.Rows[rowIndex].DataBoundItem;

            // Отрисовка заголовка
            Brush br = msg.Direction == MessageDirection.In ? Brushes.Blue : Brushes.Red;
            Font nickFont = new Font("Courier new", 9, FontStyle.Bold);
            Font font = new Font("Courier new", 8, FontStyle.Regular);
            string nick = string.IsNullOrEmpty(msg.SenderNick) ? msg.FromAddress.Host : msg.SenderNick;
            SizeF nickSize = graphics.MeasureString(msg.SenderNick, nickFont);
            graphics.DrawString(msg.SenderNick, nickFont, br, cellBounds.X + 2, cellBounds.Y + 5);
            if (msg.Delivered)
                graphics.DrawImage(ImageHelper.Tick, cellBounds.X + 2 + nickSize.Width + 5, cellBounds.Y);

            SizeF textSize = graphics.MeasureString(msg.Text, nickFont, cellBounds.Width);
            int height = 0;
            // Высота строки
            if (msg.Type == MessageType.Message)
            {
                height = (int)Math.Ceiling(textSize.Height) + AddHeight;
            }
            else if (msg.Type == MessageType.FilesRequest)
            {
                if (msg.Direction == MessageDirection.In)
                {
                    DennyTalk.DialogManager.FilePortRequestInfo req = (DennyTalk.DialogManager.FilePortRequestInfo)msg.Tag;
                    if (!req.IsAcknowledged)
                    {
                        height = (int)Math.Ceiling(textSize.Height) + 50;
                        graphics.FillRectangle(Brushes.LightGray, cellBounds.X + 10, cellBounds.Y + height - 25, 40, 15);
                        graphics.DrawString("OK", font, Brushes.Black, cellBounds.X + 10, cellBounds.Y + height - 25);
                        graphics.FillRectangle(Brushes.LightGray, cellBounds.X + 60, cellBounds.Y + height - 25, 40, 15);
                        graphics.DrawString("Cancel", font, Brushes.Black, cellBounds.X + 60, cellBounds.Y + height - 25);
                    }
                    else
                    {
                        height = (int)Math.Ceiling(textSize.Height) + AddHeight;
                    }
                }
            }
            else if (msg.Type == MessageType.Files)
            {
                FileTransferClient client = (FileTransferClient)msg.Tag;
                string[] downloaded = client.GetLoadedFiles();
                for (int i = 0; i < downloaded.Length; i++)
                {
                    int y = cellBounds.Y + 20 + 14 + i * 14;
                    int x = cellBounds.X + 20;
                    graphics.FillRectangle(Brushes.Blue, x, y, cellBounds.Width - 40, 14);
                    graphics.DrawImage(ImageHelper.Tick, x + cellBounds.Width - 35, y, 15, 15);
                    graphics.DrawString(downloaded[i], font, Brushes.Black, x, y);
                }
                if (client.CurrentFileName != null)
                {
                    int y = cellBounds.Y + 20 + 14 + client.CurrentFileNumber * 14;
                    int x = cellBounds.X + 20;
                    graphics.FillRectangle(Brushes.White, x, y, cellBounds.Width - 40, 14);
                    graphics.FillRectangle(Brushes.Blue, x, y, (cellBounds.Width - 40) * client.CurrentFileLoadingPercent / 100, 14);
                    graphics.DrawString(client.CurrentFileName, font, Brushes.Black, x, y);
                }
                height = (int)Math.Ceiling(textSize.Height) + AddHeight;
            }
            if (height < MinHeight)
                height = MinHeight;
            this.DataGridView.Rows[rowIndex].Height = height;
        }

        public Message Message { get; set; }
    }
}

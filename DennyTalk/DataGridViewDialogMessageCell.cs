using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DennyTalk
{
    public class DataGridViewDialogMessageCell : DataGridViewTextBoxCell
    {
		public const int ButtonHeight = 15;
		public const int ButtonWidth = 40;
		public const int ButtonMargin = 10;

		public readonly Font NickFont = new Font("Courier new", 9, FontStyle.Bold);

        private const int AddHeight = 25;
        private const int MinHeight = 52;
		private const int TickWidthAndHeight = 15;

		public DataGridViewDialogMessageCell ()
		{
		}

		private void DrawNick (Message msg, System.Drawing.Graphics graphics, System.Drawing.Rectangle cellBounds)
		{
            // Отрисовка заголовка
            Brush br = msg.Direction == MessageDirection.In ? Brushes.Blue : Brushes.Red;
            SizeF nickSize = graphics.MeasureString(msg.SenderNick, NickFont);
            graphics.DrawString(msg.SenderNick, NickFont, br, cellBounds.X + 2, cellBounds.Y + 5);
            if (msg.Delivered)
                graphics.DrawImage(ImageHelper.Tick, cellBounds.X + 2 + nickSize.Width + 5, cellBounds.Y);
		}

		private void DrawText ()
		{
		}

        protected override void Paint (System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			Brush selectedBackBrush = new SolidBrush(cellStyle.SelectionBackColor);
			Brush backBrush = new SolidBrush(cellStyle.BackColor);
			Brush selectedForeBrush = new SolidBrush(cellStyle.SelectionForeColor);
			Brush foreBrush = new SolidBrush(cellStyle.ForeColor);
			try {
				Message msg = (Message)DataGridView.Rows [rowIndex].DataBoundItem;
				Brush currentBackBrush = backBrush;
				if ((cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
					currentBackBrush = selectedBackBrush;
				Brush currentForeBrush = foreBrush;
				if ((cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
					currentForeBrush = selectedForeBrush;

				//
				// Draw text
				//
				if (Common.CommonUtil.IsWindows) {
					base.Paint (graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
				} else {
					graphics.FillRectangle (currentBackBrush, cellBounds);
					graphics.DrawString ((string)formattedValue, cellStyle.Font, currentForeBrush, 
                     new RectangleF (cellBounds.X + cellStyle.Padding.Left, cellBounds.Y + cellStyle.Padding.Top,
  						   cellBounds.Width - cellStyle.Padding.Horizontal, 0)
					);
				}

				DrawNick (msg, graphics, cellBounds);

				SizeF textSize = graphics.MeasureString (msg.Text, cellStyle.Font, cellBounds.Width);

				int height = (int)Math.Ceiling (textSize.Height) + cellStyle.Padding.Top;
				Font buttonFont = cellStyle.Font;
				int charHeight = (int)Math.Ceiling (cellStyle.Font.GetHeight());

				if (msg.Type == MessageType.FilesRequest) {
					//
					// Draw input request
					//
					if (msg.Direction == MessageDirection.In) {
						DennyTalk.DialogManager.FilePortRequestInfo req = (DennyTalk.DialogManager.FilePortRequestInfo)msg.Tag;
						string s = string.Format ("Received request for transfering {0} file(s):", req.NumberOfFiles);
						int sHeight = (int)graphics.MeasureString(s, cellStyle.Font).Height;
						graphics.DrawString (s, cellStyle.Font, currentForeBrush, cellBounds.Left, cellBounds.Top + cellStyle.Padding.Top);

						height = sHeight + cellStyle.Padding.Top;

						if (!req.IsAcknowledged) {

							height += ButtonHeight + ButtonMargin * 2;

							graphics.FillRectangle (Brushes.LightGray, 
						                       cellBounds.X + ButtonMargin,
						                       cellBounds.Y + height - ButtonHeight - ButtonMargin, 
						                       ButtonWidth, ButtonHeight);

							graphics.DrawString ("OK", buttonFont, Brushes.Black, 
						                    cellBounds.X + ButtonMargin,
						                    cellBounds.Y + height - ButtonHeight - ButtonMargin);

							graphics.FillRectangle (Brushes.LightGray,
						                       cellBounds.X + ButtonMargin * 2 + ButtonWidth,
						                       cellBounds.Y + height - ButtonHeight - ButtonMargin,
						                       ButtonWidth, ButtonHeight);

							graphics.DrawString ("Cancel", buttonFont, Brushes.Black,
						                    cellBounds.X + ButtonMargin * 2 + ButtonWidth,
						                    cellBounds.Y + height - ButtonHeight - ButtonMargin);
						}
					}
				} else if (msg.Type == MessageType.Files) {
					//
					// Draw received/sent files
					//
					FileTransferClient client = (FileTransferClient)msg.Tag;
					string[] downloaded = client.GetLoadedFiles ();
					string s = msg.Direction== MessageDirection.In ? "Receiving file(s):" : "Sending file(s):";
					int sHeight = (int)graphics.MeasureString(s, cellStyle.Font).Height;
					graphics.DrawString (s, cellStyle.Font, currentForeBrush, cellBounds.Left, cellBounds.Top + cellStyle.Padding.Top);

					for (int i = 0; i < downloaded.Length; i++) {
						int y = cellBounds.Y + cellStyle.Padding.Top + charHeight + i * charHeight;
						int x = cellBounds.X;
						int rectOffsetX = (int)graphics.MeasureString(string.Format("{0}. ", i+1), cellStyle.Font).Width;
						graphics.FillRectangle (Brushes.BlueViolet, x + rectOffsetX, y, 
						                        cellBounds.Width - rectOffsetX - TickWidthAndHeight,
						                        charHeight);
						graphics.DrawImage (ImageHelper.Tick,
						                    x + cellBounds.Width - rectOffsetX - TickWidthAndHeight,
						                    y,
						                    TickWidthAndHeight,
						                    TickWidthAndHeight);
						graphics.DrawString (string.Format("{0}. {1}", i + 1, downloaded[i]), cellStyle.Font, currentForeBrush, x, y);
					}
					if (client.CurrentFileName != null) {
						int y = cellBounds.Y + cellStyle.Padding.Top + charHeight + client.CurrentFileNumber * charHeight;
						int x = cellBounds.X;
						int rectOffsetX = (int)graphics.MeasureString(string.Format("{0}. ", client.CurrentFileNumber+1), cellStyle.Font).Width;
						graphics.FillRectangle (Brushes.White, x + rectOffsetX, y, cellBounds.Width - rectOffsetX - TickWidthAndHeight, charHeight);
						graphics.FillRectangle (Brushes.BlueViolet, x + rectOffsetX, y, (cellBounds.Width - rectOffsetX - TickWidthAndHeight) * client.CurrentFileLoadingPercent / 100, charHeight);
						graphics.DrawString (client.CurrentFileName, buttonFont, Brushes.Black, x, y);
					}
					FileSenderConnection conn = client as FileSenderConnection;
					if (conn != null) {
						conn = (FileSenderConnection)client;
						for (int i = client.CurrentFileNumber + 1; i < conn.FileNames.Length; i++) {
							int y = cellBounds.Y + cellStyle.Padding.Top + charHeight + i * charHeight;
							int x = cellBounds.X;
							graphics.DrawString (string.Format("{0}. {1}", i + 1, System.IO.Path.GetFileName(conn.FileNames[i])), cellStyle.Font, currentForeBrush, x, y);
						}
					}

					height = sHeight + (conn==null ? downloaded.Length : conn.FileNames.Length) * charHeight + cellStyle.Padding.Top; //Math.Max(sHeight + (conn==null ? downloaded.Length : conn.FileNames.Length) * charHeight + cellStyle.Padding.Top, height);

					if (client.IsFinished) {
						height += ButtonMargin * 2 + charHeight;
						string str;
						if (client.IsCanceled)
							str = "File transfering canceled";
						else if (client.IsRejected)
							str = "File transfering rejected remote user";
						else
							str = "File transfering finished succesfull";
						graphics.DrawString (str, buttonFont, Brushes.BlueViolet, cellBounds.X, cellBounds.Y + height - charHeight - ButtonMargin);
					} else {
						height += ButtonMargin * 2 + ButtonHeight;
						graphics.FillRectangle (Brushes.LightGray, cellBounds.X + ButtonMargin, cellBounds.Y + height - ButtonMargin - ButtonHeight, ButtonWidth, ButtonHeight);
						graphics.DrawString ("Cancel", buttonFont, Brushes.Black, cellBounds.X + ButtonMargin, cellBounds.Y + height - ButtonMargin - ButtonHeight);
					}
				}
				if (height < MinHeight)
					height = MinHeight;
				this.DataGridView.Rows [rowIndex].Height = height;
			} finally {
				selectedBackBrush.Dispose();
				backBrush.Dispose();
				selectedForeBrush.Dispose();
				foreBrush.Dispose();
			}
        }

        public Message Message { get; set; }
    }
}

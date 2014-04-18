using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DennyTalk
{
    public class DataGridViewDialogMessageColumn : DataGridViewColumn
    {
        public DataGridViewDialogMessageColumn()
        {
            this.HeaderText = "";
            CellTemplate = new DataGridViewDialogMessageCell();
        }
    }
}

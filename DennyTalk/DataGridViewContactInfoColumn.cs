using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DennyTalk
{
    public class DataGridViewContactInfoColumn : DataGridViewColumn
    {
        public DataGridViewContactInfoColumn()
        {
            this.HeaderText = "";
            CellTemplate = new DataGridViewContactInfoCell();
            DataPropertyName = "";
        }

    }
}

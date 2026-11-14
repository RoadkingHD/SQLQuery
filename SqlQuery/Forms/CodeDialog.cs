using System;
using System.Windows.Forms;

namespace SqlQuery.Forms
{
    public partial class CodeDialog : Form
    {
        public CodeDialog()
        {
            InitializeComponent();
        }

        public string DialogTitle
        {
            get { return Text; }
            set { Text = value; }
        }

        public string CodeText
        {
            get { return txtCode.Text; }
            set { txtCode.Text = value ?? string.Empty; }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtCode.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Could not access the clipboard.", "Copy",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

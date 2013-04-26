using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class RootForm : Form
    {
        public RootForm()
        {
            InitializeComponent();
        }

        public ProjectEntity project = new ProjectEntity();

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateProjectForm form = new CreateProjectForm();
            form.MdiParent = this;
            form.Show();
        }

        private void оТкрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm OpForm = new OpenForm();
            OpForm.MdiParent = this;
            OpForm.Show();
            
        }


        private void newCSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeCSS form = new ChangeCSS();
            form.MdiParent = RootForm.ActiveForm;
            form.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EzMotion.clsGlVariable;

namespace EzMotion.Conrorller
{
    public class clsMainController
    {
        private frmMain view;

        public clsMainController()
        {

            view = Gform.fmain;

            prepareMainFormComponents();

        }
        public void prepareMainFormComponents()
        {
            Gform.fmain.getMainPanel().Controls.Clear();
          
            configureForms(Gform.fMove);
            configureForms(Gform.fParameter);
        }
        private void configureForms(Form form)
        {
           form.Owner = Gform.fmain;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            Gform.fmain.getMainPanel().Controls.Add(form);


        }
 
    }
}

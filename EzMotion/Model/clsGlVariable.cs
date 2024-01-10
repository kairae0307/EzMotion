using EzMotion.Conrorller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzMotion
{
    public class clsGlVariable
    {
        #region 폼 전역변수 
        public class Gform
        {
            public static frmMain fmain = new frmMain();
            public static frmMove fMove = new frmMove();
            public static frmParameter fParameter = new frmParameter();
        }
        /// <summary>
        /// 싱틀 톤 패턴 single tone pattern
        /// </summary>
        private static clsGlVariable _default;
        public static clsGlVariable Default
        {
            get
            {
                if (_default == null)
                    _default = new clsGlVariable();
                return _default;
            }
        }

        #endregion
        public class Gcont
        {
            public static clsMainController cMainController;
        }
        public void cleanModel()
        {
            Gform.fmain = null;
            Gform.fMove = null;
            Gform.fParameter = null;

            Gcont.cMainController = null;


            GC.Collect();

            Gform.fmain = new frmMain();
            Gform.fMove = new frmMove();
            Gform.fParameter = new frmParameter();
            Gcont.cMainController.prepareMainFormComponents();
        }
        public uint ConvertComboToAxm(ref ComboBox pCboItem)
        {
            if (pCboItem == null) return 0;

            String strText;

            int iCount, iSeldate;

            iCount = pCboItem.Items.Count;
            if (iCount == 0) return 0;
            iSeldate = pCboItem.SelectedIndex;
            if (iSeldate < 0) return 0;

            strText = pCboItem.Text.Substring(0, 2);
            return Convert.ToUInt32(strText);
        }
    }
}

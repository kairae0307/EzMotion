using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;
using static EzMotion.clsGlVariable;
using EzMotion.Conrorller;

namespace EzMotion
{
    public partial class frmMain : Form
    {
        public int m_lAxisCounts = 0;                // 제어 가능한 축갯수 선언 및 초기화
        private int m_lAxisNo = 0;                // 제어할 축 번호 선언 및 초기화   
        public uint m_uModuleID = 0;                // 제어할 축의 모듈 I/O 선언 및 초기화
        public uint m_duOldResult = 0;                // tmTimer에서 사용할 이전 Command Position 선언 및 초기화
        String m_strResult;                           // 홈검색 결과를 출력 변수 선언
        public int m_lBoardNo = 0, m_lModulePos = 0;
        private static frmMain m_formMotion;
        private List<Form> tempForms;
        private Thread[] m_hTestThread = new Thread[64];
        private bool[] m_bTestActive = new bool[64];

        public frmMain()
        {
            InitializeComponent();
        }



        private void frmMain_Load(object sender, EventArgs e)
        {
            Gform.fmain = this;
            Gcont.cMainController = new clsMainController();
          


            showView(Gform.fMove, true);
        }


     

        public Panel getMainPanel()
        {
            return mainPanel;
        }

        public void showView(Form form, bool temp)
        {
            if (!mainPanel.Controls.Contains(form))
            {
                MessageBox.Show(form.Name + " is not a child of the frmMain.mainPanel !");
                return;
            }
            if (!temp) // hide all other forms
            {
                for (int j = 0; j < mainPanel.Controls.Count; j++)
                {
                    if (!mainPanel.Controls[j].Equals(form))
                    {
                        mainPanel.Controls[j].Hide();
                    }
                }
            }
   
            form.BringToFront();
            form.Show();
        }

    

        private void button1_Click_1(object sender, EventArgs e)
        {
            showView(Gform.fMove, true);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
              showView(Gform.fParameter, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Gform.fMove.m_Thread  != null)
            {
                Gform.fMove.m_Thread.Abort();
            }
          
            this.BeginInvoke((MethodInvoker)delegate { this.Close(); });
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Gform.fMove.m_Thread != null)
            {
                Gform.fMove.m_Thread.Abort();
               
            }
            CAXL.AxlClose();
            this.BeginInvoke((MethodInvoker)delegate { this.Close(); });
        }



        // ++ =======================================================================
        // >> TranslateHomeResult(...) : 지정한 원점검색 결과에 해당하는 문자열을 반환
        //    하는 함수.
        //  - "AXHS.h"에 정의되어있는 AXT_MOTION_HOME_RESULT 구조체를 기반으로 합니다.
        // --------------------------------------------------------------------------
        public String TranslateHomeResult(uint duHomeResult)
        {
            switch (duHomeResult)
            {
                case (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS: m_strResult = ("[01H] HOME_SUCCESS"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_SEARCHING: m_strResult = ("([02H] HOME_SEARCHING"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_GNT_RANGE: m_strResult = ("[10H] HOME_ERR_GNT_RANGE"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_USER_BREAK: m_strResult = ("[11H] HOME_ERR_USER_BREAK"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_VELOCITY: m_strResult = ("[12H] HOME_ERR_VELOCITY"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_AMP_FAULT: m_strResult = ("[13H] HOME_ERR_AMP_FAULT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NEG_LIMIT: m_strResult = ("[14H] HOME_ERR_NEG_LIMIT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_POS_LIMIT: m_strResult = ("[15H] HOME_ERR_POS_LIMIT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NOT_DETECT: m_strResult = ("[16H] HOME_ERR_NOT_DETECT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_UNKNOWN: m_strResult = ("[FFH] HOME_ERR_UNKNOWN"); break;
            }
            return m_strResult;
        }
    }
}
